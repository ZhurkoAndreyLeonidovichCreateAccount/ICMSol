using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ICM.Models
{
    public class UserView
    {
        

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Не указан Емайл")]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }



        [Required]
        [Range(100000000, 999999999, ErrorMessage = "Недопустимый УДН")]
        [Display(Name = "УНП")]
        public  int[] PayerAccountNumber { get; set; }

        public string[]? LocalDb { get; set; } 
        public string[]? ExternalDb { get; set; } 
    }
}
