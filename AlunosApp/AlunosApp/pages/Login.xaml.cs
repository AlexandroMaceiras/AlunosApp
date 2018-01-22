using AlunosApp.Classes;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlunosApp.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
		public Login ()
		{
			InitializeComponent ();
            this.Padding = Device.OnPlatform(
                new Thickness(10, 20, 10, 10),
                new Thickness(10),
                new Thickness(10)
                );

            loginButton.Clicked += LoginButton_Clicked;
            registerButton.Clicked += RegisterButton_Clicked;
		}

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }

        protected override void OnAppearing()
        {
            waitActivityIndicator.IsRunning = true;
            waitActivityIndicator.IsRunning = false;
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(emailEntry.Text))
            {
                await DisplayAlert("Erro", "Digite um email!", "Aceitar");
                emailEntry.Focus();
                return;
            }


            if (!Utilities.ValidarEmail(emailEntry.Text))
            {
                await DisplayAlert("Erro", "Digite um email Valido!", "Aceitar");
                emailEntry.Focus();
                return;
            }

            if (string.IsNullOrEmpty(passwordEntry.Text))
            {
                await DisplayAlert("Erro", "Digite uma senha", "Aceitar");
                emailEntry.Focus();
                return;
            }

            this.Logar();
        }

        private async void Logar()
        {
            waitActivityIndicator.IsRunning = true;

            var loginRequest = new LoginRequest
            {
                Email = emailEntry.Text,
                Senha = passwordEntry.Text,
            };
            var jsonRequest = JsonConvert.SerializeObject(loginRequest);
            var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var resp = string.Empty;

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.15.100:4000");
                var url = "/API/Usuarios/Login";
                var result = await client.PostAsync(url, httpContent);

                if (!result.IsSuccessStatusCode)
                {
                    await DisplayAlert("Erro", "Usuário ou Senha Incorretos", "Aceitar");
                    waitActivityIndicator.IsRunning = false;
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

            var user = JsonConvert.DeserializeObject<User>(resp);
            user.Senha = passwordEntry.Text;
            this.VerificarRemember(user);
            waitActivityIndicator.IsRunning = false;
            await Navigation.PushAsync(new index(user));


        }

        private void VerificarRemember(User user)
        {
            if (lembrarmeSwitch.IsToggled)
            {
                using (var db = new DataAccess())
                {
                    db.Inserir<User>(user);
                }
            }
        }
    }
}