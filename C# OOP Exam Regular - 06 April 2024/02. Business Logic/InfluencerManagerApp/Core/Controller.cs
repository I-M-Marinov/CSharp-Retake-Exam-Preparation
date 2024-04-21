using InfluencerManagerApp.Core.Contracts;
using InfluencerManagerApp.Models.Contracts;
using InfluencerManagerApp.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfluencerManagerApp.Repositories;
using InfluencerManagerApp.Models;
using InfluencerManagerApp.Utilities.Messages;
using System.Xml.Linq;

namespace InfluencerManagerApp.Core
{
    public class Controller: IController
    {

        private readonly IRepository<IInfluencer> influencers;
        private readonly IRepository<ICampaign> campaigns;
        private readonly string[] validCampaignTypes = {"ProductCampaign", "ServiceCampaign"};


        public Controller()
        {
            influencers = new InfluencerRepository();
            campaigns = new CampaignRepository();
        }
        public string RegisterInfluencer(string typeName, string username, int followers)
        {
            var influencerType = GetInfluencerType(typeName);

            if (influencerType == null)
            {
                return string.Format(OutputMessages.InfluencerInvalidType, typeName);
            }

            if (influencers.FindByName(username) != null)
            {
                return string.Format(OutputMessages.UsernameIsRegistered, username, typeof(InfluencerRepository).Name);
            }

            IInfluencer influencer;

            if (typeName == typeof(BusinessInfluencer).Name)
            {
                influencer = new BusinessInfluencer(username, followers);
            }
            else if (typeName == typeof(FashionInfluencer).Name)
            {
                influencer = new FashionInfluencer(username, followers);
            }
            else // (typeName == typeof(BloggerInfluencer).Name)
            {
                influencer = new BloggerInfluencer(username, followers);
            }

            influencers.AddModel(influencer);

            return string.Format(OutputMessages.InfluencerRegisteredSuccessfully, username);
        }

        public string BeginCampaign(string typeName, string brand)
        {
            if (!validCampaignTypes.Contains(typeName))
            {
                return $"{typeName} is not a valid campaign in the application.";
            }

            if (campaigns.FindByName(brand) != null)
            {
                return $"{brand} campaign cannot be duplicated.";
            }

            ICampaign campaign = null;

            if (typeName == "ProductCampaign")
            {
                campaign = new ProductCampaign(brand);
            }
            else // (typeName == "ServiceCampaign") 
            {
                campaign = new ServiceCampaign(brand);
            }

            campaigns.AddModel(campaign);

            return $"{brand} started a {typeName}.";
        }

        public string AttractInfluencer(string brand, string username)
        {
            var influencer = influencers.FindByName(username);
            var campaign = campaigns.FindByName(brand);

            if (influencer == null)
            {
                return $"{typeof(InfluencerRepository).Name} has no {username} registered in the application.";
            }

            if (campaign == null)
            {
                return $"There is no campaign from {brand} in the application.";
            }

            if (campaign.Contributors.Contains(username))
            {
                return $"{username} is already engaged for the {brand} campaign.";
            }

            if ((campaign is ProductCampaign && influencer is BloggerInfluencer) ||
                (campaign is ServiceCampaign && influencer is FashionInfluencer))
            {
                return $"{username} is not eligible for the {brand} campaign.";
            }

            int campaignPrice = influencer.CalculateCampaignPrice();

            if (campaign.Budget < campaignPrice)
            {
                return $"The budget for {brand} is insufficient to engage {username}.";
            }

            influencer.EarnFee(campaignPrice);
            influencer.EnrollCampaign(brand);
            campaign.Engage(influencer);

            return $"{username} has been successfully attracted to the {brand} campaign.";

        }

        public string FundCampaign(string brand, double amount)
        {
            ICampaign campaign = campaigns.FindByName(brand);
            if (campaign == null)
            {
                return "Trying to fund an invalid campaign.";
            }
            if (amount <= 0)
            {
                return "Funding amount must be greater than zero.";
            }

            campaign.Gain(amount);

            return $"{brand} campaign has been successfully funded with {amount} $";
        }

        public string CloseCampaign(string brand)
        {
            ICampaign campaign = campaigns.FindByName(brand);
            if (campaign == null)
            {
                return "Trying to close an invalid campaign.";
            }

            if (campaign.Budget <= 10000)
            {
                return $"{brand} campaign cannot be closed as it has not met its financial targets.";
            }

            foreach (string contributor in campaign.Contributors)
            {
                IInfluencer influencer = influencers.FindByName(contributor);
                var bonus = 2000;

                if (influencer != null)
                {
                    influencer.EarnFee(bonus);
                    influencer.EndParticipation(brand);
                }
            }
            campaigns.RemoveModel(campaign);
            return $"{brand} campaign has reached its target.";
        }

        public string ConcludeAppContract(string username)
        {
            IInfluencer influencer = influencers.FindByName(username);

            if (influencer == null)
            {
                return $"{username} has still not signed a contract.";
            }

            if (influencer.Participations.Any())
            {
                return $"{username} cannot conclude the contract while enrolled in active campaigns.";
            }

            influencers.RemoveModel(influencer);

            return $"{username} concluded their contract.";
        }

        public string ApplicationReport()
        {
            StringBuilder report = new StringBuilder();

            var sortedInfluencers = influencers.Models
                .OrderByDescending(i => i.Income)
                .ThenByDescending(i => i.Followers)
                .ToList();

            foreach (IInfluencer influencer in sortedInfluencers)
            {
                report.AppendLine(influencer.ToString());

                var participations = influencer.Participations.ToList();

                if (participations.Any())
                {
                    report.AppendLine("Active Campaigns:");

                    foreach (var campaignBrand in participations)
                    {
                        var campaign = campaigns.FindByName(campaignBrand); 
                        if (campaign != null)
                        {
                            report.AppendLine($"--{campaign.GetType().Name} - Brand: {campaign.Brand}, Budget: {campaign.Budget}, Contributors: {campaign.Contributors.Count}");
                        }

                    }
                }
            }

            return report.ToString().Trim();
        }

        private Type GetInfluencerType(string typeName)
        {
            switch (typeName.ToLower())
            {
                case "businessinfluencer":
                    return typeof(BusinessInfluencer);
                case "fashioninfluencer":
                    return typeof(FashionInfluencer);
                case "bloggerinfluencer":
                    return typeof(BloggerInfluencer);
                default:
                    return null;
            }
        }
    }
}
