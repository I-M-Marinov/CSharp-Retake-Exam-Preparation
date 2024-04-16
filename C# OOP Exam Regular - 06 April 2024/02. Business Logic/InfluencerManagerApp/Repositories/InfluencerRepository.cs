using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfluencerManagerApp.Models;
using InfluencerManagerApp.Models.Contracts;
using InfluencerManagerApp.Repositories.Contracts;

namespace InfluencerManagerApp.Repositories
{
    public class InfluencerRepository: IRepository<IInfluencer>
    {
        private readonly List<IInfluencer> influencers;

        public InfluencerRepository()
        {
            influencers = new List<IInfluencer>();
        }

        public IReadOnlyCollection<IInfluencer> Models
        {
            get { return influencers.AsReadOnly(); }
        }
        public void AddModel(IInfluencer model)
        {
            influencers.Add(model);
        }

        public bool RemoveModel(IInfluencer model)
        {
            return influencers.Remove(model);
        }

        public IInfluencer FindByName(string name)
        {
            return influencers.FirstOrDefault(influencer => influencer.Username == name);
        }
    }
}
