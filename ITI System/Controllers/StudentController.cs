using ITI_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;


namespace ITI_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentController : Controller
    {
        static ApplicationDbContext db = new ApplicationDbContext();
        SelectList dept_drop = new SelectList(db.Departments, "Dept_Id", "Dept_name");
        // GET: Student
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var st_list = db.Students.ToList();
            return View(st_list);
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
        public async Task<ActionResult> create(StudentViewModel std)
        {
            if (ModelState.IsValid)
            {
                Department D = db.Departments.FirstOrDefault(a => a.Dept_Id == std.Dept_Id);
                if (D != null)
                {
                    int st_count = D.Std_List.Count();

                    if (D.Dept_Capacity == st_count)
                    {
                        std.Dept_Id = null;
                    }
                }

                Student newstudent = new Student()
                {
                    BirthDate = std.BirthDate,
                    Dept_Id = std.Dept_Id,
                    Email = std.Email,
                    FirstName = std.FirstName,
                    LastName = std.LastName,
                    IsMarried = std.IsMarried,
                    PhoneNumber = std.PhoneNumber,
                    UserName = std.Email
                };

                ApplicationUserManager Usermanager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var res = await Usermanager.CreateAsync(newstudent, std.Password);
                if (res.Succeeded)
                {
                    await Usermanager.AddToRoleAsync(newstudent.Id, "Student");
                    return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("create");
            }
            else
            {
                var depts = db.Departments.ToList();
                SelectList d = new SelectList(depts, "Dept_Id", "Dept_name");
                ViewBag.dep = d;
                return View("create", std);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult delete(string id)
        {
            Student stud = db.Students.FirstOrDefault(a => a.Id == id);
            return PartialView(stud);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> deleteStud(string id)
        {
            Student stud = db.Students.FirstOrDefault(a => a.Id == id);

            ApplicationUserManager Usermanager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            await Usermanager.RemoveFromRoleAsync(stud.Id, "Student");

            db.Students.Remove(stud);
            db.SaveChanges();
            return PartialView("std_list", db.Students.ToList());
        }
        [Authorize(Roles = "Admin")]
        public ActionResult details(string id)
        {
            Student stud = db.Students.FirstOrDefault(a => a.Id == id);
            return PartialView(stud);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult edit(string id)
        {
            Student stud = db.Students.FirstOrDefault(a => a.Id == id);
            ViewBag.dept_drop = dept_drop;
            return PartialView(stud);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult edit(Student editedStud)
        {
            Student stud = db.Students.FirstOrDefault(a => a.Id == editedStud.Id);

            stud.BirthDate = editedStud.BirthDate;
            stud.Dept_Id = editedStud.Dept_Id;
            stud.FirstName = editedStud.FirstName;
            stud.LastName = editedStud.LastName;
            stud.IsMarried = editedStud.IsMarried;
            stud.PhoneNumber = editedStud.PhoneNumber;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult stdWithoutDep()
        {
            ViewBag.depts = dept_drop;
            var stds_without_dep = (from s in db.Students where (s.Dept_Id == null) select s).ToList();
            return View(stds_without_dep);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult addToDep(string[] stdindep, int deparetment)
        {
            try
            {
                for (int i = 0; i < stdindep.Length; i++)
                {
                    Student st = db.Students.Find(stdindep[i]);
                    Department D = db.Departments.FirstOrDefault(a => a.Dept_Id == deparetment);
                    var stds = db.Students;
                    int stds_in_dep = (from std in stds where std.Dept_Id == std.Dept_Id select std).Count();

                    if (D.Dept_Capacity <= stds_in_dep)
                    {
                        st.Dept_Id = null;
                    }
                    else
                    {

                        st.Dept_Id = deparetment;
                    }


                }
                db.SaveChanges();

            }
            catch
            {

            }
            return RedirectToAction("stdWithoutDep");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult courses(string id)
        {
            List<Student_Course_Instructor> st_cr_ins = db.Student_Course_Instructor.ToList();
            List<Student> students = db.Students.ToList();
            List<Course> crs = db.Courses.ToList();
            List<int> crses = (from c in st_cr_ins where c.Std_Id == id select c.Crs_Id).ToList();//list of student courses(id)
            List<string> crses_name = new List<string>();//list of student courses(name)
            for (var i = 0; i < crses.Count(); i++)
            {
                string crs_nm = (from c in crs where c.Crs_Id == crses[i] select c.Crs_name).First();
                crses_name.Add(crs_nm);
            }
            List<StudentExam> std_exam = db.StudentExam.ToList();
            List<Exam> exams = db.Exams.ToList();
            List<int> grades = new List<int>();


            for (var i = 0; i < crses.Count(); i++)
            {
                var query =
   (from post in std_exam
    join meta in exams on post.Exam_Id equals meta.Exam_Id
    where post.Std_Id == id && meta.Crs_Id == crses[i]
    select post.Std_Grade).First();
                grades.Add(query);//gradesin courses
            }
            ViewBag.courses = crses_name;
            ViewBag.grades = grades;
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult eval_ins()
        {
            string id = User.Identity.GetUserId();
            List<Student_Course_Instructor> st_cr_in = db.Student_Course_Instructor.ToList();
            List<Department_Instructor_Course> d_i_c = db.Department_Instructor_Course.ToList();

            var std_crs = (from c in st_cr_in where c.Std_Id == id select c.Crs_Id).ToList();//course that are taks by std 

            List<Course> crs = db.Courses.ToList();
            List<Course> std_crs_obj = new List<Course>();

            for (int i = 0; i < std_crs.Count(); i++)
            {
                if (crs[i].Crs_Id == std_crs[i])
                {
                    std_crs_obj.Add(crs[i]);
                }
            }

            var all_Finneshed_crs = (from c in d_i_c where c.Crs_Status == "completed" select c.Crs_Id).ToList();
            List<Course> std_fin_crs = new List<Course>();

            for (var i = 0; i < std_crs.Count(); i++)
            {
                for (var n = 0; n < all_Finneshed_crs.Count(); n++)
                {
                    if (std_crs_obj[i].Crs_Id == all_Finneshed_crs[n])
                    {
                        std_fin_crs.Add(std_crs_obj[i]);
                    }
                }
            }

            ViewBag.stdCrs = new SelectList(std_fin_crs, "Crs_Id", "Crs_name");
            ViewBag.flag = 0;
            return View();
        }
        [Authorize(Roles = "Student")]
        public ActionResult insInCrs(int course)
        {
            Course cors = db.Courses.FirstOrDefault(a => a.Crs_Id == course);

            string id = User.Identity.GetUserId();

            List<Instructor> insts = cors.Ins_List;  //ins how tech a course
            List<int> evaluations = new List<int>() { 1, 2, 3, 4, 5 };
            ViewBag.evaluations = new SelectList(evaluations);
            ViewBag.flag = 1;

            //find finished courses with ins,std
            List<Student_Course_Instructor> st_cr_in = db.Student_Course_Instructor.ToList();
            List<Department_Instructor_Course> d_i_c = db.Department_Instructor_Course.ToList();

            var std_crs = (from c in st_cr_in where c.Std_Id == id select c.Crs_Id).ToList();//students courses

            List<Course> crs = db.Courses.ToList();
            List<Course> std_crs_obj = new List<Course>();

            for (int i = 0; i < std_crs.Count(); i++)
            {
                if (crs[i].Crs_Id == std_crs[i])
                {
                    std_crs_obj.Add(crs[i]);
                }
            }

            var all_Finneshed_crs = (from c in d_i_c where c.Crs_Status == "completed" select c.Crs_Id).ToList();
            List<Course> std_fin_crs = new List<Course>();

            for (var i = 0; i < std_crs.Count(); i++)
            {
                for (var n = 0; n < all_Finneshed_crs.Count(); n++)
                {
                    if (std_crs_obj[i].Crs_Id == all_Finneshed_crs[n])
                    {
                        std_fin_crs.Add(std_crs_obj[i]);
                    }
                }
            }

            ViewBag.stdCrs = new SelectList(std_fin_crs, "Crs_Id", "Crs_name", course);



            return View("eval_ins", insts);
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        public ActionResult eval_ins(string[] ins, int crs_id, int[] eval_grad)
        {
            string std_id = User.Identity.GetUserId();
            for (var i = 0; i < ins.Length; i++)
            {
                string ins_id = ins[i];
                Student_Course_Instructor st_crs_ins = db.Student_Course_Instructor.FirstOrDefault(a => a.Ins_Id == ins_id && a.Crs_Id == crs_id && a.Std_Id == std_id);
                st_crs_ins.Ins_Eval = eval_grad[i];
                db.SaveChanges();
            }
            return RedirectToAction("eval_ins", new { @id = std_id });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Import()
        {
            return PartialView();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelFile)
        {
            if (excelFile == null || excelFile.ContentLength == 0)
            {
                ViewBag.error = "choose excel File";
                return PartialView();
            }
            else
            {
                if (excelFile.FileName.EndsWith("xls") || excelFile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/Content/" + excelFile.FileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelFile.SaveAs(path);

                    //reading excel file
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;


                    for (int i = 2; i < range.Rows.Count + 1; i++)
                    {
                        Student stud = new Student();
                        stud.FirstName = ((Excel.Range)range.Cells[i, 1]).Text;
                        stud.LastName = ((Excel.Range)range.Cells[i, 2]).Text;
                        string m = ((Excel.Range)range.Cells[i, 3]).Text;
                        stud.IsMarried = Convert.ToBoolean(m);
                        stud.BirthDate = Convert.ToDateTime(((Excel.Range)range.Cells[i, 4]).Text);
                        stud.PhoneNumber = ((Excel.Range)range.Cells[i, 5]).Text;
                        stud.Email = ((Excel.Range)range.Cells[i, 6]).Text;
                        stud.Std_Attendence_Grade = int.Parse(((Excel.Range)range.Cells[i, 7]).Text);
                        string x = ((Excel.Range)range.Cells[i, 8]).Text;
                        if (x.Length < 1)
                            stud.Dept_Id = null;
                        else
                            stud.Dept_Id = int.Parse(x);

                        stud.UserName = ((Excel.Range)range.Cells[i, 9]).Text;
                        stud.PasswordHash = ((Excel.Range)range.Cells[i, 10]).Text;

                        db.Students.Add(stud);
                        db.SaveChanges();
                    }
                    var s = db.Students.ToList();
                    ViewBag.studlist = s;
                    return View("Index");
                }
                else
                {
                    ViewBag.error = "Wrong file type";
                    return PartialView();
                }
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Attendence()
        {

            ViewBag.dept_drop = dept_drop;
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult GetDeptStud(int Id)
        {
            var student_list = db.Students.Where(s => s.Dept_Id == Id).ToList();
            TempData["DeptId"] = Id;
            return View(student_list);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult SubmitAttendence(string[] attendence)
        {
            var dept_id = TempData["DeptId"];
            int id = Convert.ToInt32(dept_id);
            var student_list = db.Students.Where(s => s.Dept_Id == id).ToList();
            List<Student> absence_list = new List<Student>();

            foreach (var item in attendence)
            {
                var date = DateTime.Now.Date;

                Student st = student_list.Where(s => s.Id != item).First();
                Attendence att = new Attendence();
                att.Id = st.Id;
                att.Absence_Date = date;
                db.Attendence.Add(att);
                db.SaveChanges();
                absence_list.Add(st);
            }

            foreach (var item in absence_list)
            {
                int perCount = db.Permissions.Where(s => s.Stud_Id == item.Id).ToList().Count;
                int absenceCount = db.Attendence.Where(a => a.Id == item.Id).ToList().Count;

                if (absenceCount == 1 && perCount == 1)
                {

                }
                else if (absenceCount == 1 && perCount == 0)
                {
                    var st = db.Students.Where(s => s.Id == item.Id).First();
                    st.Std_Attendence_Grade = 600 - (25);
                    db.SaveChanges();
                }
                else if (absenceCount > perCount)
                {
                    var st = db.Students.Where(s => s.Id == item.Id).First();

                    var grade = 600 - ((absenceCount - perCount) * 25);

                    if (perCount > 1 && perCount <= 4)
                    {
                        grade = grade - (perCount * 5);
                    }
                    else if (perCount > 4 && perCount <= 7)
                    {
                        grade = grade - (3 * 5);
                        grade = grade - ((perCount - 3) * 10);
                    }
                    else if (perCount > 7)
                    {
                        grade = grade - (3 * 5);
                        grade = grade - ((3) * 10);
                        grade = grade - ((perCount - 7) * 25);

                    }
                    st.Std_Attendence_Grade = grade;
                    db.SaveChanges();


                }

            }

            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult StudentPermissions()
        {
            ViewBag.dept_drop = dept_drop;
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DeptStudent(int Id)
        {
            var student_list = db.Students.Where(s => s.Dept_Id == Id).ToList();
            ViewBag.stud_drop = new SelectList((from i in student_list
                                                select new
                                                {
                                                    Id = i.Id,
                                                    FullName = i.FirstName + " " + i.LastName
                                                }),
        "Id",
        "FullName",
        null);

            return PartialView();

        }

        [Authorize(Roles = "Admin")]
        public ActionResult PostStudentPermissions(string Id, DateTime Perm_Date_from, DateTime Perm_Date_to)
        {
            List<PostStudentPermissions_Anonymous> PermList = new List<PostStudentPermissions_Anonymous>();
            if (Perm_Date_from != null && Perm_Date_to != null)
            {
                var list =
       (from s in db.Students
        join p in db.Permissions on s.Id equals p.Stud_Id
        where (p.Stud_Id == Id && (p.Perm_Date >= Perm_Date_from && p.Perm_Date <= Perm_Date_to))
        select new { s.FirstName, s.LastName, p.Perm_Date }).ToList();
                foreach (var item in list)
                {
                    var obj = new PostStudentPermissions_Anonymous() { FirstName = item.FirstName, LastName = item.LastName, Perm_Date = item.Perm_Date };
                    PermList.Add(obj);
                }

            }
            return View(PermList);
        }


    }
}