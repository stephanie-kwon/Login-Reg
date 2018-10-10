using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Login.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Login.Controllers
{
    public class HomeController : Controller
    {
        private YourContext dbContext;
        public HomeController(YourContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        [Route("")]
        public ViewResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Home/RegisterInfo")]
        public IActionResult ReigsterInfo (User user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Login.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);

                User newUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password
                };

                dbContext.Add(newUser);
                dbContext.SaveChanges();

                HttpContext.Session.SetInt32("id", user.id);
                int? id = HttpContext.Session.GetInt32("id");
                return RedirectToAction("ShowSuccess");
            }
            else
            {
                return View("Index");
            }
        }
        [HttpGet]
        [Route("Success")]
        public IActionResult ShowSuccess()
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return Redirect("/");
            }
            return View("Success");
        }

        [HttpGet]
        [Route("Login")]
        public ViewResult Login()
        {
        
            return View("Login");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            System.Console.WriteLine("8201938120");
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        [Route("loginuser")]
        public IActionResult loginuser(LoginUser userSubmission)
        {
           if(ModelState.IsValid)
           {
               var userInDb = dbContext.Login.FirstOrDefault(u => u.Email == userSubmission.Email);
               
               if (userInDb == null)
               {
                   ModelState.AddModelError("Email", "Invalid Email");
                   return View("Login");
        
                }

               var hasher = new PasswordHasher<LoginUser>();
               var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);
               
               if(result == 0)
                {
                    ModelState.AddModelError("Password", "Invalid Password");
                    System.Console.WriteLine("pASSWORDDD INVALIDDDD");
                    return View("Login");
        
                }
    
            HttpContext.Session.SetInt32("id", userInDb.id);
            int? id = HttpContext.Session.GetInt32("id");

            return View("Success");
            } else {
                return View ("Login");
            }     
        }
    }
}




