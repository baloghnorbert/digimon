using Backend.Core.Modell.Entities;
using Backend.Core.Modell.Request;
using Flurl.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.API.Controllers
{
    public partial class DigimonController
    {
        private static Digimon GetDigimonById(int Id)
        {
            return _digimons.Find(x => x.Id == Id);
        }

        private static void DeletDigimon(int id)
        {
            Digimon digimon = _digimons.Find(x => x.Id == id);
            int index = _digimons.IndexOf(digimon);
            _digimons.RemoveAt(index);
        }

        private static Digimon AddDigimon(DigimonRequest digimon)
        {
            int id = _digimons.Last().Id + 1;

            Digimon newPlayer = new Digimon(digimon, id);
            _digimons.Add(newPlayer);

            return newPlayer;
        }

        private static void UpdateDigimon(Digimon digimon)
        {
            Digimon toUpdate = _digimons.Find(x => x.Id == digimon.Id);
            int index = _digimons.IndexOf(toUpdate);
            _digimons.RemoveAt(index);
            _digimons.Insert(index, digimon);
        }

        private static async Task<List<Digimon>> Digimons()
        {
            string url = "https://digimon-api.vercel.app/api/digimon";

            List<DigimonRequest> jsonData = await url.GetJsonAsync<List<DigimonRequest>>();

            List<Digimon> digimons = new List<Digimon>();
            Digimon digimon = null;

            foreach ((DigimonRequest value, int index) data in jsonData.Select((DigimonRequest value, int index) => (value, index)))
            {
                digimon = new Digimon(data.value, data.index);
                digimons.Add(digimon);
            }

            return digimons;
        }
    }
}
