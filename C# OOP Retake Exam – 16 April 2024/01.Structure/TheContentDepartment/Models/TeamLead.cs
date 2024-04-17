using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TheContentDepartment.Utilities.Messages;

namespace TheContentDepartment.Models
{
    public class TeamLead:TeamMember
    {
        private string path;
        public TeamLead(string name, string path) : base(name, path)
        {
            Path = path;
        }
        public string Path
        {
            get { return path; }
            set
            {
                if (value != "Master")
                {
                    throw new ArgumentException(ExceptionMessages.PathIncorrect, path);
                }

                path = value;
            }
        }


        public override string ToString()
        {
            int currentTasksCount = InProgress.Count;

            StringBuilder sb = new StringBuilder();
            sb.Append($"{Name} ({GetType().Name}) - Currently working on {currentTasksCount} tasks.");

            return sb.ToString().Trim();
        }
    }
}
