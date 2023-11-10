using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using C8.eServices.Mvc.ApiServices;
using C8.eServices.Mvc.DataAccessLayer;
using C8.eServices.Mvc.Helpers;
using C8.eServices.Mvc.Keys;
using C8.eServices.Mvc.Models;

namespace C8.eServices.Mvc.Controllers
{
    public class HomeController : Controller
    {

        #region Home Index
        public ActionResult Index()
        
        
        {
            try
            {
                switch (Request.IsAuthenticated)
                {
                    case true:
                        return View();
                    default:
                        return View();
                }
            }
            catch (Exception ex)
            {
                return View("_Error404");
                throw ex;
            }
        }
        #endregion
        #region Dashboard redirect
        public ActionResult DashboardRedirect()
        {
            if (User.IsInRole(""))
                return RedirectToAction("Index");
            if (User.IsInRole(""))
                return RedirectToAction("Index");
            if (User.IsInRole(""))
                return RedirectToAction("Index");
            if (User.IsInRole(""))
                return RedirectToAction("Index");
            if (User.IsInRole(""))
                return RedirectToAction("Index");
            if (User.IsInRole(""))
                return RedirectToAction("Index");
            if (User.IsInRole(""))
                return RedirectToAction("Index");
            else 
                return View("Home");
        }
        #endregion

        #region FAQ Index
        public ActionResult FAQ()
        {
            return View();
        }
        #endregion


        #region Home Index2
        public ActionResult Index2()
        {
            ViewBag.Status = "Email address activated";
            return View("Index");
        }

   
        #endregion

        #region Intro Index
        public ActionResult Introduction()
        {
            return View();
        }
        #endregion


        #region RightsandUsage Index
        public ActionResult RightsandUsage()
        {
            return View();
        }
        #endregion

        #region UsernamePassword Index
        public ActionResult UsernamePassword()
        {
            return View();
        }
        #endregion


        #region InforandPrivacy Index
        public ActionResult InfoandPrivacy()
        {
            return View();
        }
        #endregion

        #region Unavailability Index
        public ActionResult Unavailability()
        {
            return View();
        }
        #endregion
        #region Comming Soon
        public ActionResult UnderConstructionComingSoon()
        {
            return View();
        }
        #endregion

    }
}
