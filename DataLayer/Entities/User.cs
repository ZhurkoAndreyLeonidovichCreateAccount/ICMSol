using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class User
    {
        [Key]
        public string Email { get; set; }

        //Навигационные свойства
        public List<CheckPayerAccountNumber> checkPayerAccountNumbers { get; set; }
      
    }
}
