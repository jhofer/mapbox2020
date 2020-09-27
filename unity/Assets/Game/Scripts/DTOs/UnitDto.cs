

namespace Endgame.DTOs
{

    // TODO: move to own file
    public enum UnitType
    {
        Vehicle,
        Soldier,
    }

    public class UnitDtos
    {
        public double health;

        public string id { get; set; }

        public float damage { get; set; }

        public string userId { get; set; }

        public string buildingType { get; set; }

        public LocationDto location { get; set; }


    }
}


