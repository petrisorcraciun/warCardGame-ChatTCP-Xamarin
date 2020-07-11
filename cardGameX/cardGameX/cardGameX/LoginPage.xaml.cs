using Android.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace cardGameX
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            var current = Connectivity.NetworkAccess;
            var profiles = Connectivity.ConnectionProfiles;
            if (current == NetworkAccess.Internet)
            {
                lblNetworkStatus.Text = "Network is Available";
            }

           
            if (!profiles.Contains(ConnectionProfile.WiFi))
            {
                lblNetworkStatus.Text = profiles.FirstOrDefault().ToString();
            }
            

        }

        private async void Connect_Clicked(object sender, EventArgs e)
        {

            //Application.Current.MainPage = new NavigationPage(new PlayersList());

           
            try
            {
                TcpClient client = new TcpClient();
                await client.ConnectAsync("192.168.100.17", Convert.ToInt32("7777"));

                Connection.Instance.client = client;

                Stream tcpStream2 = client.GetStream();

                ASCIIEncoding asen2 = new ASCIIEncoding();
                byte[] b2a = asen2.GetBytes(namePlayer.Text);
             
                tcpStream2.Write(b2a, 0, b2a.Length);

                if (client.Connected)
                {
                   // Application.Current.MainPage = new NavigationPage(new PlayersList());

                    Navigation.PushAsync(new PlayersList());

                    //await DisplayAlert("Connected", "Canalul a fost creat cu succes!", "Ok");
                }
                else
                {
                    await DisplayAlert("Error", "Eroare! Nu am putut crea canalul.", "Ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "" + ex.ToString(), "Ok");
            }
           
        }

    }
}