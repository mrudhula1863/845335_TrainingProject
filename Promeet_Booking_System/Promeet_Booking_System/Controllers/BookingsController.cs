using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Promeet_Booking_System.Models;

namespace Promeet_Booking_System.Controllers
{
    public class BookingsController : Controller
    {
        private PromeetEntities5 db = new PromeetEntities5();

        // GET: Bookings
        public ActionResult BookingIndexMain()
        {
            return View();
        }
        public ActionResult Index( string searching)
        {
            return View(db.Rooms.Where(a => a.Location.Contains(searching)).ToList());
        }

        public ActionResult ViewBookings()
           
        {
            string currentid = this.User.Identity.Name;
            int userid = GetId(currentid);
            var rooms = db.Bookings.Where(a => a.UserId == userid).Include(b=>b.User);

           // var bookings = db.Bookings.Include(b => b.Room).Include(b => b.User);
   
                /* var s = db.Rooms.Where(a => a.RoomId == booking.RoomId).Select(a => a.UserId).FirstOrDefault();
                 var list = db.Users.Where(a => a.UserId == s);*/
                return View(rooms.ToList());
            

        }



        // GET: Bookings/Create
        [HttpGet]
        public ActionResult Create()
        {

        
            return View();
        }

       

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomId,UserId,Payment_Money,Location,Date_Time,Duration")]Booking booking,int roomid)
        {
            string message = "";
            bool Status = false;
            booking.RoomId = roomid;
            booking.UserId = GetId(this.User.Identity.Name);
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                
                sendconfirmationEmail(this.User.Identity.Name);
                message = "Booking Successful. Check your email to confirm booking. " + this.User.Identity.Name;
                Status = true;
                
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;

            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomId", booking.RoomId);

            return View(booking);
        }

        

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("ViewBookings");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [NonAction]
        public int GetId(string emailID)
        {
            using (PromeetEntities5 db = new PromeetEntities5())
            {
                var s = db.Users.Where(a => a.EmailId == emailID).Select(a => a.UserId).FirstOrDefault();

                return s;
            }
        }
        public void sendconfirmationEmail(string emailID)
        {
          

            var fromEmail = new MailAddress("promeetorg@gmail.com", "Booking Confirmation");
            var toEmail = new MailAddress(emailID);
            var fromemailPassword = "promeet123";

           string  subject = "Booked room Succeefully";

            string body = "<br/> <br/> Your have  successfully booked the room. <br/><br/>Thank you for using Promeet";


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

    }
}
