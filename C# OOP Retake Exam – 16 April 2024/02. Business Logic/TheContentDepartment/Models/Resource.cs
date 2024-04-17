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
    public abstract class Resource: IResource
    {
        private string name;

        protected Resource(string name, string creator, int priority)
        {
            Name = name;
            Creator = creator;
            Priority = priority;
        }


        public string Name
        {
            get { return name; }
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.NameNullOrWhiteSpace);
                }
                name = value;
            }
        }
        public string Creator { get; }
        public int Priority { get; }

        public bool IsTested
        {
            get;
            private set;
        }
        public bool IsApproved
        {
            get;
            private set;
        }
        public void Test()
        {
            if (IsTested)
            {
                IsTested = false;
            }
            else
            {
                IsTested = true;
            }
        }

        public void Approve()
        {
            IsApproved = true;
        }

        public override string ToString()
        {
           StringBuilder sb = new StringBuilder();
           sb.Append($"{Name} ({this.GetType().Name}), Created By: {Creator}");

           return sb.ToString().Trim();
        }
    }
}
