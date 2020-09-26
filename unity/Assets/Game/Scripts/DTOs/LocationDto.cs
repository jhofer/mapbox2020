namespace Endgame.DTOs
{
    public class LocationDto
    {
      
        public LocationDto(double longitude1, double latitude1)
        {
            this.longitude = longitude1;
            this.latitude= latitude1;
        }

        public double longitude { get; set; }
        public double latitude { get; set; }
    }
}
//asdf