using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Handball.Models.Contracts;
using Handball.Repositories.Contracts;

namespace Handball.Repositories
{
    internal class PlayerRepository : IRepository<IPlayer>
    {

        private List<IPlayer> players;

        public PlayerRepository()
        {
            players = new List<IPlayer>();
        }


        public IReadOnlyCollection<IPlayer> Models
        {
            get { return players.AsReadOnly(); }
        }

        public void AddModel(IPlayer model)
        {
            players.Add(model);
        }

        public bool ExistsModel(string name)
        {
            return players.Any(p => p.Name == name);
        }

        public IPlayer GetModel(string name)
        {
            return players.FirstOrDefault(p => p.Name == name);
        }

        public bool RemoveModel(string name)
        {
            IPlayer team = GetModel(name);

            return players.Remove(team);
        }
    }
}
