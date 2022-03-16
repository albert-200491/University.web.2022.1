using System;
using System.ComponentModel.DataAnnotations;

namespace University.BL.DTOs
{
    public class DepartmentDTO
    {
        public int DepartmentID { get; set; }
       
        [Display(Name = "Name")]
        public string Name { get; set; }

       
        [Display(Name = "Budget")]

        public float Budget { get; set; }
       
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        public int InstructorID { get; set; }
    }
}
