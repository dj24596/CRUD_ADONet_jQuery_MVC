using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRUD_ADONet_jQuery_MVC.Models;
using Newtonsoft.Json;

namespace CRUD_ADONet_jQuery_MVC.Controllers
{
    public class BlogsController : Controller
    {
        private PRACTICAL_ASSIGNMENTEntities db = new PRACTICAL_ASSIGNMENTEntities();

        // GET: Blogs
        public ActionResult Index()
        {
            try
            {
                List<BlogModel> blg = new List<BlogModel>();                              
                blg = db.Blogs.Select(e => new BlogModel
                {
                    BlogID = e.BlogID,
                    Title = e.Title,
                    Description = e.Description,
                    CreatedOn = e.CreatedOn,
                    Author = e.USER.Lname + " " + e.USER.Fname,    
                    Likes = e.Likes,
                    UserID = e.Author
                }).ToList();

                List<BlogModel> blgL = new List<BlogModel>();
                UserModel currentUser = (UserModel)Session["CurrentUser"];                
                
                foreach (var a in blg)
                {
                    BlogModel blog = new BlogModel();
                    blog.Author = a.Author;
                    blog.BlogID = a.BlogID;
                    blog.Title = a.Title;
                    blog.Description = a.Description;
                    blog.CreatedOn = a.CreatedOn;
                    blog.Likes = a.Likes;
                    blog.UserID = a.UserID;                    
                    List<int> Likeslist = new List<int>();
                    Likeslist = JsonConvert.DeserializeObject<List<int>>(a.Likes);
                    if (Likeslist == null)
                    {
                        blog.LikeFlag = true;
                        blog.BlogCount = 0;
                    }
                    else 
                    {
                        List<int> lstusr = new List<int>();
                        foreach(var l in Likeslist)
                        {                            
                            int usr = db.BlogLikes.Where(m => m.ID == l).Select(m => m.UserID).FirstOrDefault();
                            lstusr.Add(usr);                            
                        }
                        if (lstusr.Contains(currentUser.USER_ID))
                        {                            
                            blog.LikeFlag = false;
                            blog.BlogCount = Likeslist.Count();                            
                        }
                        else
                        {                            
                            blog.LikeFlag = true;
                            blog.BlogCount = Likeslist.Count();                            
                        }
                    }                    
                    blgL.Add(blog);
                }
                return View(blgL);
            }
            catch (Exception)
            {
                throw;
            }            
        }
       
        public ActionResult Details(int? id)
        {
            BlogModel bModel = new BlogModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            
            if (id > 0)
            {
                var r = db.Blogs.Where(x => x.BlogID == id).FirstOrDefault();
                bModel.Title = r.Title;
                bModel.Description = r.Description;
                bModel.CreatedOn = r.CreatedOn;
                bModel.Author = r.USER.Fname + " " + r.USER.Lname;                
                bModel.BlogID = r.BlogID;

                List<string> bl = new List<string>();
                List<int> Likeslist = new List<int>();
                Likeslist = JsonConvert.DeserializeObject<List<int>>(r.Likes);
                if(Likeslist != null)
                {
                    foreach (var a in Likeslist)
                    {
                        string likename = "";
                        likename = db.BlogLikes.Where(m => m.ID == a).Select(m => m.USER.Lname + " " + m.USER.Fname).FirstOrDefault();
                        bl.Add(likename);
                    }
                    bModel.BlogLikes = bl.ToList();
                }
            }
            if (bModel == null)
            {
                return HttpNotFound();
            }
            return View(bModel);
        }        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogModel model)
        {
            UserModel currentUser = (UserModel)Session["CurrentUser"];
            if (ModelState.IsValid)
            {
                if (model.BlogID == 0)
                {
                    var blg = new Blog()
                    {
                        Title = model.Title,
                        Description = model.Description,
                        CreatedOn = DateTime.Now,
                        Author = currentUser.USER_ID,
                        Likes = "",
                        
                    };
                    db.Blogs.Add(blg);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(model);
            }
            return RedirectToAction("Index");            
        }
        
        public ActionResult Edit(int? id)
        {
            BlogModel bModel = new BlogModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id > 0)
            {                
                var r = db.Blogs.Where(x => x.BlogID == id).FirstOrDefault();
                bModel.Title = r.Title;
                bModel.Description = r.Description;
                bModel.BlogID = r.BlogID;
            }            
            if (bModel == null)
            {
                return HttpNotFound();
            }
            return View(bModel);
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( BlogModel model)
        {
            if (ModelState.IsValid)
            {
                var r = db.Blogs.Where(x => x.BlogID == model.BlogID).FirstOrDefault();
                r.Title = model.Title;
                r.Description = model.Description;                                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blog = db.Blogs.Find(id);
            db.Blogs.Remove(blog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public JsonResult CreateBlogLike(int BlogID)
        {            
            if (ModelState.IsValid)
            {
                BlogModel bModel = new BlogModel();
                UserModel currentUser = (UserModel)Session["CurrentUser"];
                if (BlogID > 0)
                {
                    var r = db.Blogs.Where(x => x.BlogID == BlogID).FirstOrDefault();
                    if(r != null)
                    {                        
                        bModel.BlogID = r.BlogID;
                        var blglike = new BlogLike()
                        {
                            BlogID = bModel.BlogID,
                            UserID = currentUser.USER_ID,
                            LikedOn = DateTime.Now,
                        };
                        blglike = db.BlogLikes.Add(blglike);
                        db.SaveChanges();

                        List<int> Likeslist = new List<int>();
                        Likeslist = JsonConvert.DeserializeObject<List<int>>(r.Likes);
                        if (Likeslist == null)
                        {
                            Likeslist = new List<int>();
                            Likeslist.Add(blglike.ID);
                        }
                        else if (!Likeslist.Contains(blglike.ID))
                        {
                            Likeslist.Add(blglike.ID);
                        }
                        
                        string strserialize = JsonConvert.SerializeObject(Likeslist);

                        r.Likes = strserialize;
                        db.SaveChanges();

                        return Json(new { result = "success" });
                    }
                    else
                    {
                        return Json(new { result = "failure" });
                    }
                }
                else
                {
                    return Json(new { result = "failure" });
                }                
            }
            else
            {
                return Json(new { result = "failure" });                
            }            
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
