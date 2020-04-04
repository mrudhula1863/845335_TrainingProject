using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Promeet_Booking_System.Models
{
 [MetadataType(typeof(BookingMetadata))]
public partial class  Booking
    {
    }
public class BookingMetadata
{
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date Time")]
        public System.DateTime Date_Time { get; set; }

        [Required]
        public int Duration { get; set; }
    }
}