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
	public partial class MudarSenhaPage : ContentPage
	{

        private User user;
        public MudarSenhaPage (User user)
		{
			
            InitializeComponent();
            this.Padding = Device.OnPlatform(
               new Thickness(10, 20, 10, 10),
               new Thickness(10),
               new Thickness(10)
               );

            this.user = user;
            saveButton.Clicked += SaveButton_Clicked;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            


            if (string.IsNullOrEmpty(senhaAtualEntry.Text))
            {
                await DisplayAlert("Erro", "Insira uma senha", "Aceitar");
                senhaAtualEntry.Focus();
                return;
            }


            if (string.IsNullOrEmpty(confirmarnovaSenhaEntry.Text))
            {
                await DisplayAlert("Erro", "Insira uma senha", "Aceitar");
                confirmarnovaSenhaEntry.Focus();
                return;
            }

            if (string.IsNullOrEmpty(novaSenhaEntry.Text))
            {
                await DisplayAlert("Erro", "Insira uma senha", "Aceitar");
                novaSenhaEntry.Focus();
                return;
            }

            if (novaSenhaEntry.Text != confirmarnovaSenhaEntry.Text)
            {
                await DisplayAlert("Erro", "As senhas não se coincidem!", "Aceitar");
                confirmarnovaSenhaEntry.Focus();
                return;
            }

            MudarSenha();

        }

        private async void MudarSenha()
        {
            waitActivityIndicator.IsRunning = true;
            saveButton.IsEnabled = false;

            var request = new
            {
                UserName = user.UserName,
                NovaSenha = novaSenhaEntry.Text,
                VelhaSenha = senhaAtualEntry.Text
            };


            var jsonRequest = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.15.100:4000");
                var url = "/API/Usuarios/MudarSenha";
                var result = await client.PutAsync(url, httpContent);

                if (!result.IsSuccessStatusCode)
                {
                    waitActivityIndicator.IsRunning = false;

                    await DisplayAlert("Erro", result.Content.ToString(), "Aceitar");

                    return;
                }


                using (var db = new DataAccess())
                {
                    user.Senha = novaSenhaEntry.Text;
                    db.Update(user);
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "Aceitar");
                waitActivityIndicator.IsRunning = false;
                saveButton.IsEnabled = true;
                return;

            }

            waitActivityIndicator.IsRunning = false;
            saveButton.IsEnabled = true;
            await DisplayAlert("Confirmação", "Senha editada com sucesso!", "ok");
            await Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            photoImage.Source = this.user.PhotoFullPath;
            photoImage.WidthRequest = 250;
            photoImage.HeightRequest = 250;
        }
    }
}