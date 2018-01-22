using AlunosApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlunosApp.pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class index : ContentPage
	{

        private User user;
		public index (User user)
		{
			InitializeComponent ();
            this.Padding = Device.OnPlatform(
               new Thickness(10, 20, 10, 10),
               new Thickness(10),
               new Thickness(10)
               );

            this.user = user;

            logoutButton.Clicked += LogoutButton_Clicked;
            configurarButton.Clicked += ConfigurarButton_Clicked;
            addNotasButton.Clicked += VerNotasButton_Clicked;
            verNotasButton.Clicked += VerNotasButton_Clicked1;
        }

        private async void VerNotasButton_Clicked1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AlunosMatriculadosPage(this.user));
        }

        private async void VerNotasButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MateriasPage(this.user));
        }

        private async void ConfigurarButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditPage(this.user))
 ;        }

        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            using (var db = new DataAccess())
            {
                var user = db.First<User>();
                if (user != null)
                {
                    db.Delete(user);
                }

                await Navigation.PushAsync(new Login());
               
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            usernameLabel.Text = this.user.NomeCompleto;
            photoImage.Source = this.user.PhotoFullPath;
            photoImage.WidthRequest = 250;
            photoImage.HeightRequest = 250;

            if (this.user.Estudante)
            {
                verNotasButton.IsVisible = true;
            }
            else
            {
                verNotasButton.IsVisible = false;
            }

            if (this.user.Professor)
            {
                addNotasButton.IsVisible = true;
            }
            else
            {
                addNotasButton.IsVisible = false;
            }
        }
    }
}