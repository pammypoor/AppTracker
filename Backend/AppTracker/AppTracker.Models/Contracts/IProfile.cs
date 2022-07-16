

namespace AppTracker.Models.Contracts
{
    public interface IProfile
    {
        public string Pronouns { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public string Degree { get; set; }
        public string School { get; set; }
        public string Field { get; set; }
        public DateTime GraduationDate { get; set; }
        public string LocationCity { get; set; }
        public string LocationCountry { get; set; }
        public string About { get; set; }
    }
}
