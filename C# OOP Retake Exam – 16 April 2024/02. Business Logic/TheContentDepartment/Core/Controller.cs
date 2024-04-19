using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheContentDepartment.Core.Contracts;
using TheContentDepartment.Models;
using TheContentDepartment.Models.Contracts;
using TheContentDepartment.Repositories;
using TheContentDepartment.Repositories.Contracts;

namespace TheContentDepartment.Core
{
    public class Controller: IController
    {

        private IRepository<IResource> resources;
        private IRepository<ITeamMember> teamMembers;

        private string[] validMemberTypes = { "TeamLead", "ContentMember"};
        private string[] validResourceTypes = { "Exam", "Workshop", "Presentation" };
        public Controller()
        {
            resources = new ResourceRepository();
            teamMembers = new MemberRepository();
        }


        public string JoinTeam(string memberType, string memberName, string path)
        {
            if (!validMemberTypes.Contains(memberType))
            {
                return $"{memberType} is not a valid member type.";
            }

            if (memberType == "ContentMember")
            {
                foreach (var member in teamMembers.Models)
                {
                    if (member is ContentMember contentMember && contentMember.Path == path)
                    {
                        return "Position is occupied.";
                    }
                }
            }

            if (teamMembers.Models.Any(m => m.Name == memberName))
            {
                return $"{memberName} has already joined the team.";
            }

            ITeamMember newMember = null;

            if (memberType == "TeamLead")
            {
                newMember = new TeamLead(memberName, path);
            }
            else if (memberType == "ContentMember")
            {
                newMember = new ContentMember(memberName, path);
            }

            teamMembers.Add(newMember);
            
            return $"{memberName} has joined the team. Welcome!";
        }

        public string CreateResource(string resourceType, string resourceName, string path)
        {
            if (!validResourceTypes.Contains(resourceType))
            {
                return $"{resourceType} type is not handled by Content Department.";
            }

            ITeamMember contentCreator = null;

            foreach (var member in teamMembers.Models)
            {
                if (member is ContentMember contentMember && contentMember.Path == path)
                {
                    contentCreator = contentMember;

                    if (contentCreator.InProgress.Contains(resourceName))
                    {
                        return $"The {resourceName} resource is being created.";
                    }

                    break; 
                }
            }

            if (contentCreator == null)
            {
                return $"No content member is able to create the {resourceName} resource.";
            }

            IResource newResource = null;
            var contentCreatorName = contentCreator.Name;

            if (resourceType == "Exam")
            {
                newResource = new Exam(resourceName, contentCreatorName);
            }
            else if (resourceType == "Workshop")
            {
                newResource = new Workshop(resourceName, contentCreatorName);
            }
            else if (resourceType == "Presentation")
            {
                newResource = new Presentation(resourceName, contentCreatorName);
            }

            resources.Add(newResource);
            contentCreator.WorkOnTask(resourceName);
            return $"{contentCreatorName} created {resourceType} - {resourceName}.";
        }

        public string LogTesting(string memberName)
        {
            ITeamMember member = teamMembers.Models.FirstOrDefault(m => m.Name == memberName);
            if (member == null)
            {
                return "Provide the correct member name.";
            }

            IResource resourceToBeTested = null;

            foreach (var resource in resources.Models)
            {
                if (resource.Creator == memberName && !resource.IsTested)
                {
                    if (resourceToBeTested == null || resource.Priority < resourceToBeTested.Priority)
                    {
                        resourceToBeTested = resource;
                    }
                }
            }

            if (resourceToBeTested == null)
            {
                return $"{memberName} has no resources for testing.";
            }

            TeamLead teamLead = FindTeamLead();


            member.FinishTask(resourceToBeTested.Name);
            teamLead.WorkOnTask(resourceToBeTested.Name);

            if (!resourceToBeTested.IsTested)
            {
                resourceToBeTested.Test();
            }

            return $"{resourceToBeTested.Name} is tested and ready for approval.";
        }

        public string ApproveResource(string resourceName, bool isApprovedByTeamLead)
        {
            IResource resource = resources.Models.First(m => m.Name == resourceName);

            if (!resource.IsTested)
            {
                return $"{resourceName} cannot be approved without being tested.";
            }

            TeamLead teamLead = FindTeamLead();

            if (!isApprovedByTeamLead)
            {
                if (resource.IsTested)
                {
                    resource.Test(); // change the IsTested bool back to FALSE ( If it is TRUE )
                }
                return $"{teamLead.Name} returned {resourceName} for a re-test.";
            }

            resource.Approve();
            teamLead.FinishTask(resourceName);

            return $"{teamLead.Name} approved {resourceName}.";

        }

        public string DepartmentReport()
        {
            StringBuilder reportBuilder = new StringBuilder();

            reportBuilder.AppendLine("Finished Tasks:");

            foreach (var resource in resources.Models)
            {
                if (resource.IsApproved)
                {
                    reportBuilder.AppendLine($"--{resource}");
                }
            }

            reportBuilder.AppendLine("Team Report:");

            TeamLead teamLead = FindTeamLead();

            foreach (var member in teamMembers.Models)
            {
                if (member is TeamLead)
                {
                    reportBuilder.AppendLine($"--{teamLead}");
                }
                else
                {
                    reportBuilder.AppendLine($"{member}");
                }

            }

            return reportBuilder.ToString().Trim();
        }

        private TeamLead FindTeamLead()
        {
            TeamLead teamLead = null;

            foreach (var teamMember in teamMembers.Models)
            {
                if (teamMember is TeamLead)
                {
                    teamLead = (TeamLead)teamMember;
                    break;
                }
            }
            return  teamLead;
        }
    }
}
