using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModelCommon;

namespace BusinessLogicLayer
{
    public class DataBank
    {
        public List<Resource> GetResourceById(int subId)
        {
            using (Context db = new Context())
            {
                return db.resource.Where(x => x.subcription.ID == subId).ToList(); 
            }
        }

        public void CreateResource(Resource finalRes)
        {
            using (Context db = new Context())
            {
                db.resource.Add(finalRes);
                db.SaveChanges(); 
            }
        }

        public Subcription GetSubcription(int? subcriptionId)
        {
            using (Context db = new Context())
            {
                return db.subcriptions.Find(subcriptionId); 
            }
        }

        public Resource GetResource(int? id)
        {
            using (Context db = new Context())
            {
                return db.resource.Find(id); 
            }
        }

        public void EditResource(Resource resource)
        {
            using (Context db = new Context())
            {
                db.Entry(resource).State = EntityState.Modified;
                db.SaveChanges(); 
            }
        }

        public bool AddCredit(int UserId,int Fee)
        {
            using (Context context = new Context())
            {
                try
                {
                    Credit credit = new Credit();
                    credit.Date = DateTime.Now;
                    credit.CreditUsed = Fee;
                    credit.user = context.users.Find(UserId);
                    context.credit.Add(credit);
                    context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public void CreateSubscription(Subcription subcription)
        {
            using (Context db = new Context())
            {
                db.subcriptions.Add(subcription);
                db.SaveChanges(); 
            }
        }

        public void EditSubcription(Subcription subcription)
        {
            using (Context db = new Context())
            {
                db.Entry(subcription).State = EntityState.Modified;
                db.SaveChanges(); 
            }
        }

        public List<ToDoList> GetToDoList()
        {
            using (Context context = new Context())
            {
                return context.toDoList.ToList().OrderBy(x => x.Prority).ToList(); 
            }
        } 

        public ToDoList GetToDolistItem(int? id)
        {
            using (Context context = new Context())
            {
                return context.toDoList.Find(id); 
            }
        }

        public bool CreateToDoListItem(ToDoList toDoList)
        {
            using (Context db = new Context())
            {
                try
                {
                    db.toDoList.Add(toDoList);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                } 
                
            }
        }

        public void EditToDo(ToDoList toDoList)
        {
            using (Context db = new Context())
            {
                db.Entry(toDoList).State = EntityState.Modified;
                db.SaveChanges(); 
            }
        }

        public void RemoveToDo(ToDoList toDoList)
        {
            using (Context db = new Context())
            {
                db.toDoList.Remove(toDoList);
                db.SaveChanges(); 
            }
        }

        public UserSubcription GetUserSubcription(int? id)
        {
            using (Context db = new Context())
            {
                return db.userSubcriptions.Find(id); 
            }
        }

        public void EditUserSubcription(UserSubcription userSubcription)
        {
            using (Context db = new Context())
            {
                db.Entry(userSubcription).State = EntityState.Modified;
                db.SaveChanges(); 
            }
        }

        public bool AddVoucher(Voucher voucher)
        {
            using (Context db = new Context())
            {
                try
                {
                    db.couchers.Add(voucher);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public User GetUser(int id)
        {
            using (Context context = new Context())
            {
                return context.users.Find(id);
            }
        }
        
    }
}
