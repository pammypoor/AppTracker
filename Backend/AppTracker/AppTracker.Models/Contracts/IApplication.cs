namespace AppTracker.Models.Contracts
{
    public interface IApplication
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

    }
}
