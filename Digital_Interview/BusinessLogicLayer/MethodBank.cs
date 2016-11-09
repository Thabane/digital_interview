using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataClientLayer;
using DataModelCommon;


namespace BusinessLogicLayer
{
    public class MethodBank
    {
        public List<User> GetUsers()
        {
            DigitalInterviewDbContext context = new DigitalInterviewDbContext();
            return context.Users.ToList();
        }
        public List<Subcription> GetSubcriptions()
        {
            DigitalInterviewDbContext context = new DigitalInterviewDbContext();
            return context.Subcriptions.ToList();
        }
    }
}
