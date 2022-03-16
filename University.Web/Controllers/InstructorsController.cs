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
    public class InstructorsController : Controller
    {
        private readonly DBContext context = new DBContext();
        // GET: Instructors
        private static List<InstructorDTO> students = new List<InstructorDTO>
        {
            new InstructorDTO{ID = 1, FirstMidName = "Albert", LastName = "beto",HireDate=DateTime.UtcNow},
            new InstructorDTO{ID = 2, FirstMidName = "alex", LastName = "casa",HireDate=DateTime.UtcNow},
            new InstructorDTO{ID = 3, FirstMidName = "pablo", LastName = "el",HireDate=DateTime.UtcNow}
        };
        //Students/Index
        // GET: Students
        [HttpGet]
        public ActionResult Index(int? id)
        {
            #region Traspaso de  datos
            //1.View(model)
            //2.ViewBag
            //3.ViewData
            //4.session

            ViewBag.StudensCount = students.Count;
            ViewBag.StudentFirst = students.FirstOrDefault();

            ViewData["StudentCount"] = students.Count;
            ViewData["StudentCount"] = students.FirstOrDefault();

            Session["StudentsCount"] = students.Count;
            var count = (int)Session["StudentsCount"];
            Session.Remove("StudentsCount");
            Session.RemoveAll();
            #endregion

            var instructorsModel = context.Instructors.ToList();
            var instructorsDTO = instructorsModel.Select(j => ConvertInstructor(j)).ToList();

            if (id != null)
            {
                //SELET _ courses.*
                //* FROM Enrollment _enrolments
                // JOIN Courses _courses ON _enrollments.CourseID = _courses.CourseID
                //WHERE 
                var coursesModel = (from _enrollments in context.CourseInstructors
                                    join _courses in context.Courses
                                    on _enrollments.CourseID equals _courses.CourseID
                                    where _enrollments.InstructorID == id.Value
                                    select _courses).ToList();

                var coursesDTO = coursesModel.Select(x => ConvertCourse(x)).ToList();

                ViewBag.Courses = coursesDTO;
            }

            return View(instructorsDTO);
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

        private CourseDTO ConvertCourse(Course course)
        {

            return new CourseDTO
            {
                CourseID = course.CourseID,
                Title = course.Title,
                Credits = course.Credits,

            };
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(InstructorDTO instructorDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Ef
                    //Insert into Student(firstMidNmae ......)
                    //Values(@firsMidName
                    var instructor = new Instructor
                    {
                        LastName = instructorDTO.LastName,
                        FirstMidName = instructorDTO.FirstMidName,
                        HireDate = instructorDTO.HireDate,
                    };
                    context.Instructors.Add(instructor);
                    context.SaveChanges();



                    return RedirectToAction(nameof(Index));
                }
            }

            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

            }

            return View(instructorDTO);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var instructorModel = context.Instructors.Find(id);
            var instructorDTO = ConvertInstructor(instructorModel);

            return View(instructorDTO);
        }

        [HttpPost]
        public ActionResult Edit(InstructorDTO instructorDTO)
        {
            try

            {
                if (ModelState.IsValid)
                {
                    var instructorModel = context.Instructors.Find(instructorDTO.ID);
                    //campos a modificar   // UPDATE student SET = FirstMidName = @FirstMidName
                    //where ID = @ID
                    instructorModel.FirstMidName = instructorDTO.FirstMidName;
                    instructorModel.LastName = instructorDTO.LastName;
                    instructorModel.HireDate = instructorDTO.HireDate;

                    context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
            }

            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

            }

            return View(instructorDTO);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                //Dependencias

                var courses = context.CourseInstructors.Where(x => x.InstructorID == id).ToList();
                var offices = context.OfficeAssignments.Where(x => x.InstructorID == id).ToList();
                var departments = context.Departments.Where(x => x.InstructorID == id).ToList();
                if (!courses.Any() && !offices.Any() && !departments.Any())
                {
                    var instructorModel = context.Instructors.Find(id);
                    context.Instructors.Remove(instructorModel);
                    context.SaveChanges();
                }
                else
                    throw new Exception("Tiene cursos matriculados");


            }
            catch (Exception ex)
            {
                var message = ex.Message;

            }
            return RedirectToAction(nameof(Index));

        }
    }
}