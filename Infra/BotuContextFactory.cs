using ApiTcc.Infra.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Infra
{
    public class BotuContextFactory : IDesignTimeDbContextFactory<BotuContext>
    {
        public BotuContext CreateDbContext(string[] args)
        {
            // Obter o diretório onde o comando 'dotnet ef' está sendo executado
            var baseDirectory = Directory.GetCurrentDirectory();

            // Construir o caminho completo para o arquivo de configuração
            var configPath = Path.Combine(baseDirectory, "..", "..", "Botu", "ApiTcc", "appsettings.json");

            // Carregar a configuração do arquivo
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(configPath)
                .Build();

            // Obter a string de conexão do arquivo de configuração
            var connectionString = configuration.GetConnectionString("BotuContext");

            // Construir as opções do DbContext
            var optionsBuilder = new DbContextOptionsBuilder<BotuContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new BotuContext(optionsBuilder.Options);
        }
    }
}
