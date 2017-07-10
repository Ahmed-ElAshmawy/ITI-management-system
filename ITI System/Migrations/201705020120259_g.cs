namespace ITI_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class g : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Question_Id = c.Int(nullable: false),
                        Ans = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Question_Id, t.Ans })
                .ForeignKey("dbo.Question", t => t.Question_Id, cascadeDelete: true)
                .Index(t => t.Question_Id);
            
            CreateTable(
                "dbo.Question",
                c => new
                    {
                        Question_Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        Body = c.String(nullable: false),
                        Correct_Answer = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Question_Id);
            
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        Exam_Id = c.Int(nullable: false, identity: true),
                        Exam_Start_Time = c.DateTime(nullable: false),
                        Exam_End_Time = c.DateTime(nullable: false),
                        Crs_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Exam_Id)
                .ForeignKey("dbo.Course", t => t.Crs_Id)
                .Index(t => t.Crs_Id);
            
            CreateTable(
                "dbo.Course",
                c => new
                    {
                        Crs_Id = c.Int(nullable: false, identity: true),
                        Crs_name = c.String(nullable: false),
                        Crs_LabDuration = c.Int(nullable: false),
                        Crs_LectDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Crs_Id);
            
            CreateTable(
                "dbo.Department_Instructor_Course",
                c => new
                    {
                        Dept_Id = c.Int(nullable: false),
                        Ins_Id = c.String(nullable: false, maxLength: 128),
                        Crs_Id = c.Int(nullable: false),
                        Crs_Status = c.String(),
                    })
                .PrimaryKey(t => new { t.Dept_Id, t.Ins_Id, t.Crs_Id })
                .ForeignKey("dbo.Course", t => t.Crs_Id, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.Dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.Instructor", t => t.Ins_Id)
                .Index(t => t.Dept_Id)
                .Index(t => t.Ins_Id)
                .Index(t => t.Crs_Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Dept_Id = c.Int(nullable: false, identity: true),
                        Dept_name = c.String(nullable: false),
                        Dept_Capacity = c.Int(nullable: false),
                        Manager_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Dept_Id)
                .ForeignKey("dbo.Instructor", t => t.Manager_Id)
                .Index(t => t.Manager_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        IsMarried = c.Boolean(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Student_Course_Instructor",
                c => new
                    {
                        Std_Id = c.String(nullable: false, maxLength: 128),
                        Ins_Id = c.String(nullable: false, maxLength: 128),
                        Crs_Id = c.Int(nullable: false),
                        Ins_Eval = c.Int(nullable: false),
                        Std_Lab_Eval = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Std_Id, t.Ins_Id, t.Crs_Id })
                .ForeignKey("dbo.Course", t => t.Crs_Id, cascadeDelete: true)
                .ForeignKey("dbo.Instructor", t => t.Ins_Id)
                .ForeignKey("dbo.Students", t => t.Std_Id)
                .Index(t => t.Std_Id)
                .Index(t => t.Ins_Id)
                .Index(t => t.Crs_Id);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Perm_Id = c.Int(nullable: false, identity: true),
                        Perm_Date = c.DateTime(nullable: false),
                        Stud_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Perm_Id)
                .ForeignKey("dbo.Students", t => t.Stud_Id)
                .Index(t => t.Stud_Id);
            
            CreateTable(
                "dbo.StudentExams",
                c => new
                    {
                        Std_Id = c.String(nullable: false, maxLength: 128),
                        Exam_Id = c.Int(nullable: false),
                        Std_Grade = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Std_Id, t.Exam_Id })
                .ForeignKey("dbo.Exams", t => t.Exam_Id, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Std_Id)
                .Index(t => t.Std_Id)
                .Index(t => t.Exam_Id);
            
            CreateTable(
                "dbo.Attendence",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Absence_Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.Absence_Date })
                .ForeignKey("dbo.Students", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.StudentViewModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        IsMarried = c.Boolean(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        PhoneNumber = c.String(),
                        Email = c.String(nullable: false),
                        Dept_Id = c.Int(),
                        Password = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.Dept_Id)
                .Index(t => t.Dept_Id);
            
            CreateTable(
                "dbo.DepartmentCourses",
                c => new
                    {
                        Department_Dept_Id = c.Int(nullable: false),
                        Course_Crs_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Dept_Id, t.Course_Crs_Id })
                .ForeignKey("dbo.Departments", t => t.Department_Dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.Course", t => t.Course_Crs_Id, cascadeDelete: true)
                .Index(t => t.Department_Dept_Id)
                .Index(t => t.Course_Crs_Id);
            
            CreateTable(
                "dbo.InstructorCourses",
                c => new
                    {
                        Instructor_Id = c.String(nullable: false, maxLength: 128),
                        Course_Crs_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Instructor_Id, t.Course_Crs_Id })
                .ForeignKey("dbo.Instructor", t => t.Instructor_Id, cascadeDelete: true)
                .ForeignKey("dbo.Course", t => t.Course_Crs_Id, cascadeDelete: true)
                .Index(t => t.Instructor_Id)
                .Index(t => t.Course_Crs_Id);
            
            CreateTable(
                "dbo.ExamQuestions",
                c => new
                    {
                        Exam_Exam_Id = c.Int(nullable: false),
                        Question_Question_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Exam_Exam_Id, t.Question_Question_Id })
                .ForeignKey("dbo.Exams", t => t.Exam_Exam_Id, cascadeDelete: true)
                .ForeignKey("dbo.Question", t => t.Question_Question_Id, cascadeDelete: true)
                .Index(t => t.Exam_Exam_Id)
                .Index(t => t.Question_Question_Id);
            
            CreateTable(
                "dbo.Instructor",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Ins_Qualifications = c.String(),
                        Ins_Status = c.String(),
                        Ins_GraduationYear = c.DateTime(nullable: false),
                        Dept_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Departments", t => t.Dept_Id)
                .Index(t => t.Id)
                .Index(t => t.Dept_Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Std_Attendence_Grade = c.Int(),
                        Dept_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Departments", t => t.Dept_Id)
                .Index(t => t.Id)
                .Index(t => t.Dept_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "Dept_Id", "dbo.Departments");
            DropForeignKey("dbo.Students", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Instructor", "Dept_Id", "dbo.Departments");
            DropForeignKey("dbo.Instructor", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentViewModels", "Dept_Id", "dbo.Departments");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Attendence", "Id", "dbo.Students");
            DropForeignKey("dbo.Answers", "Question_Id", "dbo.Question");
            DropForeignKey("dbo.ExamQuestions", "Question_Question_Id", "dbo.Question");
            DropForeignKey("dbo.ExamQuestions", "Exam_Exam_Id", "dbo.Exams");
            DropForeignKey("dbo.Exams", "Crs_Id", "dbo.Course");
            DropForeignKey("dbo.Department_Instructor_Course", "Ins_Id", "dbo.Instructor");
            DropForeignKey("dbo.Department_Instructor_Course", "Dept_Id", "dbo.Departments");
            DropForeignKey("dbo.Departments", "Manager_Id", "dbo.Instructor");
            DropForeignKey("dbo.Student_Course_Instructor", "Std_Id", "dbo.Students");
            DropForeignKey("dbo.StudentExams", "Std_Id", "dbo.Students");
            DropForeignKey("dbo.StudentExams", "Exam_Id", "dbo.Exams");
            DropForeignKey("dbo.Permissions", "Stud_Id", "dbo.Students");
            DropForeignKey("dbo.Student_Course_Instructor", "Ins_Id", "dbo.Instructor");
            DropForeignKey("dbo.Student_Course_Instructor", "Crs_Id", "dbo.Course");
            DropForeignKey("dbo.InstructorCourses", "Course_Crs_Id", "dbo.Course");
            DropForeignKey("dbo.InstructorCourses", "Instructor_Id", "dbo.Instructor");
            DropForeignKey("dbo.DepartmentCourses", "Course_Crs_Id", "dbo.Course");
            DropForeignKey("dbo.DepartmentCourses", "Department_Dept_Id", "dbo.Departments");
            DropForeignKey("dbo.Department_Instructor_Course", "Crs_Id", "dbo.Course");
            DropIndex("dbo.Students", new[] { "Dept_Id" });
            DropIndex("dbo.Students", new[] { "Id" });
            DropIndex("dbo.Instructor", new[] { "Dept_Id" });
            DropIndex("dbo.Instructor", new[] { "Id" });
            DropIndex("dbo.ExamQuestions", new[] { "Question_Question_Id" });
            DropIndex("dbo.ExamQuestions", new[] { "Exam_Exam_Id" });
            DropIndex("dbo.InstructorCourses", new[] { "Course_Crs_Id" });
            DropIndex("dbo.InstructorCourses", new[] { "Instructor_Id" });
            DropIndex("dbo.DepartmentCourses", new[] { "Course_Crs_Id" });
            DropIndex("dbo.DepartmentCourses", new[] { "Department_Dept_Id" });
            DropIndex("dbo.StudentViewModels", new[] { "Dept_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Attendence", new[] { "Id" });
            DropIndex("dbo.StudentExams", new[] { "Exam_Id" });
            DropIndex("dbo.StudentExams", new[] { "Std_Id" });
            DropIndex("dbo.Permissions", new[] { "Stud_Id" });
            DropIndex("dbo.Student_Course_Instructor", new[] { "Crs_Id" });
            DropIndex("dbo.Student_Course_Instructor", new[] { "Ins_Id" });
            DropIndex("dbo.Student_Course_Instructor", new[] { "Std_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Departments", new[] { "Manager_Id" });
            DropIndex("dbo.Department_Instructor_Course", new[] { "Crs_Id" });
            DropIndex("dbo.Department_Instructor_Course", new[] { "Ins_Id" });
            DropIndex("dbo.Department_Instructor_Course", new[] { "Dept_Id" });
            DropIndex("dbo.Exams", new[] { "Crs_Id" });
            DropIndex("dbo.Answers", new[] { "Question_Id" });
            DropTable("dbo.Students");
            DropTable("dbo.Instructor");
            DropTable("dbo.ExamQuestions");
            DropTable("dbo.InstructorCourses");
            DropTable("dbo.DepartmentCourses");
            DropTable("dbo.StudentViewModels");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Attendence");
            DropTable("dbo.StudentExams");
            DropTable("dbo.Permissions");
            DropTable("dbo.Student_Course_Instructor");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Departments");
            DropTable("dbo.Department_Instructor_Course");
            DropTable("dbo.Course");
            DropTable("dbo.Exams");
            DropTable("dbo.Question");
            DropTable("dbo.Answers");
        }
    }
}
