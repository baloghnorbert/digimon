using Backend.Core.Modell.Request;

namespace Backend.Core.Modell.Entities
{
    public class Digimon : DigimonRequest
    {
        public int Id{ get; set; }

        public Digimon()
        {
        }

        public Digimon(DigimonRequest digimon, int id)
        {
            Id = id;
            Name = digimon.Name;
            Img = digimon.Img;
            Level = digimon.Level;
        }
    }
}
