//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineStudentAcademicAdvisory.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class InstructorImage
    {
        public int Id { get; set; }
        public System.DateTime UploadDate { get; set; }
        public string BackgroundImage { get; set; }
        public string ProfilePictureURL { get; set; }
        public System.Web.HttpPostedFileBase ImageFile { get; set; }
        public Nullable<bool> ActiveProfilePicture { get; set; }
        public Nullable<bool> ActiveBackgroundImage { get; set; }
        public int InstructorId { get; set; }
    
        public virtual Instructor Instructor { get; set; }
    }
}
