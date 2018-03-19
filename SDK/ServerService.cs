using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IgiCore.SDK
{
	public abstract class ServerService : IServerService, IDisposable
	{
		public Dictionary<string, Delegate> Events { get; } = new Dictionary<string, Delegate>();
		public Action<string> Logger { get; set; } = null;
		public string DbConn { get; set; }

		public abstract string Name { get; }

		public virtual void Initilize()
		{
			Config.MySqlConnString = this.DbConn;
		}

		public void Log(string message)
		{
			this.Logger?.Invoke(message);
		}

		public void HandleEvent(string eventName, Action action) => this.Events.Add(eventName, action);
		public void HandleEvent<T1>(string eventName, Action<T1> action) => this.Events.Add(eventName, action);
		public void HandleEvent<T1, T2>(string eventName, Action<T1, T2> action) => this.Events.Add(eventName, action);
		public void HandleEvent<T1, T2, T3>(string eventName, Action<T1, T2, T3> action) => this.Events.Add(eventName, action);
		public void HandleJsonEvent<T>(string eventName, Action<T> action) => this.Events.Add(eventName, new Action<string>(json => action(JsonConvert.DeserializeObject<T>(json))));
		public void HandleJsonEvent<T1, T2>(string eventName, Action<T1, T2> action) => this.Events.Add(eventName, new Action<string, string>((j1, j2) => action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2))));
		public void HandleJsonEvent<T1, T2, T3>(string eventName, Action<T1, T2, T3> action) => this.Events.Add(eventName, new Action<string, string, string>((j1, j2, j3) => action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2), JsonConvert.DeserializeObject<T3>(j3))));

		protected abstract void Dispose(bool disposing);

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
