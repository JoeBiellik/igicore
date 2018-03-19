using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace IgiCore.SDK.Migrations
{
	public sealed class Configuration<T> : DbMigrationsConfiguration<T> where T : DbContext
	{
		public Configuration()
		{
			this.TargetDatabase = new System.Data.Entity.Infrastructure.DbConnectionInfo(Config.MySqlConnString, "MySql.Data.MySqlClient");
			this.AutomaticMigrationsEnabled = true;
		}
	}
}
