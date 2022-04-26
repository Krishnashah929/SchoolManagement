using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SM.Entity;
using SM.Repositories.IRepository;
using SM.Web.Data;
using System;
using System.Linq;

namespace SM.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly SchoolManagementContext _schoolManagementContext;
        private IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;
        public UsersController(SchoolManagementContext schoolManagementContext, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _schoolManagementContext = schoolManagementContext;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        /// <summary>
        /// After successfull login of user they will redirect on Index Page.
        /// Geeting all users with user repository.
        /// </summary>
        #region Index(GET)
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        { 
            var users = _unitOfWork.UserRepository.GetAll();
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
        /// update details of users with user repository.
        /// </summary>
        #region UpdateUserDetailsPost 
        [HttpPost]
        public IActionResult UpdateUserDetailsPost(User updateUser)
        { 
            try
            {
                ModelState.Remove("Password");
                ModelState.Remove("RetypePassword");
                if (ModelState.IsValid)
                {
                    var User = _userRepository.Update(updateUser);
                    if (User != null)
                    {
                        return Ok(Json("true"));
                    }
                }
                return NoContent();
            }
            catch (Exception)
            {
                return View("Error");
            }
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
        /// Delete users with user repository.
        /// </summary>
        #region DeleteUserDetailsPost
        [HttpPost]
        public IActionResult DeleteUserDetailsPost(User deleteUser)
        {
            try
            {
                var User = _userRepository.Delete(deleteUser);
                if (User != null)
                {
                    return Ok(Json("true"));
                }
                return NoContent();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion
    }
}


//All Previous code without repository pattern.
/// <summary>
/// After successfull login of user they will redirect on Index Page.
/// </summary>
//#region Index(GET)
//[AllowAnonymous]
//[HttpGet]
//public IActionResult Index()
//{
//    //int id = (int)HttpContext.Session.GetInt32("userID");
//    //List<User> users = _schoolManagementContext.Users.Where(x => x.UserId == id).ToList();
//    //List<User> users = _schoolManagementContext.Users.Where(x => x.IsActive == true).ToList();
//}
//#endregion
/// <summary>
/// UpdateUserDetails is modal for updating the details of particular user.
/// </summary>
//#region UpdateUserDetailsPost 
//[HttpPost]
//public IActionResult UpdateUserDetailsPost(User updateUser)
//{
//Session is set into Authcontroller for userId in Set password method.
//int id = (int)HttpContext.Session.GetInt32("links");
//if (id != null)
//{
//    User updateDetails = _schoolManagementContext.Users.FirstOrDefault(x => x.UserId == objUserDetail.UserId);
//    updateDetails.FirstName = objUserDetail.FirstName;
//    updateDetails.Lastname = objUserDetail.Lastname;
//    updateDetails.EmailAddress = objUserDetail.EmailAddress;
//    updateDetails.ModifiedDate = DateTime.Now;
//    var result = _schoolManagementContext.Users.Update(updateDetails);
//    _schoolManagementContext.SaveChanges();

//    if (result != null)
//    {
//        return Ok(Json("true"));
//    }
//    return Ok(Json("false"));
//}
//return Index();

//update details of users with user repository.
//}
//#endregion

/// <summary>
/// DeleteUserDetailsPost is modal for deleting the particular user.
/// </summary>
//#region DeleteUserDetailsPost
//[HttpPost]
//public IActionResult DeleteUserDetailsPost(User deleteUser)
//{
//    //int id = (int)HttpContext.Session.GetInt32("links");
//    //if (id != null)
//    //{
//    //    User deleteDetails = _schoolManagementContext.Users.FirstOrDefault(x => x.UserId == objDeleteDetails.UserId);
//    //    deleteDetails.IsDelete = true;
//    //    deleteDetails.IsActive = false;
//    //    var result = _schoolManagementContext.Users.Update(deleteDetails);
//    //    _schoolManagementContext.SaveChanges();
//    //    if (result != null)
//    //    {
//    //        return Ok(Json("true"));
//    //    }
//    //    return Ok(Json("false"));
//    //}
//    //return Index();
//}
//#endregion