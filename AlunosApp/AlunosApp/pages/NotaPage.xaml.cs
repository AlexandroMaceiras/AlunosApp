using AlunosApp.cells;
using AlunosApp.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlunosApp.pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotaPage : ContentPage
	{
        private Materias materias;
        private List<estudantesResponse> estResponse;
        float participacao;
		public NotaPage (Materias materias)
		{
            InitializeComponent();
            this.Padding = Device.OnPlatform(
               new Thickness(10, 20, 10, 10),
               new Thickness(10),
               new Thickness(10)
               );

            this.materias = materias;
            notasListView.ItemTemplate = new DataTemplate(typeof(notaCell));
            notasListView.RowHeight = 110;

            saveButton.Clicked += SaveButton_Clicked;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(participacaoEntry.Text))
            {
                await DisplayAlert("Erro", "Insira uma nota de Participação", "Aceitar");
                participacaoEntry.Focus();
                return;
            }

            participacao = float.Parse(participacaoEntry.Text);

            if(participacao < 0 || participacao > 1)
            {
                await DisplayAlert("Erro", "A nota de participação só pode ser de 1 a 3", "Aceitar");
                participacaoEntry.Focus();
                return;
            }

            foreach(var estudante in estResponse)
            {
                if(estudante.Nota < 0 || estudante.Nota > 5)
                {
                    await DisplayAlert("Erro", string.Format("O Aluno {0} tem uma nota inválida, insira uma nota de 1 a 10", estudante.Estudante.NomeCompleto), "Aceitar");
                    participacaoEntry.Focus();
                    return;
                }


                

            }

            SalvarNotas();
        }

        private async void SalvarNotas()
        {
            waitActivityIndicator.IsRunning = true;

            var body = new
            {
                Percentual = participacao,
                Estudante = estResponse,
            };
            var jsonRequest = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var resp = string.Empty;


            try
            {

                var client = new HttpClient();
            client.BaseAddress = new Uri("http://192.168.15.100:4000");
            var url = "/API/Grupos/SalvarNotas";
            var result = await client.PostAsync(url, httpContent);

            if (!result.IsSuccessStatusCode)
            {
                await DisplayAlert("Erro", result.StatusCode.ToString(), "Aceitar");
                waitActivityIndicator.IsRunning = false;
                return;
            }


            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "Aceitar");
                waitActivityIndicator.IsRunning = false;
                return;

            }
            await DisplayAlert("Salvo", "Nota Salva com sucesso!!", "Aceitar");
            await Navigation.PopAsync();
            waitActivityIndicator.IsRunning = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            loadEstudantes();
        }

        private async void loadEstudantes()
        {
            waitActivityIndicator.IsRunning = true;

            var resp = string.Empty;

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.15.100:4000");
                var url = string.Format("/API/Grupos/GetEstudantes/{0}", materias.GrupoId);
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

            estResponse = JsonConvert.DeserializeObject<List<estudantesResponse>>(resp);
            notasListView.ItemsSource = estResponse;

            waitActivityIndicator.IsRunning = false;
        }
    }
}