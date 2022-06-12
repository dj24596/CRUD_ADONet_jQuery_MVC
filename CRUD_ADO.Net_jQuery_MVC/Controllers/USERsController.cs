using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRUD_ADONet_jQuery_MVC.Models;

namespace CRUD_ADONet_jQuery_MVC.Controllers
{
    public class USERsController : Controller
    {
        private PRACTICAL_ASSIGNMENTEntities db = new PRACTICAL_ASSIGNMENTEntities();

        // GET: USERs
        public ActionResult Index()
        {
            return View(db.USERs.ToList());
        }
                
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.USER_ID == 0)
                {                    
                    var usr = new USER()
                    {
                        Fname = model.Fname,
                        Lname = model.Lname,
                        Email = model.Email,
                        Phone = model.Phone,
                        Password = model.Password,
                        IsActive = true,
                        CreatedON = DateTime.Now
                    };                    
                    db.USERs.Add(usr);
                    db.SaveChanges();
                }                               
            }
            else
            {
                return View(model);
            }               
            return RedirectToAction("Index");
        }             

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
