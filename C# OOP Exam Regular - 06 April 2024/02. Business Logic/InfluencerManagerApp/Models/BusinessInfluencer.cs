using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace InfluencerManagerApp.Models
{
    public class BusinessInfluencer: Influencer
    {
        public BusinessInfluencer(string userName, int followers) : base(userName, followers, 3.0)
        {
        }

        public override int CalculateCampaignPrice()
        {
            double result = Followers * EngagementRate * 0.15;
            return (int)Math.Floor(result);
        }
    }
}
