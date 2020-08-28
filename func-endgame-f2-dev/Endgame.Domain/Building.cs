
using Microsoft.Azure.Cosmos.Spatial;


namespace Endgame.Backend.Domain
{

    public class Building
    {
        public double Value;

        public string Id { get; set; }

        public float Volume { get; set; }

        public string UserId { get; set; }

        public string BuildingType { get; set; }

        public Point Location { get; set; }


    }
}


