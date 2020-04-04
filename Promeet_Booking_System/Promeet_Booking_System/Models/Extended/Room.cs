using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Promeet_Booking_System.Models
{
    [MetadataType(typeof(RoomMetadata))]
    public partial class Room
    {

    }

    public class RoomMetadata
    {

        [HiddenInput(DisplayValue = false)]
        [Key]
        public int RoomId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [Display(Name = "Room Name")]
        public string RoomName { get; set; }


        public bool AC { get; set; }
        public bool Projector { get; set; }
        public bool CoffeeFilter { get; set; }
        public bool WaterBottles { get; set; }
        public bool Mic { get; set; }
        public bool Speaker { get; set; }
        public bool System { get; set; }
        public bool Podium { get; set; }
        public bool WiFi { get; set; }
        public bool WhiteBoard { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [Display(Name = "Amount in Rupees")]
        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [Display(Name = "Room Capacity")]
        public int CapacityOfRoom { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public bool Availability { get; set; }

     
    }
}