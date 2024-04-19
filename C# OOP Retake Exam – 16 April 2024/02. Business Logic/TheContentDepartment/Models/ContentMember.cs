using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TheContentDepartment.Utilities.Messages;

namespace TheContentDepartment.Models
{
    public class ContentMember: TeamMember
    {
        private string[] validPaths = { "CSharp", "JavaScript", "Python", "Java" };
        private string path;

        public ContentMember(string name, string path) : base(name, path)
        {
            Path = path;
        }

        public string Path
        {
            get { return path; }
            private set
            {
                if (!validPaths.Any(p => p.ToLower() == value.ToLower()))
                {
                    throw new ArgumentException($"{value} path is not valid.");
                }

                path = value;
            }
        }

        public override string ToString()
        {
            int currentTasksCount = InProgress.Count;
            StringBuilder sb = new StringBuilder();

            sb.Append($"{Name} - {Path} path. Currently working on {currentTasksCount} tasks.");

            return sb.ToString().Trim();
        }
    }
}
