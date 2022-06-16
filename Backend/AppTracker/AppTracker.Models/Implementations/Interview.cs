using AppTracker.Models.Contracts;

namespace AppTracker.Models.Implementations
{
    public class Interview: IInterview, IEquatable<Interview>
    {
        public long InterviewID { get; set; }
        public DateTime Appointment { get; set; }
        public bool Online { get; set; }
        public string LocationAddress { get; set; }
        public string LocationCity { get; set; }
        public string LocationState { get; set; }
        public string LocationCountry { get; set; }
        public string InterviewLink { get; set; }
        public bool Equals(Interview? obj)
        {
            if (obj != null)
            {
                return InterviewID.Equals(obj.InterviewID);
            }
            return false;
        }

        public Interview(DateTime Appointment, bool Online, string LocationAddress, string LocationCity, string LocationState, string LocationCountry, string InterviewLink)
        {
            this.Appointment = Appointment;
            this.Online = Online;
            this.LocationAddress = LocationAddress;
            this.LocationCity = LocationCity;
            this.LocationState = LocationState;
            this.LocationCountry = LocationCountry;
            this.InterviewLink = InterviewLink;
        }

    }
}
