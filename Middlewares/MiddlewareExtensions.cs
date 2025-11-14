using Microsoft.AspNetCore.Builder;

namespace YourProjectNamespace.Middlewares
{
	public static class MiddlewareExtensions
	{
		public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
		{
			return app.UseMiddleware<RequestLoggingMiddleware>();
		}

		public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
		{
			return app.UseMiddleware<ExceptionHandlingMiddleware>();
		}
	}
}
