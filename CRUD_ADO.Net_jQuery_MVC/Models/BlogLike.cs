//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CRUD_ADONet_jQuery_MVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BlogLike
    {
        public int ID { get; set; }
        public int BlogID { get; set; }
        public int UserID { get; set; }
        public System.DateTime LikedOn { get; set; }
    
        public virtual USER USER { get; set; }
        public virtual Blog Blog { get; set; }
    }
}
