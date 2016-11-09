using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelCommon
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserSubcription
    {
        public int ID { get; set; }
        public Subcription SubcriptionID { get; set; }
        public User UserID { get; set; }
    }

    public class Subcription
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Limit { get; set; }
    }
}
