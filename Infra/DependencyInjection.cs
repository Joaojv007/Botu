using Hangfire;
using Hangfire.Dashboard;
using Infra.Hangfire.Jobs;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra
{
    public static class DependencyInjection
    {

        public static IApplicationBuilder UseHanfire(this IApplicationBuilder app)
        {
            ConfigurarAutorizacaoAcessoDashboard(app);
            CriarTarefasRecorrentes();
            return app;
        }

        private static void ConfigurarAutorizacaoAcessoDashboard(IApplicationBuilder app)
        {
            app.UseHangfireDashboard("/hangfire", new DashboardOptions() { Authorization = new[] { new Infra.Hangfire.AuthorizationFilter() } });
        }

        public static void CriarTarefasRecorrentes()
        {
            var options = new RecurringJobOptions { TimeZone = TimeZoneInfo.Local };

            RecurringJob.AddOrUpdate<SigaNotasJob>(
                recurringJobId: "siga_notas_job",
                methodCall: sigaNotasJob => sigaNotasJob.Run(),
                cronExpression: "* * * * *",
                options: options
            );
        }
    }
}
