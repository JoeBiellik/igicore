using System;
using System.Threading.Tasks;

namespace IgiCore.Plugins.Demo
{
	public class ClientService : SDK.ClientService
	{
		public override string Name => "Demo Client plugin";

		public override async Task OnTick()
		{
			this.Log("Test");

			await Delay(TimeSpan.FromSeconds(5));
		}
	}
}
