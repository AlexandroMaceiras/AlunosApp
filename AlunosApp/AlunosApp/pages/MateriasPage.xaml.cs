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
	public partial class MateriasPage : ContentPage
	{
        private User user;
		public MateriasPage (User user)
		{
            InitializeComponent();
            this.Padding = Device.OnPlatform(
               new Thickness(10, 20, 10, 10),
               new Thickness(10),
               new Thickness(10)
               );

            materiasListView.ItemTemplate = new DataTemplate(typeof(MateriasCell));
            materiasListView.ItemSelected += MateriasListView_ItemSelected;
            this.user = user;

        }

        private async void MateriasListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new NotaPage((Materias)e.SelectedItem));

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            loadMaterias();
        }

        private async void loadMaterias()
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
            materiasListView.ItemsSource = resposta.MateriasProf;

            waitActivityIndicator.IsRunning = false;
        }
    }
}