using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SM.Common;
using SM.Entity;
using SM.Models;
using SM.Web.Data;
using SM.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SM.Web.Controllers
{
    public class AuthController : Controller
    {

        private readonly SchoolManagementContext _schoolManagementContext;
        private readonly IHostingEnvironment _hostingEnvironment;
        public AuthController(SchoolManagementContext schoolManagementContext, IHostingEnvironment hostingEnvironment)
        {
            _schoolManagementContext = schoolManagementContext;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// This action method is for getting login modal.
        /// </summary>
        #region Login
        [AllowAnonymous]
        [HttpGet]
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
                    var userPassword = EncryptionDecryption.Encrypt(objLoginModel.Password.ToString());
                    var loggedinUser = _schoolManagementContext.Users.FirstOrDefault(x => x.EmailAddress == objLoginModel.EmailAddress && x.Password == userPassword);
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
            try
            {
                //Remove coloum name from validation ModelState.Remove 
                ModelState.Remove("Password");
                ModelState.Remove("RetypePassword");
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    if (_schoolManagementContext.Users.Where(x => x.EmailAddress == objUser.EmailAddress).Count() == 0)
                    {

                        HttpContext.Session.SetString("email", objUser.EmailAddress);

                        objUser.Password = string.Empty;
                        objUser.CreatedDate = DateTime.Now;
                        objUser.IsActive = false;

                        _schoolManagementContext.Users.Add(objUser);
                        _schoolManagementContext.SaveChanges();

                        //encrypt the userid for link in url.
                        var userId = EncryptionDecryption.Encrypt(objUser.UserId.ToString());
                        var userDetails = _schoolManagementContext.Users.Where(x => x.EmailAddress == objUser.EmailAddress).ToList();
                        //link generation with userid.
                        var link = "http://localhost:9334/Auth/SetPassword?link=" + userId;

                        string webRootPath = _hostingEnvironment.WebRootPath + "/MalTemplates/SetPasswordTemplate.html";
                        StreamReader reader = new StreamReader(webRootPath);
                        string readFile = reader.ReadToEnd();
                        string myString = string.Empty;
                        myString = readFile;
                        var subject = "Set Password";
                        //when you have to replace the content of html page
                        myString = myString.Replace("@@Name@@", objUser.FirstName);
                        myString = myString.Replace("@@FullName@@", objUser.FirstName + " " + objUser.Lastname);
                        myString = myString.Replace("@@Email@@", objUser.EmailAddress);
                        myString = myString.Replace("@@Link@@", link);
                        var body = myString.ToString();

                        SendEmail(objUser.EmailAddress, body, subject);
                        return RedirectToAction("Login", "Auth");
                    }
                    else
                    {
                        message = CommonValidations.RecordExistsMsg;
                        return Content(message);
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
        ///  In this method we are sending main set password template to user. where they can set their new password.
        /// </summary>
        #region SendEmail
        private void SendEmail(string email, string body, string subject)
        {
            using (MailMessage mm = new MailMessage("krishnaa9121@gmail.com", email))
            {
                mm.Subject = subject;
                mm.Body = body;
                mm.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("krishnaa9121@gmail.com", "Kri$hn@91");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }
            }
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
        public IActionResult SetPassword(string link)
        {
            try
            {

                //int id = Convert.ToInt32(HttpContext.Session.SetString("links", link));
                link = EncryptionDecryption.Decrypt(link.ToString());
                int id = Convert.ToInt32(link);
                HttpContext.Session.SetInt32("links", id);
                var userDetails = _schoolManagementContext.Users.Where(x => x.UserId == id).FirstOrDefault();
                return View(userDetails);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        ///  Set password Post method will call when user enter both their password and click to set password.
        /// </summary>
        #region SetPasswordPost
        [HttpPost]
        public IActionResult SetPassword(User objUser)
        {
            try
            {
                string message = string.Empty;
                ModelState.Remove("FirstName");
                ModelState.Remove("Lastname");
                ModelState.Remove("EmailAddress");
                if (ModelState.IsValid)
                {
                    int id = (int)HttpContext.Session.GetInt32("links");

                    if (id != null)
                    {
                        User updateDetails = _schoolManagementContext.Users.FirstOrDefault(x => x.UserId == id);

                        //Passing the Password to Encrypt method and the method will return encrypted string and 
                        // stored in Password variable.  

                        updateDetails.Password = EncryptionDecryption.Encrypt(objUser.Password.ToString());
                        updateDetails.IsActive = true;
                        updateDetails.ModifiedDate = DateTime.Now;

                        _schoolManagementContext.Users.Update(updateDetails);
                        _schoolManagementContext.SaveChanges();

                        return RedirectToAction("Login", "Auth");
                    }
                    else
                    {
                        message = CommonValidations.RecordExistsMsg;
                        return Content(message);
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
    }
}
