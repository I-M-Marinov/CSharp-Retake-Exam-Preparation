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
        private readonly List<string> contributors;


        protected Campaign(string brand, double budget)
        {
            Brand = brand;
            Budget = budget;
            contributors = new List<string>();
        }


        public string Brand
        {
            get { return brand; }
            private set
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
            private set
            {
                budget = value;
            }
        }

        public IReadOnlyCollection<string> Contributors =>  contributors;


        public void Gain(double amount)
        {
            budget += amount;
        }

        public void Engage(IInfluencer influencer)
        {
            double campaignPrice = influencer.CalculateCampaignPrice();
            budget -= campaignPrice;
            contributors.Add(influencer.Username);
        }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append($"{typeof(Campaign).Name} - Brand: {Brand}, Budget: {Budget}, Contributors: {Contributors.Count}");

            return sb.ToString().Trim();
        }
    }
}
