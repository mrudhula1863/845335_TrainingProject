using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Promeet_Booking_System.Models;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Data.Entity;

namespace Promeet_Booking_System.Controllers
{
    [Authorize]
    public class UserEntryController : Controller
    {




        // GET: UserEntry
        [HttpGet]
        [Authorize]
        public ActionResult Profile()
        {

            return View();
        }
        
        [HttpGet]
        public ActionResult AddRoom()
        {
       
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRoom([Bind(Exclude = "IsRoomBooked")]Room room)
        {
            bool Status = false;
           string message = "";
            
            //Model Validation
            if (ModelState.IsValid)
            {

                room.UserId = GetId(this.User.Identity.Name);
                
                #region //RoomName already Exists

                var isExist = IsRoomExist(room.RoomName,room.UserId);
                  if (isExist)
                  {
                      ModelState.AddModelError("RoomExist", "Room Already Added");
                      return View(room);
                  }
                #endregion

                #region //Save data to database
                using (PromeetEntities5 db = new PromeetEntities5())
                {

                    db.Rooms.Add(room);
                    db.SaveChanges();

                    message = "Room added successfully ";
                    Status = true;
                }
              return RedirectToAction("Index", "Rooms");
                #endregion
            }
            else
            {
                message = "Invalid Request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;

            return View();
        }
        
           
         

        [NonAction]
    public bool IsRoomExist(string roomname,int userid)
        {
            using (PromeetEntities5 db = new PromeetEntities5())
            {
               
                var v = db.Rooms.Where(a => a.RoomName == roomname && a.UserId == userid).FirstOrDefault();
              
                return v != null;
            }
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
    }
    
}