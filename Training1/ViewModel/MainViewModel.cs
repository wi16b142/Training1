using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Training1.Classes;

namespace Training1.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region local variable/property definition
        public RelayCommand Toggle1BtnClickCmd { get; set; }
        public SolidColorBrush Toggle1Brush { get; set; }
        public RelayCommand Toggle2BtnClickCmd { get; set; }
        public SolidColorBrush Toggle2Brush { get; set; }
        public RelayCommand Toggle3BtnClickCmd { get; set; }
        public SolidColorBrush Toggle3Brush { get; set; }
        public RelayCommand Toggle4BtnClickCmd { get; set; }
        public SolidColorBrush Toggle4Brush { get; set; }
        public RelayCommand ListenBtnClickCmd { get; set; }
        public RelayCommand ConnectBtnClickCmd { get; set; }
        public RelayCommand CloseBtnClickCmd { get; set; }
        public ObservableCollection<Entry> History { get; set; }
        private Server server;
        private Client client;
        private bool isConnected = false;
        private bool isServer = false;
        private const string ip = "127.0.0.1"; //localhost
        private const int port = 10100;
        #endregion

        public MainViewModel()
        {
            //instantiate connect and listen button relay commands            
            //depending on button clicked, start app as client or server and configure gui accordingly
            //instantiate brushes for the toggle states and the observable for the history entries
            #region startup
            Toggle1Brush = new SolidColorBrush(Colors.Red);
            Toggle2Brush = new SolidColorBrush(Colors.Red);
            Toggle3Brush = new SolidColorBrush(Colors.Red);
            Toggle4Brush = new SolidColorBrush(Colors.Red);
            History = new ObservableCollection<Entry>();

            ListenBtnClickCmd = new RelayCommand( 
                () => 
                {
                    StartUpAsServer();
                }, 
                () => { return !isConnected; } );

            ConnectBtnClickCmd = new RelayCommand(
                () =>
                {
                    StartUpAsClient();
                },
                () => { return !isConnected; });

            CloseBtnClickCmd = new RelayCommand(
                () =>
                {
                    if (isServer)
                    {
                        server.Close();
                        History.Add(new Entry("Server Closed", ""));
                    }
                    else
                    {
                        client.Close();
                        History.Add(new Entry("Client Closed", ""));
                    }
                },
                () => { return isConnected; });
            #endregion


            //instantiate toggle buttons, only visible for servers"
            #region toggle relaycommands
            Toggle1BtnClickCmd = new RelayCommand(
                () =>
                {
                    Toggle("1");
                },
                () => { return isServer; });

            Toggle2BtnClickCmd = new RelayCommand(
                () =>
                {
                    Toggle("2");
                },
                () => { return isServer; });

            Toggle3BtnClickCmd = new RelayCommand(
                () =>
                {
                    Toggle("3");
                },
                () => { return isServer; });

            Toggle4BtnClickCmd = new RelayCommand(
                () =>
                {
                    Toggle("4");
                },
                () => { return isServer; });
            #endregion


        }

        private void StartUpAsServer()
        {
            isConnected = true;
            isServer = true;
            server = new Server(ip, port, UpdateGUI);

        }

        private void StartUpAsClient()
        {
            isConnected = true;
            client = new Client(ip, port, UpdateGUI);

        }

        private void Toggle(string id)
        {
            string updateID = "";
            string updateBrush = "";

            switch (id)
            {
                case "1":
                    updateID = "Button1";
                    updateBrush = InvertBrush(Toggle1Brush);
                    break;
                case "2":
                    updateID = "Button2";
                    updateBrush = InvertBrush(Toggle2Brush);
                    break;
                case "3":
                    updateID = "Button3";
                    updateBrush = InvertBrush(Toggle3Brush);
                    break;
                case "4":
                    updateID = "Button4";
                    updateBrush = InvertBrush(Toggle4Brush);
                    break;
            }
            
            History.Add(new Entry(updateID, updateBrush));
        }

        private string InvertBrush(SolidColorBrush brush)
        {
            string brushToReadable = "red";

            if (brush.Color == Colors.Red)
            {
                brush.Color = Colors.Green;
                brushToReadable = "green";
            }
            else brush.Color = Colors.Red;

            return brushToReadable;
        }

        private void UpdateGUI(string update)
        {
            string button = update.Split(':')[0];
            string state = update.Split(':')[1];
            History.Add(new Entry(button, state));
        }
    }
}