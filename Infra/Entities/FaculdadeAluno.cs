﻿namespace ApiTcc.Infra.DB.Entities
{
    public class FaculdadeAluno : EntityBase
    {
        public Faculdade Faculdade{ get; set; }
        public Aluno Aluno { get; set; }
    }
}
