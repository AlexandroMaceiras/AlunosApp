using AlunosApp.Classes;
using AlunosApp.pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AlunosApp
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            using (var db = new DataAccess())
            {
                var user = db.First<User>();
                if(user == null)
                {
                    this.MainPage = new NavigationPage(new Login());
                }
                else
                {
                    this. MainPage = new NavigationPage(new index(user));
                }
            }

           
			
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
