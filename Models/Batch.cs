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
    
    public partial class Batch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Batch()
        {
            this.StudentBatches = new HashSet<StudentBatch>();
        }
    
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int ProgramId { get; set; }
        public int AdmissionId { get; set; }
        public System.DateTime EntryYear { get; set; }
    
        public virtual AdmissionType AdmissionType { get; set; }
        public virtual Department Department { get; set; }
        public virtual Program Program { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentBatch> StudentBatches { get; set; }
    }
}
