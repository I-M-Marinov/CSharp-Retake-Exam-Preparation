using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfluencerManagerApp.Models.Contracts;
using InfluencerManagerApp.Utilities.Messages;

namespace InfluencerManagerApp.Models
{
    public abstract class Influencer : IInfluencer
    {

        private string userName;
        private int followers;
        private double engagementRate;
        private List<string> participations;
        private int income;

        protected Influencer(string userName, int followers, double engagementRate)
        {
            Username = userName;
            Followers = followers;
            EngagementRate = engagementRate;
            Income = income;
            participations = new List<string>();
        }



        public string Username
        {
            get { return userName; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.UsernameIsRequired);
                }
                userName = value;
            }
        }


        public int Followers
        {
            get { return followers; }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException(ExceptionMessages.FollowersCountNegative);
                }
                followers = value;
            }
        }

       

        public double EngagementRate
        {
            get { return engagementRate; }
            protected set { engagementRate = value; }
        }

        public double Income
        {
            get;
            private set;
        }



        public IReadOnlyCollection<string> Participations
        {
            get { return participations.AsReadOnly(); }
        }


        public void EarnFee(double amount)
        {
            Income += amount;
        }

        public void EnrollCampaign(string brand)
        {
            participations.Add(brand);
        }

        public void EndParticipation(string brand)
        {
            var brandToRemove = participations.FirstOrDefault(brand);
            participations.Remove(brandToRemove);
        }

        public abstract int CalculateCampaignPrice();

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append($"{Username} - Followers: {Followers}, Total Income: {Income}");

            return sb.ToString().Trim();
        }
    }
}
