using System.Data.Entity;
using IgiCore.SDK.Models;

namespace IgiCore.Plugins.Demo
{
	public class Database : Db<Database>
	{
		public DbSet<Models.Demo> Demos { get; set; }
	}
}
