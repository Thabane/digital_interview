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
    }
}
