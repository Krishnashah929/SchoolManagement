using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SM.Entity;
using SM.Repositories.IRepository;
using SM.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace SM.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly SchoolManagementContext _schoolManagementContext;
        private IUnitOfWork _unitOfWork;
        public UsersController(SchoolManagementContext schoolManagementContext, IUnitOfWork unitOfWork)
        {
            _schoolManagementContext = schoolManagementContext;
            _unitOfWork = unitOfWork;
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
            //List<User> users = _schoolManagementContext.Users.Where(x => x.IsActive == true).ToList();

            //Geeting all users with user repository.
            var users = _unitOfWork.UserRepository.GetAll();
            ViewBag.users = users;
            return View();
        }
        #endregion

        /// <summary>
        /// GetUsers with user repository
        /// </summary>
        #region GetUsers with repository
        //public IList<User> GetAll()
        //{
        //    return _schoolManagementContext.Users.ToList();
        //}
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
            //Session is set into Authcontroller for userId in Set password method.
            int id = (int)HttpContext.Session.GetInt32("links");
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
            int id = (int)HttpContext.Session.GetInt32("links");
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
        /// Delete user with the help of repository.
        /// </summary>
        #region Delete with repo
        //public void Delete(User user)
        //{
        //    _schoolManagementContext.Remove(user);
        //}
        #endregion

    }
}
