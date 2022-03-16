using System.Collections.Generic;
using System.Data.Entity;
using University.BL.Models;

namespace University.BL.Data
{
    public class DBContext : DbContext
    {
        public IEnumerable<object> Hiredate;

        public DBContext() : base("DBContext")
        {  
                                         

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<CourseInstructor> CourseInstructors { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
    }
}
