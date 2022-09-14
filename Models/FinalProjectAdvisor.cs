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
    
    public partial class FinalProjectAdvisor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FinalProjectAdvisor()
        {
            this.AdvisorDocuments = new HashSet<AdvisorDocument>();
        }
    
        public int Id { get; set; }
        public int FinalProjectId { get; set; }
        public int InstractorId { get; set; }
        public int AdvisorTypeId { get; set; }
        public bool IsTerminated { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdvisorDocument> AdvisorDocuments { get; set; }
        public virtual AdvisorType AdvisorType { get; set; }
        public virtual FinalProject FinalProject { get; set; }
        public virtual Instructor Instructor { get; set; }
    }
}
