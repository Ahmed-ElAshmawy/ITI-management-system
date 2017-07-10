using ITI_System.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ITI_System.Controllers
{
    public class InstructorController : Controller
    {
        // GET: Instructor
        static ApplicationDbContext db = new ApplicationDbContext();
        SelectList dept_drop = new SelectList(db.Departments, "Dept_Id", "Dept_name");
        SelectList crs_drop = new SelectList(db.Courses, "Crs_Id", "Crs_name");
        SelectList ins_drop =
    new SelectList((from i in db.Instructor.ToList()
                    select new
                    {
                        Id = i.Id,
                        FullName = i.FirstName + " " + i.LastName
                    }),
        "Id",
        "FullName",
        null);
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var InsList = db.Instructor.ToList();
            return View(InsList);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult create()
        {
            ViewBag.dept_drop = dept_drop;
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> create(InstructorViewModel Ins)
        {
            if (ModelState.IsValid)
            {
                if (Ins.Dept_Id == null)
                    Ins.Ins_Status = "External";
                else
                    Ins.Ins_Status = "Internal";

                Instructor newIns = new Instructor()
                {
                    FirstName = Ins.FirstName,
                    LastName = Ins.LastName,
                    Email = Ins.Email,
                    PhoneNumber = Ins.PhoneNumber,
                    BirthDate = Ins.BirthDate.Date,
                    Ins_GraduationYear = Ins.Ins_GraduationYear.Date,
                    Ins_Qualifications = Ins.Ins_Qualifications,
                    IsMarried = Ins.IsMarried,
                    Dept_Id = Ins.Dept_Id,
                    Ins_Status = Ins.Ins_Status,
                    UserName = Ins.Email
                };
                ApplicationUserManager Usermanager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var res = await Usermanager.CreateAsync(newIns, Ins.Password);
                if (res.Succeeded)
                {
                    if (newIns.Dept_Id != null)
                        await Usermanager.AddToRoleAsync(newIns.Id, "Instructor_Internal");
                    else
                        await Usermanager.AddToRoleAsync(newIns.Id, "Instructor_External");
                    return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("create");
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> delete(string id)
        {
            Instructor Ins = db.Instructor.FirstOrDefault(a => a.Id == id);

            ApplicationUserManager Usermanager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            if (Ins.Ins_Status == "Internal")
            {
                await Usermanager.RemoveFromRoleAsync(Ins.Id, "Instructor_Internal");
            }
            else
                await Usermanager.RemoveFromRoleAsync(Ins.Id, "Instructor_External");

            db.Instructor.Remove(Ins);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            Instructor Ins = db.Instructor.FirstOrDefault(a => a.Id == id);
            InstructorViewModel Insviewmodel = new InstructorViewModel()
            {
                Id = Ins.Id,
                FirstName = Ins.FirstName,
                LastName = Ins.LastName,
                PhoneNumber = Ins.PhoneNumber,
                BirthDate = Ins.BirthDate.Date,
                Ins_GraduationYear = Ins.Ins_GraduationYear.Date,
                Ins_Qualifications = Ins.Ins_Qualifications,
                IsMarried = Ins.IsMarried,
                Dept_Id = Ins.Dept_Id,
                Ins_Status = Ins.Ins_Status
            };

            ViewBag.dept_drop = dept_drop;
            return PartialView(Insviewmodel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(InstructorViewModel newIns)
        {
            Instructor Ins = db.Instructor.FirstOrDefault(a => a.Id == newIns.Id);
            Ins.FirstName = newIns.FirstName;
            Ins.LastName = newIns.LastName;
            Ins.PhoneNumber = newIns.PhoneNumber;
            Ins.Ins_GraduationYear = newIns.Ins_GraduationYear;
            Ins.Ins_Qualifications = newIns.Ins_Qualifications;
            Ins.BirthDate = newIns.BirthDate;
            Ins.IsMarried = newIns.IsMarried;
            Ins.Dept_Id = newIns.Dept_Id;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Details(string id)
        {
            Instructor Ins = db.Instructor.FirstOrDefault(a => a.Id == id);
            return PartialView(Ins);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DisplayInsCourse()
        {
            ViewBag.ins_drop = ins_drop;
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult GetInsCourse(string Id)
        {
            List<Course> crs = new List<Course>();
            if (Id != null)
            {
                crs = db.Instructor.FirstOrDefault(a => a.Id == Id).Crs_List;
            }
            return PartialView(crs);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult CrsList()
        {
            ViewBag.crs_drop = crs_drop;
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult CrsDept(int Id)
        {
            var dept_list = db.Courses.FirstOrDefault(a => a.Crs_Id == Id).Dept_List;
            SelectList deptCrs_drop = new SelectList(dept_list, "Dept_Id", "Dept_name");
            ViewBag.deptCrs_drop = deptCrs_drop;
            return PartialView();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult CrsInsDept(int Id, int crs_id)
        {
            var ins_list = (from i in db.Department_Instructor_Course
                            where i.Dept_Id == Id && i.Crs_Id == crs_id
                            select i.Ins_Id).ToList();

            List<Instructor> inslists = new List<Instructor>();

            foreach (var ins in ins_list)
            {
                var insobj = db.Instructor.FirstOrDefault(a => a.Id == ins);
                inslists.Add(insobj);
            }
            SelectList ins_drop =
    new SelectList((from i in inslists
                    select new
                    {
                        Id = i.Id,
                        FullName = i.FirstName + " " + i.LastName
                    }),
        "Id",
        "FullName",
        null);
            ViewBag.ins_drop = ins_drop;
            return PartialView();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult InsCrsStd(string Id, int crs_id)
        {
            List<Student_Course_Instructor> OBJ_list = new List<Student_Course_Instructor>();
            List<Student> stdlist = new List<Student>();
            if (Id != null)
            {
                OBJ_list = (from i in db.Student_Course_Instructor
                            where i.Ins_Id == Id && i.Crs_Id == crs_id
                            select i).ToList();

                foreach (var obj in OBJ_list)
                {
                    var stdobj = db.Students.FirstOrDefault(a => a.Id == obj.Std_Id);
                    stdlist.Add(stdobj);
                }

            }
            TempData["stdlist"] = stdlist;
            return View(OBJ_list);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult updatestdgrades(Student_Course_Instructor[] stds)
        {
            for (int i = 0; i < stds.Length; i++)
            {
                var stdid = stds[i].Std_Id;
                var crsid = stds[i].Crs_Id;
                var x = db.Student_Course_Instructor.Where(a => a.Std_Id == stdid && a.Crs_Id == crsid).First();
                x.Std_Lab_Eval = stds[i].Std_Lab_Eval;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Manager")]
        public ActionResult MgrStdGrades()
        {
            var mngid = User.Identity.GetUserId();
            var Student_Course_Instructor = db.Student_Course_Instructor.Where(c => c.Student.Department.Manager_Id == mngid).ToList();
            List<Student> stdlist = new List<Student>();
            List<Course> crslist = new List<Course>();
            List<StudentExam> StudentExam = new List<StudentExam>();
            for (int i = 0; i < Student_Course_Instructor.Count; i++)
            {
                string stdid = Student_Course_Instructor[i].Std_Id;
                var std = db.Students.FirstOrDefault(a => a.Id == stdid);
                stdlist.Add(std);

                int crsid = Student_Course_Instructor[i].Crs_Id;
                var crs = db.Courses.FirstOrDefault(c => c.Crs_Id == crsid);
                crslist.Add(crs);
                
                var stdexam = db.StudentExam.Where(a => a.Std_Id == stdid && a.Exam_Id == crs.Exams_List.Select(r => r.Exam_Id).First()).First();
                StudentExam.Add(stdexam);
            }
            TempData["stdlist"] = stdlist;
            TempData["crslist"] = crslist;
            TempData["StudentExam"] = StudentExam;
            return View(Student_Course_Instructor);
        }

        [Authorize(Roles = "Manager")]
        public ActionResult MgrInsEval()
        {
            var mngid = User.Identity.GetUserId();
            var Dept_id = db.Instructor.FirstOrDefault(a => a.Id == mngid).Dept_Id;
            var ins_list = db.Departments.FirstOrDefault(a => a.Manager_Id == mngid).Ins_list;

            List<Student_Course_Instructor> sci_list = (from i in db.Student_Course_Instructor
                                                        where i.Instructor.Dept_Id == Dept_id
                                                        select i).ToList();

            return View(sci_list);
        }





        [Authorize(Roles = "Manager")]
        public ActionResult Permissions()
        {
            var mail = User.Identity.Name;
            var dept_id = db.Instructor.FirstOrDefault(a => a.Email == mail).Dept_Id;
            var std_list = (from p in db.Permissions
                            where p.Student.Dept_Id == dept_id
                            select p).ToList();

            List<Permission> per_list = new List<Permission>();

            foreach (var s in std_list)
            {
                var per = (from p in db.Permissions
                           where p.Stud_Id == s.Stud_Id
                           select p).ToList();
                if (per.Count != 0)
                {
                    per_list.AddRange(per);
                }
            }
            ViewBag.dept_id = dept_id;

            return View(per_list);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public ActionResult CreatePermission(int Id)
        {
            var std_list = db.Departments.FirstOrDefault(a => a.Dept_Id == Id).Std_List;
            SelectList std_drop =
                       new SelectList((from i in std_list
                                       select new
                                       {
                                           Id = i.Id,
                                           FullName = i.FirstName + " " + i.LastName
                                       }),
                         "Id", "FullName", null);
            ViewBag.std_drop = std_drop;
            return View();
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public ActionResult CreatePermission(string Stud_Id, DateTime Perm_Date)
        {
            Permission per = new Permission();
            per.Stud_Id = Stud_Id;
            per.Perm_Date = Perm_Date.Date;
            db.Permissions.Add(per);
            db.SaveChanges();
            return RedirectToAction("Permissions");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult GetDept()
        {
            ViewBag.deps = new SelectList(db.Departments, "Dept_Id", "Dept_name");
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult GetDeptSelected(int id)
        {
            var student_list = db.Students.Where(s => s.Dept_Id == id).ToList();
            ViewBag.std = new SelectList(student_list, "Id", "FirstName");
            TempData["DeptId"] = id;
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult studentID(string id)
        {
            TempData["StudentId"] = id;
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Report(st_permession model)
        {
            var id = TempData["StudentId"] as string;
            List<Attendence> std_list = new List<Attendence>();

            var att = db.Attendence.ToList();
            foreach (var item in att)
            {
                Attendence st = att.FirstOrDefault(s => item.Id == id);
                std_list.Add(st);
            }
            return View(std_list);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult reportStdInDate()
        {
            ViewBag.dept_drop = dept_drop;

            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult reportStdInDateGet(int Dept_Id, DateTime Date)
        {
            List<Student> realdata = new List<Student>();

            var dateStd = (from i in db.Attendence
                           from s in db.Students
                           where i.Id == s.Id
                           where (i.Absence_Date != Date)
                           select s).ToArray();

            var allstds = db.Students.ToArray();

            realdata = allstds.Except(dateStd).ToList();

            return PartialView(realdata);
        }

    }
}