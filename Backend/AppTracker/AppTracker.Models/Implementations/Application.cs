using AppTracker.Models.Contracts;

namespace AppTracker.Models.Implementations
{
    public class Application: IApplication, IEquatable<Application>
    {
        public long ApplicationID { get; set; }
        public string UserHash { get; set; }
        public DateTime SubmissionDateTime { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public long Salary { get; set; }
        public string Link { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public bool IsRemote { get; set; }
        public bool Deleted { get; set; }
        
        public bool Equals(Application? obj)
        {
            if(obj != null)
            {
                return ApplicationID.Equals(obj.ApplicationID);
            }
            return false;
        }

        public Application(string userHash, string position, string company)
        {
            UserHash = userHash;
            Position = position;
            Company = company;
        }

        public Application(string userHash, DateTime submissionDateTime, string company, string position, string type, string status, long salary, string link, string state, string city, string country, string description, bool deleted)
        {
            UserHash=userHash;
            SubmissionDateTime=submissionDateTime;
            Company=company;
            Position=position;
            Type=type;
            Status=status;
            Salary=salary;
            Link=link;
            State=state;
            City=city;
            Country=country;
            Description=description;
            Deleted=deleted;
        }

    }
}
