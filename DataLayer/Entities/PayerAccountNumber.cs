using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
   public class PayerAccountNumber
   {
        [Key]
        [Required(ErrorMessage = "Введите УНП")]
        [Range(100000000, 999999999, ErrorMessage = "Недопустимый УДН")]
        [Remote(action: "CheckPayer", controller: "Home", ErrorMessage = "Такой УНП уже есть в БД")]
        public int Name { get; set; }

      


    }
}
