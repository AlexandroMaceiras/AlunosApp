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
	public partial class EditPage : ContentPage
	{

        private User user;
		public EditPage (User user)
		{
		
            InitializeComponent();
            this.Padding = Device.OnPlatform(
               new Thickness(10, 20, 10, 10),
               new Thickness(10),
               new Thickness(10)
               );

            this.user = user;
            saveButton.Clicked += SaveButton_Clicked;
            mudarSenhaButton.Clicked += MudarSenhaButton_Clicked;
        }

        private async void MudarSenhaButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MudarSenhaPage(user));
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

           
           
            this.EditarEstudante();
        }

        private async void EditarEstudante()
        {
            waitActivityIndicator.IsRunning = true;
            saveButton.IsEnabled = false;

            this.user.UserName = userNameEntry.Text;
            this.user.Nome = nameEntry.Text;
            this.user.Sobrenome = sobrenomeEntry.Text;
            this.user.Telefone = telefoneNameEntry.Text;
            this.user.Endereco = enderecoEntry.Text;

            var jsonRequest = JsonConvert.SerializeObject(this.user);
            var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var resp = string.Empty;

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.15.100:4000");
                var url = string.Format("/API/Usuarios/{0}", this.user.UserId);
                var result = await client.PutAsync(url, httpContent);

                if (!result.IsSuccessStatusCode)
                {
                    waitActivityIndicator.IsRunning = false;

                    await DisplayAlert("Erro", result.Content.ToString(), "Aceitar");

                    return;
                }

               
                using(var db = new DataAccess())
                {
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
            await DisplayAlert("Confirmação", "Dados editados com sucesso!", "ok");
            await Navigation.PopAsync();
        }
    

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            photoImage.Source = this.user.PhotoFullPath;
            photoImage.WidthRequest = 250;
            photoImage.HeightRequest = 250;

            userNameEntry.Text = this.user.UserName;
            nameEntry.Text = this.user.Nome;
            sobrenomeEntry.Text = this.user.Sobrenome;
            telefoneNameEntry.Text = this.user.Telefone;
            enderecoEntry.Text = this.user.Endereco;
            userNameEntry.Text = this.user.UserName;
        }

        }
}