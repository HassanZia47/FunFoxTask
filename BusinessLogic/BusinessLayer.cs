using FunFoxTask.Data;
using FunFoxTask.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FunFoxTask.BusinessLogic
{
    public class BusinessLayer
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public BusinessLayer(AppDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        #region Admin user creation
        public void CreateAdminUserIfNotExist()
        {
            try
            {
                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    var result = _roleManager.CreateAsync(new IdentityRole { Name = "admin" }).GetAwaiter().GetResult();

                    if (result.Succeeded)
                    {
                        var user = new AppUser
                        {
                            Firstname = "admin",
                            Lastname = "admin",
                            PhoneNumber = "00112233",
                            UserName = "portaladmin@gmail.com",
                            Email = "portaladmin@gmail.com"
                        };
                        string userPWD = "12345678";

                        var chkUser = _userManager.CreateAsync(user, userPWD).GetAwaiter().GetResult();

                        if (chkUser.Succeeded)
                        {
                            var result1 = _userManager.AddToRoleAsync(user, "admin").GetAwaiter().GetResult();
                            if (!result1.Succeeded)
                                throw new Exception();
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Admin user creation

        #region Admin

        public List<CourseVM> GetAvailableCoursesToEnroll(System.Security.Claims.ClaimsPrincipal user)
        {
            try
            {
                List<CourseVM> coursesList = new List<CourseVM>();
                if (_context.Courses != null
                    && _context.Courses.Count() > 0)
                {
                    string? userId = _userManager.GetUserId(user);

                    if (!string.IsNullOrEmpty(userId))
                    {
                        var data = _context.Courses.ToList();

                        foreach (var item in data)
                        {
                            int crntStrength = _context.AppUserToCourse.Where(x => x.CourseId == item.Id).Count();

                            CourseVM courseModel = new CourseVM
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Description = item.Description,
                                Levels = item.Levels,
                                MaxStrengh = item.MaxStrengh,
                                Timings = item.Timings,
                                CurrentStrengh = crntStrength
                            };

                            if (_context.AppUserToCourse.Any(x => x.AppUserId == userId && x.CourseId == item.Id))
                            {
                                courseModel.TextToShow = "Enrolled !!";
                                courseModel.EnrollFlag = false;
                            }
                            else if (item.MaxStrengh == crntStrength)
                            {
                                courseModel.TextToShow = "Max Strength Reached !!";
                                courseModel.EnrollFlag = false;
                            }
                            else if (item.MaxStrengh > crntStrength)
                            {
                                courseModel.EnrollFlag = true;
                            }
                            coursesList.Add(courseModel);
                        }
                    }
                }

                return coursesList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EnrollInCourse(System.Security.Claims.ClaimsPrincipal user, int? id)
        {
            try
            {
                if (id.HasValue && user != null)
                {
                    string? userId = _userManager.GetUserId(user);
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var data = _context.AppUserToCourse.FirstOrDefault(x => x.CourseId == id.Value && x.AppUserId == userId);

                        if (data == null)
                        {
                            var v = _context.AppUserToCourse.Add(new AppUserToCourse
                            {
                                AppUserId = userId,
                                CourseId = id.Value
                            });

                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveEnrollment(System.Security.Claims.ClaimsPrincipal user, int? id)
        {
            try
            {
                if (id.HasValue && user != null)
                {
                    string? userId = _userManager.GetUserId(user);
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var data = _context.AppUserToCourse.FirstOrDefault(x => x.CourseId == id.Value && x.AppUserId == userId);

                        if (data != null)
                        {
                            var v = _context.AppUserToCourse.Remove(data);

                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Course> GetAllCoursesList()
        {
            try
            {
                return _context.Courses.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Course? GetCourseDetail(int? id)
        {
            try
            {
                Course? course = null;
                if (id.HasValue)
                    course = _context.Courses.FirstOrDefault(m => m.Id == id);
                return course;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddCourse(Course course)
        {
            try
            {
                _context.Add(course);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Course? GetEditCourse(int? id)
        {
            try
            {
                Course? course = null;
                if (id.HasValue)
                    course = _context.Courses.Find(id);
                return course;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void PostEditCourse(int id, [Bind("Id,Name,Description,Levels,Timings,MaxStrengh")] Course course)
        {
            try
            {
                _context.Update(course);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Course? GetDelete(int? id)
        {
            try
            {
                Course? course = null;
                if (id.HasValue)
                    course = _context.Courses.FirstOrDefault(m => m.Id == id);

                return course;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteConfirmed(int id)
        {
            try
            {
                var course = _context.Courses.Find(id);
                if (course != null)
                    _context.Courses.Remove(course);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Admin

        #region Enrollment

        public EnrolledUserVM? GetEnrolledUsers(int? id)
        {
            try
            {
                EnrolledUserVM? enrolledUserVM = null;

                if (id.HasValue)
                {
                    enrolledUserVM = new EnrolledUserVM()
                    {
                        Users = new List<AppUser>(),
                        Course = new Course()
                    };
                    var course = _context.Courses.FirstOrDefault(m => m.Id == id.Value);

                    if (course != null)
                    {
                        enrolledUserVM.Course.Id = id.Value;
                        enrolledUserVM.Course.Name = course.Name;

                        var userList = _context.AppUserToCourse.Where(m => m.CourseId == id.Value).ToList();

                        foreach (var item in userList)
                        {
                            var user = _context.AppUsers.FirstOrDefault(x => x.Id == item.AppUserId);
                            if (user != null)
                            {
                                enrolledUserVM.Users.Add(user);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }

                return enrolledUserVM;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void EnrolledUserDelete(string userId, int courseId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    var data = _context.AppUserToCourse.FirstOrDefault(x => x.CourseId == courseId && x.AppUserId == userId);

                    if (data != null)
                    {
                        var v = _context.AppUserToCourse.Remove(data);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AppUser? GetEnrolledUserDetail(string userId)
        {
            AppUser? user = null;
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    user = _context.AppUsers.FirstOrDefault(x => x.Id == userId);
                }
                else
                {
                    throw new Exception();
                }
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Enrollment
    }
}