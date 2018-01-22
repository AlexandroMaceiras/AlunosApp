using System;
using System.Collections.Generic;
using System.Text;

namespace AlunosApp.Classes
{
   public class Professor
    {
        public int GrupoId { get; set; }
        public string Descricao { get; set; }
        public User professor { get; set; }

        public float Nota { get; set; }

        public override string ToString()
        {
            return Descricao;
        }
    }
}
