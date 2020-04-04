using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Promeet_Booking_System.Models;

namespace Promeet_Booking_System.Controllers
{
    public class RoomsController : Controller
    {
        public
            PromeetEntities5 db = new PromeetEntities5();

        // GET: Rooms
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult ViewRooms()
        {
            string currentid = this.User.Identity.Name;
            int userid = GetId(currentid);
            var rooms = db.Rooms.Where(a => a.UserId == userid);
            return View(rooms);
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
        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

       
        
        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Name", room.UserId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomId,UserId,AC,Projector,CoffeeFilter,WaterBottles,Mic,Speaker,System,Podium,WiFi,WhiteBoard,Price,Location,CapacityOfRoom,Availability,RoomName,IsRoomBooked")] Room room)
        {
            if (ModelState.IsValid)
            {
                room.UserId = GetId(this.User.Identity.Name);
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Name", room.UserId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
