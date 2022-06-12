using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using CRUD_ADONet_jQuery_MVC.Models;
using System.Web.Security;

namespace CRUD_ADONet_jQuery_MVC.Controllers
{
    public class HomeController : Controller
    {
        private PRACTICAL_ASSIGNMENTEntities db = new PRACTICAL_ASSIGNMENTEntities();
        // GET: Home
        public ActionResult Index(UserModel model)
        {                        
            return View(model);
        }  
        public ActionResult Login()
        {
            UserModel model = new UserModel();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserModel model)
        {
            UserModel uInfo = null;                            
            var user = db.USERs.Where(x => x.Email == model.Email & x.Password == model.Password).FirstOrDefault();

            if (user != null)
            {
                if (user.IsActive)
                {
                    uInfo = new UserModel()
                    {
                        Fname = user.Fname,
                        Lname = user.Lname,
                        Email = user.Email,                                                        
                        USER_ID = user.USER_ID
                    };

                    HttpContext.Session.Add("CurrentUser", uInfo);
                }
            }            

            if (uInfo != null)
            {
                ViewBag.UserName = uInfo.Lname + " " + uInfo.Fname;
                
                return RedirectToAction("Index", "Home", uInfo);
            }
            else
            {
                UserModel Umodel = new UserModel();
                Umodel.ErrorMessage = "Incorrect Login Details";
                return View(Umodel);
            }            
        }        

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }
    }
}