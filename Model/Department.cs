using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTElecrtroshield.Model
{
    public class Department
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
        public Department()
        {
            
        }
        public Department(string title)
        {
			title = title;
        }

    }
}
