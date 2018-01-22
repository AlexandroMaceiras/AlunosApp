using System;
using System.Collections.Generic;
using System.Text;

namespace AlunosApp.Classes
{
    public class Materias
    {
       
        public int GrupoId { get; set; }

        public string Descricao { get; set; }

        public int UserId { get; set; }
        

       public override string ToString()
        {
            return Descricao;
        }


    }
}
