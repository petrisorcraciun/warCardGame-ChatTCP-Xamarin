using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;

namespace cardGameX
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayersList : ContentPage
    {

        ObservableCollection<String> itemList;
        private static TcpClient client =  new TcpClient();

        


        public PlayersList()
        {
            InitializeComponent();

            itemList = new ObservableCollection<string>();
            // itemList.Add("Petrisor");
            listView.SelectedItem = null;

            listView.ItemsSource = itemList;


            var timer = new Timer(receiveMessage, null, 0, 5 * 100);



        }

        private void startGame_Clicked(object sender, EventArgs e)
        {

            String textToTransmit = "startGame";
            var client = Connection.Instance.client;

            Stream tcpStream = client.GetStream();

            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(textToTransmit);
            tcpStream.Write(ba, 0, ba.Length);



            /*
           
            var client = Connection.Instance.client;
            NetworkStream stream = client.GetStream();
            String s = "Player1";
            byte[] message = Encoding.ASCII.GetBytes(s);
            stream.Write(message, 0, message.Length);


            
            
            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            int bytesRead = stream.Read(bytesToRead, 0, client.ReceiveBufferSize);
            startGame.Text = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);

            itemList.Add(Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
            
            */

        }


        private void receiveMessage(object o)
        {
            var client = Connection.Instance.client;
            NetworkStream stream = client.GetStream();

            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            int bytesRead = stream.Read(bytesToRead, 0, client.ReceiveBufferSize);
            //startGame.Text = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
            if(bytesRead > 0)
            {
                if(Encoding.ASCII.GetString(bytesToRead, 0, bytesRead).Equals("startGame"))
                {

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Application.Current.MainPage = new NavigationPage(new MultiPlayer());
                    });

                    
                    //Navigation.PushAsync(new MultiPlayer());
                }
                else if (Encoding.ASCII.GetString(bytesToRead, 0, bytesRead).Equals("stopGame"))
                {
                    // Navigation.PushAsync(new MainPage());
                    //client.Close();
                    //Application.Current.MainPage = new NavigationPage(new MainPage());

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //client.Close();
                        Navigation.PushAsync(new MainPage());
                        //Application.Current.MainPage = new NavigationPage(new MainPage());
                    });
                }
                else
                {
                    itemList.Add(Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                    //listView.ScrollTo(Encoding.ASCII.GetString(bytesToRead, 0, bytesRead), ScrollToPosition.End, true);

                    var lastItem = listView.ItemsSource.OfType<object>().Last();
                    listView.ScrollTo(lastItem, ScrollToPosition.MakeVisible, true);

                }
            }
        }

        private void closeGame_Clicked(object sender, EventArgs e)
        {
            String textToTransmit = "stopGame";
            var client = Connection.Instance.client;

            Stream tcpStream = client.GetStream();

            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(textToTransmit);
            //Console.WriteLine("Transmitting.....");

            tcpStream.Write(ba, 0, ba.Length);

           
        }

        private void newMessage_Clicked(object sender, EventArgs e)
        {

            String textToTransmit = messagePlayer.Text;

            if(textToTransmit.Length > 4)
            {
                var client = Connection.Instance.client;

                Stream tcpStream = client.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(textToTransmit);
                //Console.WriteLine("Transmitting.....");

                tcpStream.Write(ba, 0, ba.Length);

                messagePlayer.Text = "";
            }
            
        }

        private void emojiList_SelectedIndexChanged(object sender, EventArgs e)
        {
            messagePlayer.Text += " " + emojiList.SelectedItem;
        }
    }
}