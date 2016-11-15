using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataClientLayer;
using DataModelCommon;


namespace BusinessLogicLayer
{
    public class MethodBank
    {
        public List<User> GetUsers()
        {
            Context context = new Context();
            return context.users.ToList();
        }
        public List<UserInfo> GetUserWithDailyLimit()
        {
            using (Context context = new Context())
            {
                try
                {
                    var userInfo = from userSub in context.userSubcriptions
                                   join user in context.users on userSub.UserID.ID equals user.ID
                                   join x in context.subcriptions on userSub.SubcriptionID.ID equals x.ID
                                   select new UserInfo() { ID = user.ID, FirstName = user.FirstName, LastName = user.LastName, DailyLimit = userSub.DailyLimit,Email = user.Email,Subscription = x.Name,SubscriptionID = x.ID};
                    return userInfo.ToList();
                }
                catch
                {
                    // ignored
                }
            }
            return null;
        }
        public List<SubcriptionInfo> GetSubscriptionInfo()
        {
            using (Context context = new Context())
            {
                try
                {
                    var subscriptionInfo = from sub in context.subcriptions
                        join userSub in context.userSubcriptions on sub.ID equals userSub.SubcriptionID.ID
                        select
                            new SubcriptionInfo()
                            {
                                ID = sub.ID,
                                Name = sub.Name,
                                Description = sub.Description,
                                CreditLimit = sub.CreditLimit,
                                NumberOfUsers = GetSubscriptionUsers(sub.ID)
                            };                                        
                    return subscriptionInfo.ToList();
                }
                catch
                {
                    // ignored
                }
            }
            return null;
        }
        public List<Subcription> GetSubcriptions()
        {
            DigitalInterviewDbContext context = new DigitalInterviewDbContext();
            return context.subcriptions.ToList();
        }
        public void RegisterUser(User user)
        {
            using (Context dbContext = new Context())
            {
                dbContext.users.Add(user);
                dbContext.SaveChanges();
            }
        }
        public User Login(User user)
        {
            using (Context dbContext = new Context())
            {
                try
                {
                    var getUser = dbContext.users.Single(x => x.Email == user.Email && x.Password == user.Password);
                    if (getUser != null)
                    {
                        return getUser;
                    }                    
                }
                catch
                {
                    // ignored
                }
            }
            return null;
        }
        public int GetNumberOfSubcriptions()
        {
            using (Context context = new Context())
            {
                return context.subcriptions.Count();
            }
        }
        public int GetNumberOfUsers()
        {
            using (Context context = new Context())
            {
                return context.users.Count();
            }
        }
        public int GetNumberOfResources()
        {
            using (Context context = new Context())
            {
                return context.resource.Count();
            }
        }
        public int GetNumberOfVouchers()
        {
            using (Context context = new Context())
            {
                return context.couchers.Count();
            }
        }
        public int GetSubscriptionUsers(int subscriptionId)
        {
            using (Context context = new Context())
            {
                try
                {
                    var subscriptionInfo = from sub in context.subcriptions
                                           join userSub in context.userSubcriptions on sub.ID equals userSub.SubcriptionID.ID
                                           where sub.ID == subscriptionId
                                           select
                                               new SubcriptionInfo()
                                               {
                                                   ID = sub.ID,
                                                   Name = sub.Name,
                                                   Description = sub.Description,
                                                   CreditLimit = sub.CreditLimit
                                               };
                    return subscriptionInfo.Count();
                }
                catch
                {
                    // ignored
                }                
            }
            return 0;
        }
        public bool AddUserToSubcription(int userId, int subcriptionId)
        {
            Context context = new Context();
            
            try
            {
                UserSubcription userSubcription = new UserSubcription();
                User user = context.users.Find(userId);
                Subcription subcription = context.subcriptions.Find(subcriptionId);
                userSubcription.UserID = user;
                userSubcription.SubcriptionID = subcription;
                context.userSubcriptions.Add(userSubcription);
                context.SaveChanges();
                return true;
            }
            catch
            {
                // ignored
            }
            return false;
        } 
        public Dashboard GetDashboard(int userId)
        {
            Context context = new Context();
            MethodBank mb = new MethodBank();
            Dashboard dash = new Dashboard();
            dash.Resources = mb.GetNumberOfResources();
            dash.Subscriptions = mb.GetNumberOfSubcriptions();
            dash.Users = mb.GetNumberOfUsers();
            dash.Vouchers = mb.GetNumberOfVouchers();
            dash.FirstName = context.users.Where(x => x.ID == userId).FirstOrDefault().FirstName;
            dash.LastName = context.users.Where(x => x.ID == userId).FirstOrDefault().LastName;
            return dash;
        }

        public bool UseResource(int UserId,int SubcriptionId,int ResourceId)
        {
            using (Context context = new Context())
            {
                bool hasValidVoucher;
                try
                {
                    var userVoucher = context.couchers.Where(x => x.user.ID == UserId && x.Used == false && x.ExpiryDate >= DateTime.Now);
                    var firstOrDefault = userVoucher.FirstOrDefault();
                    if (firstOrDefault != null)
                    {
                        MethodBank mb = new MethodBank();
                        mb.MarkVoucherAsDone(firstOrDefault.ID);
                        return true;
                    }
                    hasValidVoucher = false;
                }
                catch
                {
                    hasValidVoucher = false;
                }

                if (!hasValidVoucher)
                {
                    try
                    {
                        var dailyLimit = context.userSubcriptions.FirstOrDefault(x => x.SubcriptionID.ID == SubcriptionId && x.UserID.ID == UserId).DailyLimit;
                        //var dailyLimit = context.userSubcriptions.FirstOrDefault(u => u.UserID.ID == UserId).DailyLimit;
                        int userCredit;
                        try
                        {
                            userCredit = context.credit.Where(x => x.user.ID == UserId && x.Date == DateTime.Now.Date).Sum(x => x.CreditUsed);
                        }
                        catch
                        {
                            userCredit = 0;
                        }
                        var resourceFee = context.resource.Single(x => x.ID == ResourceId).ActivationFee;

                        if (userCredit <= dailyLimit)
                        {
                            DataBank db = new DataBank();
                            db.AddCredit(UserId, resourceFee, SubcriptionId);
                            return true;
                        }
                        return false;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public int GetUserUsageBalance(int subcriptionId, int UserId)
        {
            using (Context context = new Context())
            {
                int todaysBalance = 0;
                bool flag = false;
                    var dailyLimit = context.userSubcriptions.FirstOrDefault(u => u.UserID.ID == UserId).DailyLimit;

                    try
                    {
                        todaysBalance = context.credit.Where(x => x.Date == DateTime.Now.Date && x.user.ID == UserId).Sum(x => x.CreditUsed);
                        todaysBalance = dailyLimit - todaysBalance;
                        flag = true;
                    }
                    catch
                    {                        
                    }

                    if (!flag)
                    {
                        try
                        {
                            todaysBalance = dailyLimit;
                        }
                        catch
                        {
                        } 
                    }
                return todaysBalance;
            }
        }

        public void MarkVoucherAsDone(int voucherId)
        {
            using (Context context = new Context())
            {
                Voucher voucher = context.couchers.Find(voucherId);
                voucher.Used = true;
                context.SaveChanges();
            }
        }
    }

    public class Context : DigitalInterviewDbContext
    {                
    }
}
