using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRUD_ADONet_jQuery_MVC.Models
{
    public class BlogLikeModel
    {
        public int ID { get; set; }
        public int BlogID { get; set; }
        public int UserID { get; set; }
        public System.DateTime LikedOn { get; set; }

    }
}