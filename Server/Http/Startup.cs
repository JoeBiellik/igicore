using Owin;

namespace IgiCore.Server.Http
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.Run(async context =>
			{
				context.Response.ContentType = "text/plain";
				await context.Response.WriteAsync("Hello World");
			});
		}
	}
}
