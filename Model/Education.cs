namespace TTElecrtroshield.Model
{
    public class Education
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public Education()
        {

        }
        public Education(string title)
        {
            title = title;
        }
    }
}
