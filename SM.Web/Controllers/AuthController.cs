using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SM.Common;
using SM.Entity;
using SM.Models;
using SM.Web.Data;
using SM.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }
        #endregion

        /// <summary>
        /// Login for user from this post login method.
        /// loginModel is a viewmodel and used for login form.
        /// object of LoginModel is objLoginModel.
        /// </summary>
        #region Login(POST)
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel objLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var loggedinUser = _schoolManagementContext.Users.Where(x => x.EmailAddress == objLoginModel.EmailAddress).ToList();
                    if (loggedinUser.Count == 1)
                    {
                        if (loggedinUser != null)
                        {
                            HttpContext.Session.SetString("Userlogeddin", "true");
                            var claims = new List<Claim>();
                            claims.Add(new Claim("emailAddress", objLoginModel.EmailAddress));
                            claims.Add(new Claim(ClaimTypes.NameIdentifier, objLoginModel.EmailAddress));
                            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimPrincipal = new ClaimsPrincipal(claimIdentity);
                            await HttpContext.SignInAsync(claimPrincipal);
                            return RedirectToAction("Index", "Users");
                        }
                        else
                        {
                            TempData["Error"] = CommonValidations.RecordNotExistsMsg;
                            return View();
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
        /// object of User is objUser.
        /// </summary>
        #region Register(POST)
        [HttpPost]
        public IActionResult Register(User objUser)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (_schoolManagementContext.Users.Where(x => x.EmailAddress == objUser.EmailAddress).Count() == 0)
                {
                    objUser.Password = Cryptography.Encrypt(objUser.Password.ToString());
                    /// <summary>
                    /// Passing the Password to Encrypt method and the method will return encrypted string and 
                    /// stored in Password variable.  
                    /// </summary>
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
        /// Set session "Userlogeddin" as false when user logged out from their session.
        /// After logged out session will be clear and user will be redirect to Main Dashboard PAge.
        /// </summary>
        #region LogOut
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.SetString("Userlogeddin", false.ToString());
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            return RedirectToAction("Dashboard", "Home");
        }
        #endregion

        /// <summary>
        ///  Set password method will call when user set their password from email-template
        /// </summary>
        #region SetPasswordGet
        [HttpGet]
        public IActionResult SetPassword(SetPassword objSetPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                     
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
        ///  Set password Post method will call when user enter both their password and click to set password.
        /// </summary>
        #region SetPasswordPost

        #endregion
    }
}
