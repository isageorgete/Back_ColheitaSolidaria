using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Office.Interop.Outlook;
using Exception = System.Exception;

namespace YourProjectNamespace.Middlewares
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;
		private readonly IHostEnvironment _env;

		public ExceptionHandlingMiddleware(RequestDelegate next,
										   ILogger<ExceptionHandlingMiddleware> logger,
										   IHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			// Log completo (stacktrace) para diagnostico
			_logger.LogError(exception, "Unhandled exception occurred while processing request {Method} {Path}",
							 context.Request.Method, context.Request.Path);

			int statusCode = (int)HttpStatusCode.InternalServerError;
			string title = "Erro interno";
			string detail = "Ocorreu um erro inesperado.";

			// Em desenvolvimento podemos fornecer mais detalhes (cuidado em produção)
			var responseObj = new
			{
				error = new
				{
					title,
					detail = _env.IsDevelopment() ? detail : "Se o problema persistir, contate o suporte.",
					// opcional: incluir stacktrace só em dev
					stackTrace = _env.IsDevelopment() ? exception.StackTrace : null
				},
				timestamp = DateTime.UtcNow
			};

			var result = JsonSerializer.Serialize(responseObj, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = statusCode;
			await context.Response.WriteAsync(result);
		}
	}
}
