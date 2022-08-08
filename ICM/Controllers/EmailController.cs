using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ICM.Controllers
{
    public class EmailController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _service;
  
        public EmailController(ApplicationDbContext context, EmailService service)
        {
           _context = context;  
            _service = service;
           
        }
        public async Task<IActionResult>  Index()
        {
            
           return View();
        }

        public async Task<IActionResult> SendEmail()
        {

            //C таймером возникли проблемы пришлось сделать так:)
            //При нажатии на кнопку Начать отправку программа просто бесконечно будет проходить цикл пока не наступит нужное время для выхода нажать Вернуться на главную страницу
            int Hour = 7;
            int Minute = 0;
            int rememberCount = 0; //переменная для цикла
            int count = 0;        //переменная для подсчета числа пользователей
            var users = await _context.Users.Include(u=>u.checkPayerAccountNumbers).ToListAsync(); //получение пользователей и их унп из бд
            var unps = await _context.PayerAccountNumbers.ToListAsync(); //получение пользователей из унп
            while (true)
            {
                if ((Hour == DateTime.Now.Hour) &&
                    (Minute == DateTime.Now.Minute))
                {
                 
                    for (; rememberCount < users.Count; rememberCount++)
                    {
                        count++; 
                        //Если пошел 101 пользователь то обнуляется счетчик и добавляется 5 минут затем выкидывает из цикла
                        if (count == 101)
                        {
                            count = 0;
                            Minute += 5;
                            break;
                        }
                        //формирование статуса
                        string status="";
                        for (int i = 0; i < users[rememberCount].checkPayerAccountNumbers.Count; i++)
                        {
                            var item = unps.Find(unp => unp.Name == users[rememberCount].checkPayerAccountNumbers[i].Name);
                            if(item == null)
                            {
                                status+= $"{users[rememberCount].checkPayerAccountNumbers[i].Name}: Нету\n";
                            }
                            else
                            {
                                status += $"{users[rememberCount].checkPayerAccountNumbers[i].Name}: Есть\n";
                            }
                        }
                            
                        //отправка емейл
                        await _service.SendEmailAsync(users[rememberCount].Email, "Тема письма", status);
                    }                    
                   
                }
                //если все пользователи в цикле пройдены обнуляется счетчик и время
                if (rememberCount>= users.Count)
                {
                    rememberCount = 0;
                    Hour = 7;
                    Minute = 0;
                   
                }
               
            }
                
            return Redirect("~/Home/Index");
            // return View();
        }

       
        
    }
}
