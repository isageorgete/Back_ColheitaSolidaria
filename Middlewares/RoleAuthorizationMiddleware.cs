using System.Security.Claims;
using System.Text.Json;

namespace Back_ColheitaSolidaria.Middlewares
{
    public class RoleAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Permite Swagger, login e registro sem autenticação
            var path = context.Request.Path.Value?.ToLower();
            if (path != null && (
                path.Contains("login") ||
                path.Contains("register") ||
                path.Contains("swagger") ||
                path.Contains("api-docs")
            ))
            {
                await _next(context);
                return;
            }

            // Verifica autenticação
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "Usuário não autenticado." });
                return;
            }

            // Obtém o Role do token JWT
            var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userRole))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new { message = "Perfil do usuário não encontrado." });
                return;
            }

            // 🔒 Aqui você pode definir restrições de acesso por rota
            // Exemplo: somente Admin acessa /api/admin
            if (path.Contains("/api/admin") && userRole != "Admin")
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new { message = "Acesso negado. Somente administradores podem acessar esta rota." });
                return;
            }

            if (path.Contains("/api/colaborador") && userRole != "Colaborador" && userRole != "Admin")
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new { message = "Acesso negado. Apenas Colaboradores ou Admins podem acessar esta rota." });
                return;
            }

            if (path.Contains("/api/recebedor") && userRole != "Recebedor" && userRole != "Admin")
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new { message = "Acesso negado. Apenas Recebedores ou Admins podem acessar esta rota." });
                return;
            }

            // Se passou em todas as verificações, segue o fluxo
            await _next(context);
        }
    }

    // 🔧 Extensão para registrar o middleware
    public static class RoleAuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseRoleAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RoleAuthorizationMiddleware>();
        }
    }
}
