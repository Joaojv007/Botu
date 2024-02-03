using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Hangfire.Jobs
{
    public class SigaNotasJob
    {
        public async Task<GenericCommandResult> Run()
        {
            return new GenericCommandResult(true, "teste", "teste");
        }
    }
}
