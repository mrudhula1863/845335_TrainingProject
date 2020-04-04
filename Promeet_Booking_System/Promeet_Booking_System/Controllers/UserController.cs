using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using Promeet_Booking_System.Models;
using System.Web.Security;
namespace Promeet_Booking_System.Controllers
{
    public class UserController : Controller
    {
        // Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        //Registration POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")] User user)
        {
            bool Status = false;
            string message = "";

            //Model Validation
            if (ModelState.IsValid)
            {
                #region //Email already Exists
                var isExist = IsEmailExist(user.EmailId);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exists");
                    return View(user);
                }
                #endregion  

                #region //Generate activation code
                user.ActivationCode = Guid.NewGuid();
                #endregion


                #region // Password hashing
                user.Password = Crypto.Hash(user.Password);
                user.Confirm_Password = Crypto.Hash(user.Confirm_Password);
                #endregion
                user.IsEmailVerified = false;

                #region //Save data to database
                using (PromeetEntities5 db= new PromeetEntities5())
                {
                    db.Users.Add(user);
                    db.SaveChanges();

                    //Send details to user via email
                    sendverificationEmail(user.EmailId, user.ActivationCode.ToString());
                    message = "Registration successfully completed. Check your email to verify the account. " +user.EmailId;
                    Status = true;
                }
                #endregion
            }
            else
            {
                message = "Invalid Request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
               
                return View(user);
        }
        //Verify Account
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;

            using (PromeetEntities5 db = new PromeetEntities5())
            {
                db.Configuration.ValidateOnSaveEnabled = false;

                var v = db.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    db.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }

            }
            ViewBag.Status = Status;
            return View();
        }

        //Login Action
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //Login Post 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl)
        {
            string message = "";
            using (PromeetEntities5 db = new PromeetEntities5())
            {
                var v = db.Users.Where(a => a.EmailId == login.Email_id).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20;
                        var ticket = new FormsAuthenticationTicket(login.Email_id, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Profile", "UserEntry");
                        }
                    }
                    else
                    {
                        message = "Invalid credential provided";
                    }
                }
                else
                {
                    message = "Invalid credential provided";
                }
            }
            ViewBag.Message = message;
            return View();
        }
        //Logout
        [HttpPost]
        [Authorize]
        
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (PromeetEntities5 db = new PromeetEntities5())
            {
                var v = db.Users.Where(a => a.EmailId == emailID).FirstOrDefault();
                return v != null;
            }
        }
        [NonAction]
        //Send Verification Mail
        public void sendverificationEmail(string emailID, string activationCode,string emailFor="VerifyAccount")
        {
            var verifyUrl = "/User/"+emailFor+"/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("promeetorg@gmail.com", "Welcome To ProMeet");
            var toEmail = new MailAddress(emailID);
            var fromemailPassword = "promeet123";

            string subject = "";
            string body = "";
            if(emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created";

                body = "<br/> <br/> Your account is successfully created. Please click the below link to verify the account" +
                    "<br/> <br/> <a href='" + link + "'>" + link + "</a>";
            }
            else if(emailFor=="ResetPassword")
            {

                subject = "Reset Password";
                body = "Hi<br/><br/> We got request to reset your account password.Please click on the below link to reset your password"+
                    "<br/><br/><a href="+link+">Reset Password </a>";
            }
           

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromemailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }

        //Forgot Password

        public ActionResult ForgotPassword()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            //Verify Email Id


            //Generate Reset Password Link
            //Send Email
            string message = "";
            bool status = false;
            using (PromeetEntities5 dc= new PromeetEntities5())
            {
                var account = dc.Users.Where(a => a.EmailId == EmailID).FirstOrDefault();
                if (account!=null)
                {
                    //Send Mail for Reset Password
                    string resetCode = Guid.NewGuid().ToString();
                    sendverificationEmail(account.EmailId, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;
                    dc.Configuration.ValidateOnSaveEnabled=false;
                    dc.SaveChanges();
                    message = "Reset Password link has been sent your Email Id";



                }
                else
                {
                    message = "Account not Found";
                }

            }
            ViewBag.Message = message;
            return View();

        }

        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //Ridirect to reset password page
            using(PromeetEntities5 dc =new PromeetEntities5())
            {
                var user = dc.Users.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if(user!=null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if(ModelState.IsValid)
            {
                using(PromeetEntities5 dc=new PromeetEntities5())
                {
                    var user = dc.Users.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if(user!=null)
                    {
                        user.Password = Crypto.Hash(model.NewPassword);
                        user.ResetPasswordCode = "";
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        message = "New Password updated successfully";
                    }
                }
            }
            else
            {
                message = "Something Invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }
    }
}