using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows;
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
        public ObservableCollection<SolidColorBrush> Brush { get; set; }
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
            History = new ObservableCollection<Entry>();
            Brush = new ObservableCollection<SolidColorBrush>();
            Toggle1Brush = new SolidColorBrush(Colors.Red);
            Toggle2Brush = new SolidColorBrush(Colors.Red);
            Toggle3Brush = new SolidColorBrush(Colors.Red);
            Toggle4Brush = new SolidColorBrush(Colors.Red);
            Brush.Add(Toggle1Brush);
            Brush.Add(Toggle2Brush);
            Brush.Add(Toggle3Brush);
            Brush.Add(Toggle4Brush);

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
                    isConnected = false;
                    if (isServer)
                    {
                        isServer = false;
                        server.Close();
                    }
                    else
                    {
                        client.Close();
                    }
                    CloseWindow();
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
            client = new Client(ip, port, UpdateGUI, AbortInfo);
            isConnected = true;
        }

        private void AbortInfo()
        {
            CloseWindow();
        }

        private void CloseWindow()
        {
            Application.Current.Shutdown();
        }

        private void Toggle(string id)
        {
            string updateID = "";
            string updateBrush = "";

            switch (id)
            {
                case "1":
                    updateID = "1";
                    updateBrush = InvertBrush(Toggle1Brush);
                    break;
                case "2":
                    updateID = "2";
                    updateBrush = InvertBrush(Toggle2Brush);
                    break;
                case "3":
                    updateID = "3";
                    updateBrush = InvertBrush(Toggle3Brush);
                    break;
                case "4":
                    updateID = "4";
                    updateBrush = InvertBrush(Toggle4Brush);
                    break;
            }
            server.BroadcastToggle(updateID, updateBrush);
        }

        private string InvertBrush(SolidColorBrush brush)
        {
            string brushToReadable = "Red";

            if (brush.Color == Colors.Red) brushToReadable = "Green";
            return brushToReadable;
        }

        private void UpdateGUI(string update)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (update.Contains(":"))
                {
                    string ID = update.Split(':')[0];
                    int buttonID = Int32.Parse(ID)-1;
                    string button = "Button" + ID;
                    string state = update.Split(':')[1];
                    System.Drawing.Color tempColor = System.Drawing.Color.FromName(state);
                    Color stateColor = Color.FromArgb(tempColor.A, tempColor.R, tempColor.G, tempColor.B);
                    Brush[buttonID].Color = stateColor;
                    History.Add(new Entry(button, state));
                }
                
            });

        }
    }
}