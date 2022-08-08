using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class CheckPayerAccountNumber
    {
        
        public int Name { get; set; }

        //Навигационные свойства
        public User User { get; set; }

        public string UserEmail { get; set; }
    }
}
