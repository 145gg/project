using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace project_ver1.Models
{


    public partial class Customer
    {
        public int ID { get; set; }


        public string? Name { get; set; }


        public string? Pwd { get; set; }

        public string? Email { get; set; }



        public string? Phone { get; set; }


        public string? Address { get; set; }


        public virtual ICollection<Agricultural_Order> AgriculturalOrders { get; set; } = new List<Agricultural_Order>();

        public virtual ICollection<MealOrder> MealOrders { get; set; } = new List<MealOrder>();

        public virtual ICollection<Room_Order> RoomOrders { get; set; } = new List<Room_Order>();
    }
    public class LoginViewModel
    {
        [Required(ErrorMessage = "請輸入電子郵件")]
        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        public string? Password { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Token { get; set; }

    }


}
