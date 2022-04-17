using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SM.Entity.Entity;
using SM.Web.Data;
using SM.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SM.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly SchoolManagementContext _schoolManagementContext;
        public HomeController(SchoolManagementContext schoolManagementContext)
        {
            _schoolManagementContext = schoolManagementContext;
        }

        /// <summary>
        /// Main Dashboard when user is not logged in.
        /// </summary>
        #region Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
