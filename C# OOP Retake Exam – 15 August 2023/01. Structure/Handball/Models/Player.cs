using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Handball.Models.Contracts;
using Handball.Utilities.Messages;

namespace Handball.Models
{
    public abstract class Player :IPlayer
    {
        public Player(string name, double rating)
        {
            Name = name;
            Rating = rating;
        }

        private string name;
        private double rating;
        private string team;

        public string Name
        {
            get { return name; }
            private set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.PlayerNameNull);
                }
                name = value;
            }
        }

        public double Rating
        {
            get { return rating; }
            protected set
            {
                if (value > 10)
                {
                    rating = 10;
                    return;
                }
                else if (value < 1)
                {
                    rating = 1;
                    return;
                }

                rating = value;
            }

        }

        public string Team
        {
            get { return team; }

        }

        public void JoinTeam(string name)
        {
            team = name;
        }

        public abstract void IncreaseRating();
        public abstract void DecreaseRating();

        public override string ToString()
        {
           StringBuilder sb = new StringBuilder();

           sb.AppendLine($"{this.GetType().Name}: {Name}");
           sb.AppendLine($"--Rating: {Rating}");

           return sb.ToString().Trim();
        }
    }
}
