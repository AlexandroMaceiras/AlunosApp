using SQLite.Net.Interop;

namespace AlunosApp.intefaces
{
    public interface IConfig
    {
        string DiretorioDB { get; }
        ISQLitePlatform Plataforma { get; }
    }
}
