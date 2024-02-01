using ApiTcc.Infra.DB;
using Application.Integracoes.Command;
using Application.Integracoes.Queries;
using Application.Interfaces;
using Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
