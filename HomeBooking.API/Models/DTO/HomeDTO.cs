using System.ComponentModel.DataAnnotations;

namespace HomeBooking.API.Models.DTO
{
    public class HomeDTO{
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
