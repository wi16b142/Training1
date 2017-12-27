using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using Training1.Classes;

namespace Training1.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        RelayCommand Toggle1BtnClickCm { get; set; }
        RelayCommand Toggle2BtnClickCm { get; set; }
        RelayCommand Toggle3BtnClickCm { get; set; }
        RelayCommand Toggle4BtnClickCm { get; set; }
        public ObservableCollection<Entry> History { get; set; }

        public MainViewModel()
        {
             
        }
    }
}