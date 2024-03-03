using ApiTcc.Infra.DB;
using Application.Integracoes.Command;
using Application.Integracoes.Queries;
using Application.Interfaces;
using Infra;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.MySql;
using Application.Alunos.Queries;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
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
app.UseAuthorization();

app.MapControllers();

app.Run();
