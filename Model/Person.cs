namespace TTElecrtroshield.Model
{
    public class Person
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string fio;

        public string Fio
        {
            get { return fio; }
            set { fio = value; }
        }
        private int tabNumber;

        public int TabNumber
        {
            get { return tabNumber; }
            set { tabNumber = value; }
        }

        private DateTime birthday;

        public DateTime Birthday
        {
            get { return birthday; }
            set { birthday = value; }
        }
        private string gender;

        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        private int id_department;

        public int Id_department
        {
            get { return id_department; }
            set { id_department = value; }
        }
        private int id_education;

        public int Id_educaion
        {
            get { return id_education; }
            set { id_education = value; }
        }
        private DateTime dateHired;

        public DateTime DateHired
        {
            get { return dateHired; }
            set { dateHired = value; }
        }
        private DateTime? dateRetired;

        public DateTime? DateRetired
        {
            get { return dateRetired; }
            set { dateRetired = value; }
        }
        private decimal percentEncrease;

        public decimal PercentEncrease
        {
            get { return percentEncrease; }
            set { percentEncrease = value; }
        }

        public Person()
        {

        }
        public override string ToString()
        {
            return $"{Fio} ,табельный номер: {tabNumber}"; 
        }


    }
}
