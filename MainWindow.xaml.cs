using System.Windows;
using System.Windows.Controls;
using TTElecrtroshield.Model;
using TTElecrtroshield.Model.Database;
using TTElecrtroshield.View;

namespace TTElecrtroshield
{

    public partial class MainWindow : Window
    {
        ElectroshieldRepository _repository;
       
        public MainWindow()
        {
            _repository = new ElectroshieldRepository();
            _repository.OpenConnect();
            InitializeComponent();
            MainFrame.NavigationService.Navigate(new UpdateAndRemovePerson());
        }

       
    }
}