using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SM.Common;
using SM.Entity.Entity;
using SM.Models;
using SM.Web.Data;
using SM.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SM.Web.Controllers
{
    public class AuthController : Controller
    {

        private readonly SchoolManagementContext _schoolManagementContext;
        public AuthController(SchoolManagementContext schoolManagementContext)
        {
            _schoolManagementContext = schoolManagementContext;
        }

        /// <summary>
        /// This action method is for getting login modal.
        /// </summary>
        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        #endregion

        /// <summary>
        /// Login for user from this post login method.
        /// </summary>
        #region Login(POST)
        [HttpPost]
        public IActionResult Login(LoginModel objLoginModel)
        {
            try
            {
                var loggedinUser = _schoolManagementContext.Users.Where(x => x.EmailAddress == objLoginModel.EmailAddress && x.Password == objLoginModel.Password).ToList();
                if (ModelState.IsValid)
                {
                    if (loggedinUser.Count == 1)
                    {
                        User userdetails = _schoolManagementContext.Users.Where(x => x.EmailAddress == objLoginModel.EmailAddress && x.Password == objLoginModel.Password).FirstOrDefault();
                        if (userdetails != null)
                        {
                            HttpContext.Session.SetString("Userlogeddin", "true");
                            HttpContext.Session.SetInt32("userId", userdetails.UserId);
                            return RedirectToAction("Index", "Users");
                        }
                        else
                        {
                            string message = "";
                            message = CommonValidations.RecordNotExistsMsg;
                            return View(message);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
            return View();
        }
        #endregion

        /// <summary>
        ///  This action method is for getting Register modal.
        /// </summary>
        #region Register(GET)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        #endregion

        /// <summary>
        /// Register for user from this post Register method.
        /// </summary>
        #region Register(POST)
        [HttpPost]
        public IActionResult Register(User objUser)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                if (_schoolManagementContext.Users.Where(x => x.EmailAddress == objUser.EmailAddress).Count() == 0)
                {
                    objUser.Password = Cryptography.Encrypt(objUser.Password.ToString());   // Passing the Password to Encrypt method and the method will return encrypted string and stored in Password variable.  
                    objUser.CreatedDate = DateTime.Now;
                    objUser.IsActive = true;

                    _schoolManagementContext.Users.Add(objUser);
                    _schoolManagementContext.SaveChanges();

                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    message = CommonValidations.RecordExistsMsg;
                    return Content(message);
                }
            }
            return View();
        }
        #endregion

        /// <summary>
        /// When user logout from their account.
        /// </summary>
        #region LogOut
        public IActionResult LogOut()
        {
            HttpContext.Session.SetString("Userlogeddin", false.ToString());
            HttpContext.Session.Clear();
            return RedirectToAction("Dashboard", "Home");
        }
        #endregion
    }
}
