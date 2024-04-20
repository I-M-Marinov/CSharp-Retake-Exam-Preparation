using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using InfluencerManagerApp.Models.Contracts;
using InfluencerManagerApp.Utilities.Messages;

namespace InfluencerManagerApp.Models
{
    public abstract class Campaign : ICampaign
    {
        private string brand;
        private double budget;

        protected Campaign(string brand, double budget)
        {
            Brand = brand;
            Budget = budget;
            contributors = new List<string>();
        }


        public string Brand
        {
            get { return brand; }
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.BrandIsrequired);
                }
                brand = value;
            }
        }

        public double Budget
        {
            get => budget;
            private set => budget = value;
        }

        private List<string> contributors;
        public IReadOnlyCollection<string> Contributors =>  contributors.AsReadOnly();


        public void Gain(double amount)
        {
            Budget += amount;
        }

        public void Engage(IInfluencer influencer)
        {
            double campaignPrice = influencer.CalculateCampaignPrice();
            Budget -= campaignPrice;
            contributors.Add(influencer.Username);
        }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append($"{typeof(Campaign).Name} - Brand: {Brand}, Budget: {Budget}, Contributors: {contributors.Count}");

            return sb.ToString().Trim();
        }
    }
}
