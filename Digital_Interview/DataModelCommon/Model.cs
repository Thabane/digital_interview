using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelCommon
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(
            @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$"
            )]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }
    }

    public class UserSubcription
    {
        [Key]
        public int ID { get; set; }
        public Subcription SubcriptionID { get; set; }
        public User UserID { get; set; }
        public int DailyLimit { get; set; }
    }

    public class Subcription
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Subcription Name is required.")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Credit Limit is required.")]
        public int CreditLimit { get; set; }
    }

    public class Credit
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int CreditUsed { get; set; }

        [Required]
        public User user { get; set; }

        [Required]
        public Subcription subcription { get; set; }
    }

    public class Voucher
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool Used { get; set; }

        [Required]
        public User user { get; set; }

        [Required]
        public Subcription subcription { get; set; }
    }

    public class Resource
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int ActivationFee { get; set; }
        [Required]
        public Subcription subcription { get; set; }
    
    }

    public class ToDoList
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Prority { get; set; }
        public bool Done { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public Resource Resource { get; set; }
    }

    public class UserInfo
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int DailyLimit { get; set; }        
        public string Subscription { get; set; }        
        public int SubscriptionID { get; set; }        
    }
    public class SubcriptionInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreditLimit { get; set; }
        public int NumberOfUsers { get; set; }
    }

    public class ResourceModelView
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ActivationFee { get; set; }
        public int subcriptionId { get; set; }
    }
    public class ToDoListViewModel
    {
        public int ID { get; set; }     
        public string Title { get; set; }
        public string Content { get; set; }
        public int Prority { get; set; }
        public bool Done { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public int Resource { get; set; }
    }

    public class Dashboard
    {
        public int Subscriptions { get; set; }
        public int Users { get; set; }
        public int Resources { get; set; }
        public int Vouchers { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
