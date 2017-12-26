using GalaSoft.MvvmLight;
using Training1.Classes;

namespace Training1.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        Server server;
        public MainViewModel()
        {
             server = new Server();
        }
    }
}