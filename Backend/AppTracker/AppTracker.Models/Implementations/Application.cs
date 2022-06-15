using AppTracker.Models.Contracts;
using System.Runtime.Serialization;

namespace AppTracker.Models.Implementations
{
    public enum status
    {

    }

    [DataContract]
    public class Application: IApplication, IEquatable<Application>
    {
        [DataMember]
        public long ApplicationID { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public long Salary { get; set; }

        public bool Equals(Application? obj)
        {
            if(obj != null)
            {
                return ApplicationID.Equals(obj.ApplicationID);
            }
            return false;
        }

    }
}
