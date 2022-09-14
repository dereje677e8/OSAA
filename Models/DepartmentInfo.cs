using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStudentAcademicAdvisory.Models
{
    public class DepartmentInfo
    {
        public IEnumerable<Instructor> instructor { get; set; }
        public IEnumerable<StudentBatch> studentBatch { get; set; }
        public IEnumerable<AcademicInformation> academicInformation { get; set; }
        public virtual AcademicInstution AcademicInstution { get; set; }
        public virtual EducationLevel EducationLevel { get; set; }
        public virtual FieldOfStudy FieldOfStudy { get; set; }

    }
}