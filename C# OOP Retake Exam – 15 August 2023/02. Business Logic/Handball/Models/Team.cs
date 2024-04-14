using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Handball.Models.Contracts;
using Handball.Utilities.Messages;

namespace Handball.Models
{
    public class Team : ITeam
    {
        public Team(string name)
        {
            Name = name;
            players = new List<IPlayer>();
        }

        private string name;
        private int pointsEarned;
        private double overallRating;

        public string Name
        {
            get { return name; }
            private set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.TeamNameNull);
                }

                name = value;
            }
        }


        public int PointsEarned
        {
            get { return pointsEarned; }
            private set { pointsEarned = value; }
        }

        public double OverallRating
        {
            get
            {
                if (players.Count == 0)
                {
                    return 0;
                }

                return Math.Round(players.Average(p => p.Rating), 2);
            }
        }


        private List<IPlayer> players;

        public IReadOnlyCollection<IPlayer> Players
        {
            get { return players.AsReadOnly(); }

        }

        public void SignContract(IPlayer player)
        {
            players.Add(player);
        }

        public void Win()
        {
            PointsEarned += 3;

            foreach (var player in players)
            {
                player.IncreaseRating();
            }
        }

        public void Lose()
        {
            foreach (var player in players)
            {
                player.DecreaseRating();
            }

        }

        public void Draw()
        {
            pointsEarned += 1;
            IPlayer goalKeeper = players.FirstOrDefault(p => p is Goalkeeper);

            if (goalKeeper != null)
            {
                goalKeeper.IncreaseRating();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            string playersToString = "none";

            if (players.Count > 0)
            {
                playersToString = String.Join(", ", players.Select(p => p.Name));
            }

            sb.AppendLine($"Team: {Name} Points: {PointsEarned}");
            sb.AppendLine($"--Overall rating: {OverallRating}");
            sb.AppendLine($"--Players: {playersToString}");

            return sb.ToString().Trim();
        }
    }
}


