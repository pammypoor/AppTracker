using AppTracker.Models.Contracts;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace AppTracker.Models.Implementations
{

    [DataContract]
    public class Application: IApplication, IEquatable<Application>
    {
        [DataMember]
        public long ApplicationID { get; set; }
        [DataMember]
        public string UserHash { get; set; }
        [DataMember]
        public DateTime SubmissionDateTime { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string Position { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public long Salary { get; set; }
        [DataMember]
        public string Link { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool IsRemote { get; set; }
        [DataMember]
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

        public Application(string userHash, DateTime submissionDateTime, string company, string position, string type, string status, long salary, string link, string state, string city, string country, string description, bool isRemote, bool deleted)
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
            IsRemote = isRemote;
            Deleted=deleted;
        }

        [JsonConstructor]
        public Application(long applicationID, string userHash, DateTime submissionDateTime, string company, string position, string type, string status, long salary, string link, string state, string city, string country, string description, bool isRemote, bool deleted)
        {
            ApplicationID = applicationID;
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
            IsRemote = isRemote;
            Deleted=deleted;
        }

    }
}
