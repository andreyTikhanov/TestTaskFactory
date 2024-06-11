using Mysqlx.Expect;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using TTElecrtroshield.Model;
using TTElecrtroshield.Model.Database;

namespace TTElecrtroshield.View
{

    public partial class UpdateAndRemovePerson : Page
    {
        ElectroshieldRepository _repository;
        List<Person> _persons;
        List<SalaryIncrease> _salaryIncrease;
        private Person _selectedPerson;
        public UpdateAndRemovePerson()
        {
            _repository = new ElectroshieldRepository();
            InitializeComponent();
            LoadDepartments();
            LoadPersons();
        }
        private void lbEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedPerson = lbEmployees.SelectedItem as Person;
        }
        private void btnDeleteSelectedPersons_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
            "Вы действительно хотите удалить сотрудника?",
            "Подтверждение удаления",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var selectedPersons = lbEmployees.SelectedItems.Cast<Person>().ToList();
                foreach (var person in selectedPersons)
                {
                    _repository.DeletePerson(person.TabNumber);
                }
            }
            LoadPersons();
            MessageBox.Show("Выбранные сотрудники успешно удален");
        }
        private void LoadDepartments()
        {
            var departmentList = _repository.GetDepartments();
            cbDepartments.ItemsSource = departmentList;
            cbDepartments.DisplayMemberPath = "Title";
            cbDepartments.SelectedValuePath = "Id";
        }
        public void LoadPersons()
        {
            _persons = _repository.GetPersons();
            if (_persons == null || _persons.Count == 0)
            {
                return;
            }
            lbEmployees.ItemsSource = _persons;
            lbEmployees.DisplayMemberPath = "";
            lbEmployees.SelectedValuePath = "Id";
        }
        private void cbDepartments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDepartment = cbDepartments.SelectedItem as Department;
            if (selectedDepartment != null)
            {
                ObservableCollection<Person> filteredPersons = new(_persons.Where(p => p.Id_department == selectedDepartment.Id));
                lbEmployees.ItemsSource = filteredPersons;
            }
            else
            {
                lbEmployees.ItemsSource = _persons;
            }
        }
        private void btnAddIncrease_Click(object sender, RoutedEventArgs e)
        {
                if (decimal.TryParse(tbPercent.Text, out decimal percentIncrease))
                {
                    var selectedPersons = lbEmployees.SelectedItems.Cast<Person>().ToList();
                    foreach (var person in selectedPersons)
                    {
                        _repository.AddPersonOnIncrease(person.Id, percentIncrease);
                    }
                    MessageBox.Show("Сотрудники на повышение зарплаты добавлены");
                    tbPercent.Text = "";
                    LoadPersons(); 
                }
                else
                {
                    MessageBox.Show("Введите верное значение для процентов");
                }
            }
        private void btnAddPersonClick(object sender, RoutedEventArgs e)
        {
            var addPersonPage = new AddPersonPage(this);
            NavigationService.Navigate(addPersonPage);
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void btnEditSelectedPerson_Click(object sender, RoutedEventArgs e)
        {
            var selectedPerson = (Person)lbEmployees.SelectedItem;
            if (selectedPerson != null)
            {
                var editPersonPage = new AddPersonPage(this, selectedPerson);
                NavigationService.Navigate(editPersonPage);
            }

        }
        private void btnLookIncrease_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ViewIncreasePage());
        }
        private void btnGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string reportsDirectory = Path.Combine(myDocumentsPath, "Reports");
            if (!Directory.Exists(reportsDirectory))
            {
                Directory.CreateDirectory(reportsDirectory);
            }
            string filePath = Path.Combine(reportsDirectory, "SalaryIncreaseReport.xlsx");
            try
            {
                _repository.GenerateSalaryIncreaseReport(filePath);
                MessageBox.Show("Отчет успешно создан.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании отчета: {ex.Message}");
            }
        }

        private void btnListNotIncrease(object sender, RoutedEventArgs e)
        {
            lbEmployees.ItemsSource = _repository.GetPersonNotIncrease();
        }
    }
}
