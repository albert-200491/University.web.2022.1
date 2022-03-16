using System;
using System.ComponentModel.DataAnnotations;

namespace University.BL.DTOs
{
    public class InstructorDTO
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "The field Last Name is required")]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The field FirstMid Name is required")]
        [StringLength(50)]
        [Display(Name = "firstMid name")]
        public string FirstMidName { get; set; }

        [Required(ErrorMessage = "The field Hire Date is required")]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return $"{LastName} {FirstMidName}"; }
        }
    }

}
