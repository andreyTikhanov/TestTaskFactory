using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Windows;

namespace TTElecrtroshield.Model.Database
{

    public class ElectroshieldRepository
    {
        List< Department> Departments;
        List<Education> Educations;
        private string fileConnection = "connectionFile.txt";
        private static string readConnection(string file)
        {
            if (!File.Exists(file))
            {
                throw new Exception("Connection file not found");
            }
            string connString = File.ReadAllText(file);
            if (connString == null)
            {
                throw new Exception("Connection string is empty");
            }
            return connString;
        }
        private MySqlConnection _connection;
        public ElectroshieldRepository()
        {
            _connection = new MySqlConnection(readConnection(fileConnection));
            
        }
        public bool OpenConnect()
        {
            try
            {
                _connection.Open();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool CloseConnect()
        {
            try
            {
                _connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void AddPerson(Person person)
        {
            if (!OpenConnect()) return;
            try
            {
                string query = "INSERT INTO persons (tabNumber, fio, datebirthday, gender, department_id, " +
                               "education_id, dateHired, dateRetired) " +
                               "VALUES (@tn, @fio, @bir, @gen, @di, @ei, @dh, @dr)";
                MySqlCommand cmd = _connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@tn", person.TabNumber);
                cmd.Parameters.AddWithValue("@fio", person.Fio);
                cmd.Parameters.AddWithValue("@bir", person.Birthday);
                cmd.Parameters.AddWithValue("@gen", person.Gender);
                cmd.Parameters.AddWithValue("@di", person.Id_department);
                cmd.Parameters.AddWithValue("@ei", person.Id_educaion);
                cmd.Parameters.AddWithValue("@dh", person.DateHired);
                cmd.Parameters.AddWithValue("@dr", person.DateRetired.HasValue ? (object)person.DateRetired.Value : DBNull.Value);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении сотрудника");
            }
            finally
            {
                CloseConnect();
            }
        }
        public void DeletePerson(int tabNumber)
        {
            try
            {
                if (tabNumber == 0) return;
                if (!OpenConnect()) return;

                string query = "DELETE FROM persons WHERE tabNumber = @tb";
                MySqlCommand cmd = _connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@tb", tabNumber);
                cmd.ExecuteNonQuery();
                CloseConnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении сотрудника: {ex.Message}");
            }
        }
        public Person GetPersonById(int id)
        {
            Person person = null;
            MySqlConnection newConnection = new(readConnection(fileConnection));
            newConnection.Open();
            string query = "SELECT * FROM persons WHERE id = @id";
            MySqlCommand cmd = newConnection.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", id);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    person = new Person()
                    {
                        Id = reader.GetInt32(0),
                        TabNumber = reader.GetInt32(1),
                        Fio = reader.GetString(2),
                        Birthday = reader.GetDateTime(3),
                        Gender = reader.GetString(4),
                        Id_department = reader.GetInt32(5),
                        Id_educaion = reader.GetInt32(6),
                        DateHired = reader.GetDateTime(7),
                    };
                    if (reader.IsDBNull(8) == null) person.DateRetired = reader.GetDateTime(8);
                }
                reader.Close();
                CloseConnect();

            }
            return person;
        }
        public List<Education> GetListEducation()
        {
            try
            {
                if (!OpenConnect()) return null;
                List<Education> educations = new List<Education>();
                string query = "select * from educations";
                MySqlCommand cmd = _connection.CreateCommand();
                cmd.CommandText = query;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Education education = new Education
                    {
                        Id = reader.GetInt32("id"),
                        Title = reader.GetString("title"),
                    };
                    educations.Add(education);
                }
                reader.Close();
                CloseConnect();
                return educations;
            }
            catch { return null; }
        }
        public List<Department> GetDepartments()
        {
            try
            {
                if (!OpenConnect()) return null;
                List<Department> departments = new List<Department>();
                string query = "select * from departments";
                MySqlCommand cmd = _connection.CreateCommand();
                cmd.CommandText = query;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Department department = new Department
                    {
                        Id = reader.GetInt32("id"),
                        Title = reader.GetString("title"),
                    };
                    departments.Add(department);

                }
                reader.Close();
                CloseConnect();
                return departments;
            }
            catch { return null; }
        }
        public List<Person> GetPersons()
        {
            List<Person> persons = new List<Person>();
            if (!OpenConnect()) return null;

            string query = "SELECT * FROM persons";
            MySqlCommand cmd = new MySqlCommand(query, _connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Person person = new Person
                {
                    Id = reader.GetInt32(0),
                    TabNumber = reader.GetInt32(1),
                    Fio = reader.GetString(2),
                    Birthday = reader.GetDateTime(3),
                    Gender = reader.GetString(4),
                    Id_department = reader.GetInt32(5),
                    Id_educaion = reader.GetInt32(6),
                    DateHired = reader.GetDateTime(7),

                };
                if (reader.IsDBNull(8) == null) person.DateRetired = reader.GetDateTime(8);
                persons.Add(person);
            }
            reader.Close();
            CloseConnect(); return persons;
        }
        public void AddPersonOnIncrease(int id, decimal percentIncrease)
        {
            if (!OpenConnect()) return;
            string query = "INSERT INTO salaryincrease (employee_id, percentIncrease) VALUES (@ei, @per)";
            MySqlCommand cmd = new MySqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@ei", id);
            cmd.Parameters.AddWithValue("@per", percentIncrease);
            cmd.ExecuteNonQuery();
            CloseConnect();
        }
        public List<SalaryIncrease> PersonsOnIncrease()
        {
            List<SalaryIncrease> personsForIncrease = new();
            MySqlConnection conn = new(readConnection(fileConnection));
            conn.Open();
            string query = @"SELECT * FROM salaryincrease";
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var person = GetPersonById(reader.GetInt32(1));
                if (person != null)
                {
                    

                    personsForIncrease.Add(new SalaryIncrease()
                    {
                        Id = reader.GetInt32(0),
                        Person = person,
                        PercentIncrease = reader.GetDecimal(2)
                    });

                    // Дополнительно устанавливаем PercentEncrease для person
                    person.PercentEncrease = reader.GetDecimal(2);
                }
            }
            reader.Close();
            conn.Close();
            return personsForIncrease;
        }
        public void UpdatePerson(Person person)
        {
            if (person == null || !OpenConnect()) return;
            string query = @"UPDATE persons SET 
                           TabNumber = @TabNumber,
                           Fio = @Fio,
                           dateBirthday = @Birthday,
                           Gender = @Gender,
                           department_id = @Id_department,
                           education_id = @Id_educaion,
                           DateHired = @DateHired,
                           DateRetired = @DateRetired
                           WHERE Id = @Id";

            MySqlCommand cmd = new MySqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@Id", person.Id);
            cmd.Parameters.AddWithValue("@TabNumber", person.TabNumber);
            cmd.Parameters.AddWithValue("@Fio", person.Fio);
            cmd.Parameters.AddWithValue("@Birthday", person.Birthday);
            cmd.Parameters.AddWithValue("@Gender", person.Gender);
            cmd.Parameters.AddWithValue("@Id_department", person.Id_department);
            cmd.Parameters.AddWithValue("@Id_educaion", person.Id_educaion);
            cmd.Parameters.AddWithValue("@DateHired", person.DateHired);
            if (person.DateRetired.HasValue)
            {
                cmd.Parameters.AddWithValue("@DateRetired", person.DateRetired.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@DateRetired", DBNull.Value);
            }
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении данных сотрудника: " + ex.Message);
            }
            finally
            {
                CloseConnect();
            }
        }
        public List<Person> GetPersonsForSalaryIncrease()
        {
            var personsForIncrease = new List<Person>();
            if (!OpenConnect()) return personsForIncrease;

            string query = @"SELECT p.fio, s.percentIncrease 
                         FROM persons p 
                         LEFT JOIN salaryincrease s ON p.id = s.employee_id";
            MySqlCommand cmd = new MySqlCommand(query, _connection);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var person = new Person
                    {
                        Fio = reader.GetString("fio"),
                        PercentEncrease = reader.IsDBNull("percentIncrease") ? 0 : reader.GetDecimal("percentIncrease")
                    };
                    personsForIncrease.Add(person);
                }
            }
            CloseConnect();
            return personsForIncrease;
        }
        public void GenerateSalaryIncreaseReport(string filePath)
        {
            var personsForIncrease = PersonsOnIncrease();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Salary Increases");
                worksheet.Cell(1, 1).Value = "ФИО";
                worksheet.Cell(1, 2).Value = "Процент повышения";
                worksheet.Cell(1, 3).Value = "Табельный номер";
                worksheet.Cell(1, 4).Value = "Дата приема на работу";

                int currentRow = 2;
                foreach (var person in personsForIncrease)
                {
                    worksheet.Cell(currentRow, 1).Value = person.Person.Fio;
                    worksheet.Cell(currentRow, 2).Value = (decimal)person.Person.PercentEncrease;
                    worksheet.Cell(currentRow, 3).Value = person.Person.TabNumber;
                    worksheet.Cell(currentRow, 4).Value = person.Person.DateHired != null ? person.Person.DateHired.ToShortDateString() : "";

                    currentRow++;
                }
                try
                {
                    workbook.SaveAs(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}");
                }
            }
        }
        public List<Person> GetPersonNotIncrease()
        {
            if (!OpenConnect()) return null;
            var personNotIncrease = new List<Person>();
            string query = "select fio from persons p " +
                            "left join salaryincrease s " +
                            "on p.id = s.employee_id " +
                            "where s.employee_id is null;";
            MySqlCommand cmd= _connection.CreateCommand();
            cmd.CommandText = query;
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var person = new Person()
                    {
                        Fio = reader.GetString("fio"),

                    };
                    personNotIncrease.Add(person);
                }
                reader.Close();
                CloseConnect();
            }
            return personNotIncrease;
        }
    }



}



