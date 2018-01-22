using System;
using System.Collections.Generic;
using System.Text;

namespace AlunosApp.Classes
{
    public class estudantesResponse
    {
        public int GruposDetalhesId { get; set; }
        public int GrupoId { get; set; }
        public User Estudante { get; set; }

        public float Nota { get; set; }

        public override string ToString()
        {
            return Estudante.NomeCompleto;
        }
    }
}
