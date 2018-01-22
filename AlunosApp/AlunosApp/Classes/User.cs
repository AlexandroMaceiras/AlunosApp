using SQLite.Net.Attributes;

namespace AlunosApp.Classes
{
    public class User
    {
        [PrimaryKey]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string NomeCompleto { get { return string.Format("{0} {1}", this.Nome, this.Sobrenome); } }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string Photo { get; set; }
        public bool Estudante { get; set; }
        public bool Professor { get; set; }
        public string Senha { get; set; }
       
       

        public string PhotoFullPath { get
            {
                if (string.IsNullOrEmpty(this.Photo))
                {
                    return string.Empty;
                }

                return string.Format("http://192.168.15.100:4000{0}", this.Photo.Substring(1));
            }
        }


        public override int GetHashCode()
        {
            return this.UserId;
        }


    }
}
