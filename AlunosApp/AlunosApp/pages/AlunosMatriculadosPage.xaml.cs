using AlunosApp.cells;
using AlunosApp.Classes;
using Newtonsoft.Json;
using System;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlunosApp.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AlunosMatriculadosPage : ContentPage
	{
        private User user;
		public AlunosMatriculadosPage (User user)
		{
            InitializeComponent();
            this.Padding = Device.OnPlatform(
               new Thickness(10, 20, 10, 10),
               new Thickness(10),
               new Thickness(10)
               );

            materiasListView.ItemTemplate = new DataTemplate(typeof(alunosMatriculadosCell));
            materiasListView.RowHeight = 160;
            this.user = user;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            loadMateriasAlunos();
        }

        private async void loadMateriasAlunos()
        {
            waitActivityIndicator.IsRunning = true;

            var resp = string.Empty;

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.15.100:4000");
                var url = string.Format("/API/Grupos/GetGrupos/{0}", user.UserId);
                var result = await client.GetAsync(url);

                if (!result.IsSuccessStatusCode)
                {
                    waitActivityIndicator.IsRunning = false;
                    await DisplayAlert("Erro", result.StatusCode.ToString(), "Aceitar");
                    return;
                }

                resp = await result.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "Aceitar");
                waitActivityIndicator.IsRunning = false;
                return;

            }

            var resposta = JsonConvert.DeserializeObject<MateriaisResponse>(resp);
            await CalcularNota(resposta.MatriculadoEm);
            materiasListView.ItemsSource = resposta.MatriculadoEm;

            waitActivityIndicator.IsRunning = false;
        }

        private async Task CalcularNota(List<Professor> matriculadoEm)
        {
            foreach(var aluno in matriculadoEm)
            {
                aluno.Nota = await NotaDefinitiva(user.UserId, aluno.GrupoId);

            }
        }

        private async Task<float> NotaDefinitiva(int userId, int grupoId)
        {
            
            var resp = string.Empty;

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.15.100:4000");
                var url = string.Format("/API/Grupos/GetNotas/{0}/{1}", grupoId, userId);
                var result = await client.GetAsync(url);

                if (!result.IsSuccessStatusCode)
                {
                   
                    return 0;
                }

                resp = await result.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                return 0;

            }

            var resposta = JsonConvert.DeserializeObject<NotaResponse>(resp);
            return resposta.Notas;
        }
    }
}