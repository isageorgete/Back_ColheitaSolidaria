using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace YourProjectNamespace.Middlewares
{
	public class RequestLoggingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<RequestLoggingMiddleware> _logger;

		public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			var sw = Stopwatch.StartNew();
			var method = context.Request.Method;
			var path = context.Request.Path;

			_logger.LogInformation("Incoming request {Method} {Path}", method, path);

			await _next(context); // executar pipeline

			sw.Stop();
			var status = context.Response?.StatusCode;
			_logger.LogInformation("Finished {Method} {Path} responded {StatusCode} in {Elapsed}ms", method, path, status, sw.ElapsedMilliseconds);
		}
	}
}
