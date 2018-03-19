using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace IgiCore.SDK
{
	public abstract class ClientService : IClientService
	{
		public abstract string Name { get; }

		public abstract Task OnTick();

		public void Log(string message)
		{
			Debug.WriteLine(message);
		}

		public async Task Delay(TimeSpan ts)
		{
			await Delay((int)ts.TotalMilliseconds);
		}

		public async Task Delay(int msecs)
		{
			await Task.Delay(msecs);
		}
	}
}
