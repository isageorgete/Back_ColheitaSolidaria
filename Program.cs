using Back_ColheitaSolidaria.Data;
using Back_ColheitaSolidaria.Middlewares;
using Back_ColheitaSolidaria.Profiles;
using Back_ColheitaSolidaria.Services.Doacoes;
using Back_ColheitaSolidaria.Services.Solicitacoes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using YourProjectNamespace.Middlewares;


var builder = WebApplication.CreateBuilder(args);

// =======================
// CONFIGURAÇÕES DE BANCO
// =======================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// =======================
// CONFIGURAÇÃO AUTOMAPPER
// =======================
builder.Services.AddAutoMapper(typeof(SolicitacaoProfile));

// =======================
// REGISTRO DOS SERVICES
// =======================
builder.Services.AddScoped<SolicitacaoService>();
builder.Services.AddScoped<DoacaoService>();

// =======================
// CONFIGURAÇÃO DO CORS
// =======================
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins, policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", "https://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// =======================
// CONTROLLERS E SWAGGER
// =======================
builder.Services.AddControllers(options =>
{
    options.MaxModelBindingCollectionSize = int.MaxValue;
}).AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Colheita Solidária API",
        Version = "v1",
        Description = "API do projeto Colheita Solidária com autenticação JWT"
    });

    // 🔐 Configuração do Swagger para autenticação JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// =======================
// CONFIGURAÇÃO JWT
// =======================
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // ✅ permite HTTP local
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),

        ClockSkew = TimeSpan.Zero // ✅ sem tolerância extra
    };

    // Log opcional para debug
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"❌ Erro de autenticação JWT: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine($"✅ Token válido para: {context.Principal.Identity?.Name}");
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
            Console.WriteLine($"🔐 Header recebido: {context.Request.Headers["Authorization"]}");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(); // sem política global


// =======================
// BUILD E PIPELINE
// =======================
var app = builder.Build();

// Configuração de porta
app.Urls.Clear();
app.Urls.Add("http://localhost:7100");

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Colheita Solidária API V1");
});

// Pipeline de middlewares
app.UseCors(myAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
// 🔹 Middleware de autorização personalizada
//app.UseRoleAuthorization();


// ----------------------
// Middleware para debug das claims
// ----------------------
app.Use(async (context, next) =>
{
    Console.WriteLine($"🔐 Header recebido: {context.Request.Headers["Authorization"]}");
    if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
    {
        foreach (var claim in context.User.Claims)
        {
            Console.WriteLine($"Claim {claim.Type} = {claim.Value}");
        }
    }
    await next();
});

app.UseRequestLogging();
app.UseExceptionHandling();

app.MapControllers();

app.Run();
