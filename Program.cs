using Back_ColheitaSolidaria.Data;
using Back_ColheitaSolidaria.Profiles;
using Back_ColheitaSolidaria.Services.Doacoes;
using Back_ColheitaSolidaria.Services.Solicitacoes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ----------------------
// Configura o DbContext para SQL Server
// ----------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ----------------------
// Configura AutoMapper
// ----------------------
builder.Services.AddAutoMapper(typeof(SolicitacaoProfile));

// ----------------------
// Registra os Services
// ----------------------
builder.Services.AddScoped<SolicitacaoService>();
builder.Services.AddScoped<DoacaoService>();

// ----------------------
// Configuração do CORS
// ----------------------
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173") // front-end
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// ----------------------
// Configura controllers e Swagger
// ----------------------
builder.Services.AddControllers(options =>
{
    // Permite upload de arquivos grandes
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

    // Configuração JWT no Swagger
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

// ----------------------
// Configuração JWT
// ----------------------
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// ----------------------
// Build da aplicação
// ----------------------
var app = builder.Build();

// ----------------------
// Configura URLs explícitas
// ----------------------
app.Urls.Clear();
app.Urls.Add("http://localhost:7100");
app.Urls.Add("https://localhost:5144"); // HTTPS opcional, só funciona se o certificado estiver confiável

// ----------------------
// Swagger
// ----------------------
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Colheita Solidária API V1");
});

// ----------------------
// Pipeline de middleware
// ----------------------
app.UseHttpsRedirection(); // mantém HTTPS
app.UseCors(myAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
