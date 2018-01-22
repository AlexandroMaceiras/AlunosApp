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
	public partial class RegisterPage : ContentPage
	{

       
        public RegisterPage ()
		{
			InitializeComponent ();

            this.Padding = Device.OnPlatform(
               new Thickness(10, 20, 10, 10),
               new Thickness(10),
               new Thickness(10)
               );

            saveButton.Clicked += SaveButton_Clicked;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(userNameEntry.Text))
            {
                await DisplayAlert("Erro", "Insira um email", "Aceitar");
                userNameEntry.Focus();
                return;
            }

            if (!Utilities.ValidarEmail(userNameEntry.Text))
            {
                await DisplayAlert("Erro", "Digite um email Valido!", "Aceitar");
                userNameEntry.Focus();
                return;
            }

            if (string.IsNullOrEmpty(nameEntry.Text))
            {
                await DisplayAlert("Erro", "Insira um nome", "Aceitar");
                nameEntry.Focus();
                return;
            }


            if (string.IsNullOrEmpty(sobrenomeEntry.Text))
            {
                await DisplayAlert("Erro", "Insira um Sobrenome", "Aceitar");
                sobrenomeEntry.Focus();
                return;

            }

            if (string.IsNullOrEmpty(senhaEntry.Text))
            {
                await DisplayAlert("Erro", "Insira uma senha", "Aceitar");
                senhaEntry.Focus();
                return;
            }

            if (string.IsNullOrEmpty(confirmarsenhaEntry.Text))
            {
                await DisplayAlert("Erro", "Confirme a senha", "Aceitar");
                confirmarsenhaEntry.Focus();
                return;
                
            }

            if(senhaEntry.Text != confirmarsenhaEntry.Text)
            {
                await DisplayAlert("Erro", "As senhas não se coincidem!", "Aceitar");
                confirmarsenhaEntry.Focus();
                return;
            }

            this.RegistrarEstudante();
        }

        private async void RegistrarEstudante()
        {
            waitActivityIndicator.IsRunning = true;
            saveButton.IsEnabled = false;

            var user = new User
            {
                Endereco = enderecoEntry.Text,
                Professor = false,
                Estudante = true,
                Sobrenome = sobrenomeEntry.Text,
                Telefone = telefoneNameEntry.Text,
                UserName = userNameEntry.Text,
                Nome = nameEntry.Text,
                Senha = senhaEntry.Text,
                
            };

            var jsonRequest = JsonConvert.SerializeObject(user);
            var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var resp = string.Empty;

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.15.100:4000");
                var url = "/API/Usuarios";
                var result = await client.PostAsync(url, httpContent);

                if (!result.IsSuccessStatusCode)
                {
                    waitActivityIndicator.IsRunning = false;
                    
                    await DisplayAlert("Erro", result.Content.ToString(), "Aceitar");
                    
                    return;
                }

                resp = await result.Content.ReadAsStringAsync();

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

            var userResponse = JsonConvert.DeserializeObject<User>(resp);
            await Navigation.PushAsync(new index(userResponse));
        }
    }
}