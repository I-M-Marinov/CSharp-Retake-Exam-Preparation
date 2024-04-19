using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TheContentDepartment.Models.Contracts;
using TheContentDepartment.Utilities.Messages;

namespace TheContentDepartment.Models
{
    public abstract class TeamMember: ITeamMember
    {
        private string name;

        protected TeamMember(string name, string path)
        {
            Name = name;
            Path = path;
            inProgress = new List<string>();
        }


        public string Name
        {
            get { return name; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.NameNullOrWhiteSpace);
                }
                name = value;
            }
        }

        public string Path
        {
            get;
        }

        private List<string> inProgress;

        public IReadOnlyCollection<string> InProgress => inProgress.AsReadOnly();

        public void WorkOnTask(string resourceName)
        {
            inProgress.Add(resourceName);
        }

        public void FinishTask(string resourceName)
        {
            inProgress.Remove(resourceName);
        }
    }
}
