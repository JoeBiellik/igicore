using System;
using IgiCore.SDK;

namespace IgiCore.Plugins.Demo
{
	public class DemoServerService : ServerService
	{
		public override string Name => "Demo Server plugin";

		public Database Database { get; set; }

		public override void Initilize()
		{
			base.Initilize();

			this.Database = new Database();

			HandleEvent("demo:server:test", new Action<string>(HandleTest));
		}

		private void HandleTest(string test)
		{
			Log($"{this.Name}: {test}");

			this.Database.Demos.Add(new Models.Demo
			{
				Id = Guid.NewGuid(),
				Something = test
			});

			this.Database.SaveChanges();
		}

		protected override void Dispose(bool disposing)
		{
			this.Database?.Dispose();
		}
	}
}
