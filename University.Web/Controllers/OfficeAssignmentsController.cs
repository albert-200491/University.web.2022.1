using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.BL.Data;
using University.BL.DTOs;
using University.BL.Models;

namespace University.Web.Controllers
{
    public class OfficeAssignmentsController : Controller
    {
        private readonly DBContext context = new DBContext();

        [HttpGet]
        public ActionResult Create()
        {
            GetInstructors();
            return View();
        }

        [HttpPost]
        public ActionResult Create(OfficeAssignmentDTO officeAssignmentDTO)
        {
            GetInstructors();
            if (ModelState.IsValid)
            {
                var officeModel = new OfficeAssignment
                {
                    InstructorID = officeAssignmentDTO.InstructorID,
                    Location = officeAssignmentDTO.Location,
                };
                context.OfficeAssignments.Add(officeModel);
                context.SaveChanges();

                //return RedirectToAction("Index");
            }

            return View(officeAssignmentDTO);
        }

        private void GetInstructors()
        {
            var instructorsModel = context.Instructors.ToList();
            var instructorDTO = instructorsModel.Select(x => ConvertInstructor(x)).ToList();
            ViewBag.Instructors = new SelectList(instructorDTO, "ID", "FullName");
            
        }
        private InstructorDTO ConvertInstructor(Instructor instructor)
        {

            return new InstructorDTO
            {
                ID = instructor.ID,
                FirstMidName = instructor.FirstMidName,
                LastName = instructor.LastName,
                HireDate = instructor.HireDate,
            };
        }

        [HttpGet]
        public ActionResult Edit()
        {
            GetInstructors();
            return View();
        }
        
    }
}