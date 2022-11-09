using HomeBooking.API.Models.DTO;
using System.Linq;

namespace HomeBooking.API.Data
{
    public static class HomeDataStore
    {
        public static readonly List<HomeDTO> homeList = new List<HomeDTO>
        {
            new HomeDTO{ Id = 1, Name="Pool"},
            new HomeDTO{ Id = 2, Name="Hill"},
            new HomeDTO{ Id = 3, Name="Beach"}
        };
    }
}
