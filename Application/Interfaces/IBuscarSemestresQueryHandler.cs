using ApiTcc.Infra.DB.Entities;
using ApiTcc.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBuscarDisciplinasQueryHandler
    {
        Task<List<Disciplina>> Handle(Guid AlunoId, Guid Semestre);
    }
}
