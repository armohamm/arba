using ArbaShop.DAL.Entities;
using ArbaShop.DAL.Repos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using wad_arba_00003741_ii.Models;

namespace wad_arba_00003741_ii.Controllers
{
    public class AuthController : Controller
    {
        #region Authentication

        [HttpGet]
        public ActionResult Welcome()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegistrationViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Should match with password");
            }

            if (IsEmailExist(model.Email))
            {
                ModelState.AddModelError("Email", "Email already registered");
                return View(model);
            }

            if (ModelState.IsValid && ValidateCaptcha())
            {
                var user = MapFromModel(model);
                var repo = new UserRepo();
                repo.Create(user);
                SendMessage(user.Email);
                return RedirectToAction("Login");
            }

            return View(model);
        }

        private User MapFromModel(RegistrationViewModel model)
        {
            return new User
            {
                Id = model.Id,
                FirstName = model.FirstName,
                Lastname = model.LastName,
                Email = model.Email,
                Password = model.Password
            };
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = FindUser(model);
            if (user != null)
            {
                var ticket = new FormsAuthenticationTicket(
                    1,
                    //string.Format("{0} {1}", user.FirstName, user.Lastname),
                    string.Format("{0}", user.Email),
                    DateTime.Now,
                    DateTime.Now.Add(FormsAuthentication.Timeout),
                    false,
                    user.Id.ToString(),
                    FormsAuthentication.FormsCookiePath);

                var encryptedTicket = FormsAuthentication.Encrypt(ticket);

                var authCookie = new HttpCookie("UserLoginData")
                {
                    Value = encryptedTicket,
                    Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
                };

                HttpContext.Response.Cookies.Set(authCookie);

                Session["Email"] = model.Email.Trim();

                FormsAuthentication.SetAuthCookie(user.Email, false);

                //LogHelper.log(this);

                return RedirectToAction("Welcome","Auth");
            }

            ModelState.AddModelError("", "Email or password is wrong");

            return View(model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            var httpCookie = HttpContext.Response.Cookies["UserLoginData"];
            if (httpCookie != null)
            {
                httpCookie.Value = string.Empty;
            }

            return RedirectToAction("Login");
        }


        private User FindUser(LoginViewModel model)
        {
            var repo = new UserRepo();
            return repo.GetAll().FirstOrDefault(u => model.Email == u.Email && model.Password == u.Password);
        }

        private bool IsEmailExist(string email)
        {
            var repo = new UserRepo();
            return repo.GetAll().Any(u => u.Email == email);
        }

        public class CaptchaResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }
            [JsonProperty("error-codes")]
            public List<string> ErrorCodes { get; set; }
        }

        private bool ValidateCaptcha()
        {
            var response = Request["g-recaptcha-response"];
            //secret that was generated in key value pair
            string secret = ConfigurationManager.AppSettings["reCAPTCHASecretKey"];
            var client = new WebClient();
            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);
            //when response is false check for the error message
            if (!captchaResponse.Success)
            {
                if (captchaResponse.ErrorCodes.Count <= 0) return false;
                var error = captchaResponse.ErrorCodes[0].ToLower();
                switch (error)
                {
                    case ("missing-input-secret"):
                        ViewBag.message = "The secret parameter is missing.";
                        break;
                    case ("invalid-input-secret"):
                        ViewBag.message = "The secret parameter is invalid or malformed.";
                        break;
                    case ("missing-input-response"):
                        ViewBag.message = "The response parameter is missing. Please, preceed with reCAPTCHA.";
                        break;
                    case ("invalid-input-response"):
                        ViewBag.message = "The response parameter is invalid or malformed.";
                        break;
                    default:
                        ViewBag.message = "Error occured. Please try again";
                        break;
                }
                return false;
            }
            else
            {
                ViewBag.message = "Valid";
            }
            return true;
        }


        private void SendMessage(string email)
        {
            try
            {
                //Code
                MailMessage mail = new MailMessage();
                mail.From = new System.Net.Mail.MailAddress("milano95j@gmail.com");
                mail.Subject = "Arba";
                SmtpClient smtp = new SmtpClient();
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(mail.From.Address, "abdu1995");
                smtp.Host = "smtp.gmail.com";

                //recipient
                mail.To.Add(new MailAddress(email));

                mail.IsBodyHtml = true;
                string st = "Conguratulations! you successfully registered for Arba";

                mail.Body = st;
                smtp.Send(mail);
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
        }

        #endregion

    }
}