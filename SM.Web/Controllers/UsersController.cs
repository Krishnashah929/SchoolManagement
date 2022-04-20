using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SM.Entity;
using SM.Web.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;


using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace SM.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly SchoolManagementContext _schoolManagementContext;
        private readonly IHostingEnvironment _hostingEnvironment;
        public UsersController(SchoolManagementContext schoolManagementContext, IHostingEnvironment hostingEnvironment)
        {
            _schoolManagementContext = schoolManagementContext;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// After successfull login of user they will redirect on Index Page.
        /// </summary>
        #region Index(GET)
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            //int id = (int)HttpContext.Session.GetInt32("userID");
            //List<User> users = _schoolManagementContext.Users.Where(x => x.UserId == id).ToList();
            List<User> users = _schoolManagementContext.Users.Where(x => x.IsActive == true).ToList();
            ViewBag.users = users;
            return View();
        }
        #endregion

        /// <summary>
        /// UpdateUserDetails is modal for get the details of particular user.
        /// </summary>
        #region UpdateUserDetailsGet

        [HttpGet]
        public IActionResult UpdateUserDetailsGet(int id)
        {
            var userDetails = _schoolManagementContext.Users.Where(x => x.UserId == id).FirstOrDefault();
            return PartialView("_UserDetailsPartial", userDetails);
        }
        #endregion

        /// <summary>
        /// UpdateUserDetails is modal for updating the details of particular user.
        /// </summary>
        #region UpdateUserDetailsPost 
        [HttpPost]
        public IActionResult UpdateUserDetailsPost(User objUserDetail)
        {
            int? id = HttpContext.Session.GetInt32("userId");
            if (id != null)
            {
                User updateDetails = _schoolManagementContext.Users.FirstOrDefault(x => x.UserId == objUserDetail.UserId);
                updateDetails.FirstName = objUserDetail.FirstName;
                updateDetails.Lastname = objUserDetail.Lastname;
                updateDetails.EmailAddress = objUserDetail.EmailAddress;
                updateDetails.ModifiedDate = DateTime.Now;
                var result = _schoolManagementContext.Users.Update(updateDetails);
                _schoolManagementContext.SaveChanges();

                if (result != null)
                {
                    return Ok(Json("true"));
                }
                return Ok(Json("false"));
            }
            return Index();
        }
        #endregion

        /// <summary>
        /// DeleteUserDetails is modal for get the details of particular user for delete them.
        /// </summary>
        #region DeleteUserDetailsGet
        [HttpGet]
        public IActionResult DeleteUserDetailsGet(int id)
        {
            var deleteDetails = _schoolManagementContext.Users.Where(x => x.UserId == id).FirstOrDefault();
            return PartialView("_UserDeletePartial", deleteDetails);
        }
        #endregion

        /// <summary>
        /// DeleteUserDetailsPost is modal for deleting the particular user.
        /// </summary>
        #region DeleteUserDetailsPost
        [HttpPost]
        public IActionResult DeleteUserDetailsPost(User objDeleteDetails)
        {
            int? id = HttpContext.Session.GetInt32("userId");
            if (id != null)
            {
                User deleteDetails = _schoolManagementContext.Users.FirstOrDefault(x => x.UserId == objDeleteDetails.UserId);
                deleteDetails.IsDelete = true;
                deleteDetails.IsActive = false;
                var result = _schoolManagementContext.Users.Update(deleteDetails);
                _schoolManagementContext.SaveChanges();
                if (result != null)
                {
                    return Ok(Json("true"));
                }
                return Ok(Json("false"));
            }
            return Index();
        }
        #endregion

        /// <summary>
        /// SendMailGetModel is modal for send mail to particular user.
        /// </summary>
        #region SendMailGetModel
        [HttpGet]
        public IActionResult SendMailGet(int id)
        {
            var userDetails = _schoolManagementContext.Users.Where(x => x.UserId == id).FirstOrDefault();
            return PartialView("_SendMailPartial", userDetails);
        }
        #endregion

        /// <summary>
        /// SendMailPost is method for sned email with set new password template.
        /// </summary>
        #region SendMailPost
        [HttpPost]
        public IActionResult SendMailPost(User objUser)
        {
            var userDetails = _schoolManagementContext.Users.Where(x => x.EmailAddress == objUser.EmailAddress).FirstOrDefault();
            //objUser.FirstName = objUser.FirstName;
            var subject = "Set Password";
            var body = "Hi";
            //var firstname = objUser.FirstName;
            //string docPath = Path.Combine(Environment.ContentRootPath, "App_Data/docs");

            SendEmail(objUser.EmailAddress, body, subject);

            return Index();
        }
        #endregion


        public static class MyServer
        {
            public static string MapPath(string path)
            {
                var contentRootPath = (string)AppDomain.CurrentDomain.GetData("ContentRootPath");
                var webRootPath = (string)AppDomain.CurrentDomain.GetData("WebRootPath");
                return Path.Combine(
                    (string)AppDomain.CurrentDomain.GetData("ContentRootPath"),
                    path);
            }
        }

        /// <summary>
        ///  In this method we are sending main set password template to user. where they can set their new password.
        /// </summary>
      #region SendEmail
        private void SendEmail(string email, string body, string subject)
        {
            using (MailMessage mm = new MailMessage("krishnaa9121@gmail.com", email))
            {
                mm.Subject = subject;
                string webRootPath = _hostingEnvironment.WebRootPath + "/MalTemplates/MailTemplate.html";
                StreamReader reader = new StreamReader(webRootPath);
                string readFile = reader.ReadToEnd();
                string myString = "";
                myString = readFile;

                //when you have to replace the content of html page
                //myString = myString.Replace("@@Name@@", firstname);
                mm.Body = myString.ToString();
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
    }
}


/// <summary>
/// This method will call when user click on send email button.
/// It will sendlink or url of mail. and we will call sendmail method inside this.
/// </summary>
//#region Sendlink
//[HttpPost]
//public IActionResult Sendlink(User objuser)
//{
//    var p = _schoolManagementContext.Users.Where(c => c.EmailAddress == objuser.EmailAddress).ToList();
//    ModelState.Clear();
//    String ResetCode = Guid.NewGuid().ToString();

//    var uriBuilder = new UriBuilder
//    {
//        Scheme = Request.Scheme,
//        Host = Request.Host.Host,
//        Port = Request.Host.Port ?? -1, //bydefault -1
//        Path = $"/Home/SetPassword/{ResetCode}"
//    };
//    var link = uriBuilder.Uri.AbsoluteUri;

//    var getUser = (from s in _schoolManagementContext.Users where s.EmailAddress == objuser.EmailAddress select s).FirstOrDefault();
//    if (getUser != null)
//    {
//        getUser.Password = ResetCode;
//        _schoolManagementContext.SaveChanges();

//        var subject = "Set new Password Request";
//        var body = "Hi " + getUser.FirstName + ", <br/> You recently requested to set new password for your account. Click the link below to set ." +
//         "<br/> <br/><a href='" + link + "'>" + link + "</a> <br/> <br/>" +
//        "If you did not request for set new password please ignore this mail.";

//        SendEmail(getUser.EmailAddress, body, subject);

//        TempData["SuccessMsg"] = "Your Success Message";
//        return RedirectToAction("Index", "Home");
//    }
//    else
//    {
//        TempData["FailMsg"] = "Your Failure Message";
//    }
//    return RedirectToAction("Index", "Home");
//}
//#endregion