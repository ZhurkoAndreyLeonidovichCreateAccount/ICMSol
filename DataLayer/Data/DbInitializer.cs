
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Data
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _context;

        public DbInitializer(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Initialize()
        {
            _context.Database.EnsureCreated();
            if(!_context.Users.Any())
            {
                _context.AddRange(new List<User>
                {
                 new User
                 {
                       Email = "andrey675246zhurko@mail.ru",
                        checkPayerAccountNumbers = new List<CheckPayerAccountNumber>()
                        {
                            new CheckPayerAccountNumber()
                            {
                                 Name = 100000006
                            },
                             new CheckPayerAccountNumber()
                            {
                                 Name = 100000001
                            }
                        }
                       
                 }
                });

                if (!_context.PayerAccountNumbers.Any())
                {
                    _context.AddRange(new List<PayerAccountNumber>()
                    {
                        new PayerAccountNumber()
                        {
                             Name = 100000001
                        },
                         new PayerAccountNumber()
                        {
                             Name = 100000002
                        },

                        new PayerAccountNumber()
                        {
                             Name = 100000003
                        },

                        new PayerAccountNumber()
                        {
                             Name = 100000004
                        },
                        new PayerAccountNumber()
                        {
                             Name = 100000005
                        },
                    });
                }
            }
            _context.SaveChanges();
        }
    }
}
