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
    
    public partial class InstructorFeedback
    {
        public int Id { get; set; }
        public int InstructorId { get; set; }
        public string FeedbackContent { get; set; }
        public System.DateTime FeedDate { get; set; }
    
        public virtual Instructor Instructor { get; set; }
    }
}
