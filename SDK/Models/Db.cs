using System;
using System.Data.Entity;
using IgiCore.SDK.Migrations;

namespace IgiCore.SDK.Models
{
	public class Db<T> : DbContext where T : DbContext
	{
		public Db() : base(Config.MySqlConnString)
		{
			this.Database.Log = Console.Write;

			Database.SetInitializer(new MigrateDatabaseToLatestVersion<T, Configuration<T>>());
		}
	}
}
