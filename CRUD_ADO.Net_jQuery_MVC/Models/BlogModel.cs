using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRUD_ADONet_jQuery_MVC.Models
{
    public class BlogModel
    {
        public int BlogID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Likes { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UserID { get; set; }
        public int BlogCount { get; set; }
        public bool LikeFlag { get; set; }
        public List<string> BlogLikes { get; set; }
    }
}