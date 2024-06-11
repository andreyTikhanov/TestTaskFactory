using Mysqlx.Session;
using System.Windows;
using System.Windows.Controls;
using TTElecrtroshield.Model;
using TTElecrtroshield.Model.Database;

namespace TTElecrtroshield.View
{
    public partial class AddPersonPage : Page
    {
        private UpdateAndRemovePerson _parentPage;
        ElectroshieldRepository _repository;
        Person person;
        List<Person> _persons;

        public AddPersonPage(UpdateAndRemovePerson parentPage)
        {
            InitializeComponent();
            _repository = new ElectroshieldRepository();
            LoadEducationData();
            LoadDepartments();
            person = new Person();
            _parentPage = parentPage;
        }
        public AddPersonPage(UpdateAndRemovePerson parentPage, Person p)
        {
            InitializeComponent();
            _repository = new ElectroshieldRepository();
            this.person = p;
            LoadEducationData();
            LoadDepartments();
            LoadData(person);
            _parentPage = parentPage;
        }

        private void LoadData(Person person)
        {
            if (person == null) return;
            tbNamePerson.Text = person.Fio.ToString();
            tbTabNumber.Text = person.TabNumber.ToString();
            dpBirthday.Text = person.Birthday.ToString();
            cbGender.SelectedItem = person.Gender.ToString();
            departmentComboBox.SelectedItem = person.Id_department.ToString(); ;
            educationComboBox.SelectedItem = person.Id_educaion.ToString();
            dpDateHired.Text = person.DateHired.ToString();
            lbDateRetired.Visibility = Visibility.Visible;
            dpDateRetired.Visibility = Visibility.Visible;

        }
        private void LoadEducationData()
        {
            var educationList = _repository.GetListEducation();
            educationComboBox.ItemsSource = educationList;
            educationComboBox.DisplayMemberPath = "Title";
            educationComboBox.SelectedValuePath = "Id";
        }
        private void LoadDepartments()
        {
            var departmentList = _repository.GetDepartments();
            departmentComboBox.ItemsSource = departmentList;
            departmentComboBox.DisplayMemberPath = "Title";
            departmentComboBox.SelectedValuePath = "Id";
        }
        private void GenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbGender.SelectedItem != null)
            {
                var selectedGender = (cbGender.SelectedItem as ComboBoxItem).Content.ToString();
                person.Gender = selectedGender;
            }
        }
        private void btnSavePerson_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbNamePerson.Text) ||
                string.IsNullOrWhiteSpace(tbTabNumber.Text) ||
                dpBirthday.SelectedDate == null ||
                dpDateHired.SelectedDate == null)
            {
                MessageBox.Show("Все поля должны быть заполнены.");
                return;
            }

            if (person == null)
            {
                person = new Person();
            }
            else
            {
                var checkPeson = _repository.GetPersons()
                    .FirstOrDefault(p => p.TabNumber == int.Parse(tbTabNumber.Text) && p.Id != person.Id);
                if (checkPeson != null)
                {
                    MessageBox.Show("Табельный номер занят");
                    return;
                }
            }
            person.TabNumber = int.Parse(tbTabNumber.Text);
            person.Fio = tbNamePerson.Text;
            person.Birthday = dpBirthday.SelectedDate.Value;
            person.DateHired = dpDateHired.SelectedDate.Value;

            if (cbGender.SelectedItem != null)
            {
                person.Gender = (cbGender.SelectedItem as ComboBoxItem).Content.ToString();
            }
            if (educationComboBox.SelectedItem != null)
            {
                person.Id_educaion = (int)educationComboBox.SelectedValue;
            }
            if (departmentComboBox.SelectedItem != null)
            {
                person.Id_department = (int)departmentComboBox.SelectedValue;
            }
            if (person.Id == 0)
            {
                _repository.AddPerson(person);
                MessageBox.Show("Сотрудник успешно добавлен.");
                ClearFields();
                _parentPage.LoadPersons();
            }
            else
            {
                _repository.UpdatePerson(person);
                MessageBox.Show("Сотрудник обновлен");
                NavigationService.GoBack();
                _parentPage.LoadPersons();
            }
        }
        private void ClearFields()
        {
            tbNamePerson.Text = "";
            tbTabNumber.Text = "";
            dpBirthday.Text = null;
            cbGender.Text = null;
            departmentComboBox.Text = null;
            educationComboBox.Text = null;
            dpDateHired.Text = null;
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.GoBack();
        }

        
    }

}

