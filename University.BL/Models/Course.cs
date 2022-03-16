using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.BL.Models
{
    [Table("Course",Schema ="dbo")]
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CourseID { get; set; }
        public string Title { get; set; }
        public int Credits{ get; set; }

        //references
        public virtual IEnumerable<Enrollment> Enrollments { get; set; }
        public virtual IEnumerable<CourseInstructor> CourseInstructors { get; set; }

    }
}
