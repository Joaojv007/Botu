using ApiTcc.Infra.DB.Entities;
using ApiTcc.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBuscarSemestresQueryHandler
    {
        Task<List<Semestre>> Handle(Guid AlunoId, Guid curso);
    }
}
