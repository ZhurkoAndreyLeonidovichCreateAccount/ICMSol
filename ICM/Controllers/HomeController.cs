using DataLayer;
using DataLayer.Entities;
using ICM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;


namespace ICM.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly ApplicationDbContext _context;
        private readonly EmailService _service;


        public HomeController(ApplicationDbContext context, EmailService service)
        {
           _context = context;
            _service = service;  
   
        }

        public IActionResult Index(int amount)
        {
            //параметр amount для регулировки числа полей УНП
            ViewData["Amount"] = amount==0 ? 1: amount;
            return View();
        }

        [HttpPost]
        //В этом методе показывается статус
        public async Task<IActionResult>  PreSave(UserView user)
        {
            // ViewData["isValid"] используется в представлении что бы проверить были ли валидные УНП
            //унп в модели хранится как массив проверяется только первое поле (не нашел как исправить) а второе уже пропускается хоть и ошибка помечена красным
            //поэтому пришлось дополнительно валидировать на количество символов в представлении
            //ModelState.IsValid почему то все время выдает false хотя поля помечены как nullable
            ViewData["isValid"] = true;
            for (int i = 0; i < user.PayerAccountNumber.Length; i++)
            {
                if(user.PayerAccountNumber[i].ToString().Length != 9)
                {
                    ViewData["isValid"] = false;
                }
            } 
               
                //сохранение статусов для локальной бд
                string[] Local = new string[user.PayerAccountNumber.Length];
                //сохранение статусов для внешней бд
                string[] External = new string[user.PayerAccountNumber.Length];
                //получили записи из бд для УНП
                var unp = await _context.PayerAccountNumbers.ToListAsync();

                for (int i = 0; i < user.PayerAccountNumber.Length; i++)
                {
                    //поиск в коллекции УНП записей которые ввел пользователь
                    var item = unp.Find(u => u.Name == user.PayerAccountNumber[i]);
                    //Запись статуса в массив
                    Local[i] = item != null ? "Есть" : "Нету";

                    //обработать ошибку если нет соединения
                    try
                    {
                        //Обращение к внешней бд
                        HttpClient client = new HttpClient();
                        string path = $"http://www.portal.nalog.gov.by/grp/getData?unp={user.PayerAccountNumber[i]}&charset=UTF-8&type=json";

                        HttpResponseMessage response = await client.GetAsync(path);
                        External[i] = response.IsSuccessStatusCode ? "Есть" : "Нету";
                    }
                    catch (Exception ex)
                    {
                        External[i] = "Нету соединения";
                    }




                }
                //сохранение статусов для выводов представлении
                user.LocalDb = Local;
                user.ExternalDb = External;

                return View(user);
 
            ViewData["isValid"] = false;
            return View();
        }

        [HttpPost]
        //Сохранение пользователя
        public IActionResult Save(UserView user)
        {
            //ищем пользователя с заданным email
            var userDb = _context.Users.Where(u => u.Email == user.Email).FirstOrDefault();

            if (userDb == null)
            {
                //добавляем пользователя в бд
                var addUser = new User();
                addUser.Email = user.Email;
                _context.Users.Add(addUser);
                //убираем одинаковые унп
                var distinct = user.PayerAccountNumber.Distinct().ToList();
                foreach (var item in distinct)
                {
                    var checkFirst = new CheckPayerAccountNumber
                    {
                        Name = item,
                        UserEmail = user.Email

                    };
                    //Сохраняем унп пользователя
                    //тут получается 2 таблицы User CheckPayerAccountNumber связанные foreign key 
                    _context.CheckPayerAccountNumbers.Add(checkFirst);
                }
                _context.SaveChanges();

                ViewData["Success"] = "Пользователь сохранен успешно ";
            }
            else
            {
                ViewData["Success"] = "Такой пользователь уже есть в БД ";
            }

           
            return View();
        }

        //Метод для добавления УНП
        public IActionResult AddLocal()
        {
            return View();
        }

        [HttpPost]
        //Сохранение УНП
        public IActionResult SaveLocal(PayerAccountNumber payer)
        {
            _context.PayerAccountNumbers.Add(payer);
            _context.SaveChanges();
            return View();

        }


        //метод для проверки есть ли такой УНП в бд. Если есть поле обводится красным и не дает сохранить запись в бд 
        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckPayer(int Name)
      {
            bool result = false;
            var payerAccount = _context.PayerAccountNumbers.Find(Name);
            if(payerAccount == null)
            {
                result = true;
            }

            return Json(result);
        }




    }
}