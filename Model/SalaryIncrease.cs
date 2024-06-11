namespace TTElecrtroshield.Model
{
    public class SalaryIncrease
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private int id_person;

        public int Id_person
        {
            get { return id_person; }
            set { id_person = value; }
        }

        private decimal percentIncrease;

        public decimal PercentIncrease
        {
            get { return percentIncrease; }
            set { percentIncrease = value; }
        }
        private Person _person;
        public Person Person {
            get { return _person; }
            set => _person = value;
        }
        public SalaryIncrease() { }

        public override string ToString()
        {
            return $"{Person.Fio} -- {percentIncrease}%"; 
        }

    }
}
