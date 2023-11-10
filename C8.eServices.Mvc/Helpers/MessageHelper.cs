using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace C8.eServices.Mvc.Helpers
{
    public class MessageHelper
    {
        public static void SetSuccessMessage(Controller controller, string message)
        {
            controller.TempData["SuccessMessage"] = message;
        }

        public static string GetSuccessMessage(Controller controller)
        {
            return controller.TempData["SuccessMessage"] as string;
        }

        public static void SetFailureMessage(Controller controller, string message)
        {
            controller.TempData["FailureMessage"] = message;
        }

        public static string GetFailureMessage(Controller controller)
        {
            return controller.TempData["FailureMessage"] as string;
        }
    }
}