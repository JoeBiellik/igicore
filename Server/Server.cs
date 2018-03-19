﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Reflection;
using CitizenFX.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.SDK;
using IgiCore.Server.Models;
using IgiCore.Server.Storage.MySql;
using Newtonsoft.Json;
using Citizen = CitizenFX.Core.Player;
using IgiCore.Server.Models.Objects.Vehicles;
using Newtonsoft.Json.Linq;

namespace IgiCore.Server
{
	public partial class Server : BaseScript
	{
		protected List<ServerService> Plugins;
		public static DB Db;

		public Server()
		{
			Db = new DB();

			this.Plugins = LoadPlugins();

			Db.Database.CreateIfNotExists();

			//HandleEvent<string>("onResourceStarting", r => Debug.WriteLine($"Starting resource: {r}"));
			//HandleEvent<string>("onResourceStart", r => Debug.WriteLine($"Start resource: {r}"));
			//HandleEvent<string>("onResourceStop", r => Debug.WriteLine($"Stop resource: {r}"));

			HandleEvent<Citizen, string, CallbackDelegate>("playerConnecting", OnPlayerConnecting);
			HandleEvent<Citizen, string, CallbackDelegate>("playerDropped", OnPlayerDropped);

			HandleEvent<int, string, string>("chatMessage", OnChatMessage);

			HandleEvent<Citizen>("igi:user:load", User.Load);
			HandleEvent<string>("igi:character:save", Character.Save);
			HandleJsonEvent<Car>("igi:vehicle:save", VehicleExtensions.Save);
		}

		private List<ServerService> LoadPlugins()
		{
			var plugins = new List<ServerService>();

			foreach (var file in Directory.EnumerateFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources", "igicore-fork", "plugins"), "*.dll", SearchOption.TopDirectoryOnly))
			{
				var asm = Assembly.LoadFrom(file);

				var type = asm.GetTypes().FirstOrDefault(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ServerService)));
				if (type == null) continue;

				var plugin = (ServerService)JsonConvert.DeserializeObject("{}", type); // TODO: Settings
				if (plugin == null) continue;

				plugin.Logger = s => Log(s);
				plugin.DbConn = Config.MySqlConnString;

				plugin.Initilize();

				Log($"Loaded \"{plugin.Name}\" from {file}");

				foreach (var e in plugin.Events)
				{
					Log($"\tAdding event \"{e.Key}\"");
					this.EventHandlers[e.Key] += e.Value;
				}

				plugins.Add(plugin);
			}

			return plugins;
		}

		private static Character NewCharCommand(Citizen citizen, string charName)
		{
			User user = User.GetOrCreate(citizen);
			if (user.Characters == null) user.Characters = new List<Character>();

			Character character = new Character
			{
				Name = charName,
				Style = new Core.Models.Appearance.Style { Id = GuidGenerator.GenerateTimeBasedGuid() }
			};

			user.Characters.Add(character);

			Db.Users.AddOrUpdate(user);
			Db.SaveChanges();

			return character;
		}

		private static Character GetCharCommand(Citizen citizen, string name) => User.GetOrCreate(citizen).Characters.FirstOrDefault(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

		private static Character GetCharCommand(Citizen citizen, Guid charId) => User.GetOrCreate(citizen).Characters.FirstOrDefault(c => c.Id == charId);

		[System.Diagnostics.Conditional("DEBUG")]
		public static void Log(string message)
		{
			Debug.WriteLine($"{DateTime.Now:s} [SERVER]: {message}");
		}

		public void HandleEvent(string name, Action action) => this.EventHandlers[name] += action;
		public void HandleEvent<T1>(string name, Action<T1> action) => this.EventHandlers[name] += action;
		public void HandleEvent<T1, T2>(string name, Action<T1, T2> action) => this.EventHandlers[name] += action;
		public void HandleEvent<T1, T2, T3>(string name, Action<T1, T2, T3> action) => this.EventHandlers[name] += action;
		public void HandleJsonEvent<T>(string eventName, Action<T> action) => this.EventHandlers[eventName] += new Action<string>(json => action(JsonConvert.DeserializeObject<T>(json)));
		public void HandleJsonEvent<T1, T2>(string eventName, Action<T1, T2> action) => this.EventHandlers[eventName] += new Action<string, string>((j1, j2) => action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2)));
		public void HandleJsonEvent<T1, T2, T3>(string eventName, Action<T1, T2, T3> action) => this.EventHandlers[eventName] += new Action<string, string, string>((j1, j2, j3) => action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2), JsonConvert.DeserializeObject<T3>(j3)));
	}
}
