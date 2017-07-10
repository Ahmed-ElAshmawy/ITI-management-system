using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace ITI_System.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public bool IsMarried { get; set; }
        
        public DateTime BirthDate { get; set; }//all users(st,ins) have these att
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Student> Students { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<Answers> Answers { get; set; }

        public DbSet<Department_Instructor_Course> Department_Instructor_Course { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Student_Course_Instructor> Student_Course_Instructor { get; set; }

        public DbSet<StudentExam> StudentExam { get; set; }


        public DbSet<Instructor> Instructor { get; set; }

		public DbSet<Attendence> Attendence { get; set; }



        public ApplicationDbContext()
            : base("ITI_System", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
         {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<ITI_System.Models.StudentViewModel> StudentViewModels { get; set; }

        // public System.Data.Entity.DbSet<ITI_System.Models.StudentViewModel> StudentViewModels { get; set; }
    }
}