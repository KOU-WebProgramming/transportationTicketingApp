using System.ComponentModel.DataAnnotations;

namespace Transport.API.Models
{
    public class JourneyViewModel
    {
        [Required]
        public string From { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid price")]
        public decimal Price { get; set; }

        [Required]
        public string BusCompany { get; set; }
    }

}
