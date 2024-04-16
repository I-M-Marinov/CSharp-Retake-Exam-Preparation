using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfluencerManagerApp.Models
{
    public class BloggerInfluencer : Influencer
    {
        public BloggerInfluencer(string userName, int followers) : base(userName, followers, 2.0)
        {
        }

        public override int CalculateCampaignPrice()
        {
            double result = Followers * 2.0 * 0.2;
            return (int)Math.Floor(result);
        }
    }
}
