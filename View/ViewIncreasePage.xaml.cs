using System.Windows.Controls;
using TTElecrtroshield.Model;
using TTElecrtroshield.Model.Database;

namespace TTElecrtroshield.View
{

    public partial class ViewIncreasePage : Page
    {
        ElectroshieldRepository _repository;
        public ViewIncreasePage()
        {
            _repository = new ElectroshieldRepository();
            InitializeComponent();
            ShowPersonsForIncrease();
        }
        private void ShowPersonsForIncrease()
        {
            var personsForIncrease = _repository.PersonsOnIncrease();
            lbIncreaseList.ItemsSource = personsForIncrease;
        }

        private void btnBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
