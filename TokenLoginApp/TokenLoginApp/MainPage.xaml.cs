using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TokenLoginApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            GetTolen();
        }

        private async void  GetTolen()
        {
            await ServiceHandler.GetToken("saurabh@gmail.com", "12345").ContinueWith((t) =>
            {
                if (t.IsFaulted)
                {
                    Application.Current.MainPage.DisplayAlert("", "Something Went Wrong", "OK");
                }
                else
                {
                    if (t.Result != null)
                    {
                        var data = t.Result;
                        App.Token = data.access_token;
                        Navigation.PushAsync(new DashboardPage());
                    }
                    else
                    {
                        lblMessage.Text = "Username or password is incorrect.";
                    }

                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
