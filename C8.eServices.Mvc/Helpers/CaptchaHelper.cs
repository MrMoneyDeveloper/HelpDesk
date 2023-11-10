using System;
using System.Net;

namespace C8.eServices.Mvc.Helpers
{
    public class CaptchaHelper
    {
        public bool ValidateCaptcha(string response)
        {
            var success = false;
            try
            {
                using (var webClient = new WebClient())
                {
                    var validateString = string.Format(
                        "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                       "6Ld1VagUAAAAAAvdMnJTq0mXa_Bn078Su_xzUOIL",
                       response);

                    var recaptchaResult = webClient.DownloadString(validateString);

                    if (recaptchaResult.ToLower().Contains("true"))
                    {
                        success = true;
                    }
                }
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }
    }
}