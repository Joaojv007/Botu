using ApiTcc.Infra.DB;
using Application.Integracoes.Command;
using Application.Integracoes.Queries;
using Application.Interfaces;
using Infra;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.MySql;
using Application.Alunos.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infra.Criptografia;
using Application.Login.Command;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Infra.Email;
using Infra.Services;
using Infra.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder => builder.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

string connectionString = builder.Configuration.GetConnectionString("BotuContext");
builder.Services.AddDbContext<BotuContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IBotuContext, BotuContext>();
builder.Services.AddScoped<IAdicionarIntegracaoCommandHandler, AdicionarIntegracaoCommandHandler>();
builder.Services.AddScoped<IBuscarIntegracoesQueryHandler, BuscarIntegracoesQueryHandler>();
builder.Services.AddScoped<IBuscarAlunoQueryHandler, BuscarAlunoQueryHandler>();
builder.Services.AddScoped<IBuscarDisciplinasQueryHandler, BuscarDisciplinasQueryHandler>();
builder.Services.AddScoped<IBuscarSemestresQueryHandler, BuscarSemestresQueryHandler>();
builder.Services.AddScoped<IBuscarCursosQueryHandler, BuscarCursosQueryHandler>();
builder.Services.AddScoped<IAdicionarLoginCommandHandler, AdicionarLoginCommandHandler>();
builder.Services.AddScoped<IGetUserCommandHandler, GetUserCommandHandler>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IRecuperarSenhaCommandHandler, RecuperarSenhaCommandHandler>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseStorage(
        new MySqlStorage(
            connectionString,
            new MySqlStorageOptions
            {
                QueuePollInterval = TimeSpan.FromSeconds(10),
                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                PrepareSchemaIfNecessary = true,
                DashboardJobListLimit = 25000,
                TransactionTimeout = TimeSpan.FromMinutes(1),
                TablesPrefix = "Hangfire",
            }
        )
    ));

builder.Services.AddHangfireServer();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddMvc(config =>
{
    var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
}).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

//builder.Services.AddControllersWithViews(options =>
//{
//    options.Filters.Add<GlobalSampleActionFilter>();
//});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger";
    });
}

app.UseCors("AllowOrigin");

app.UseHttpsRedirection();

app.UseHanfire();
app.MapHangfireDashboard();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
