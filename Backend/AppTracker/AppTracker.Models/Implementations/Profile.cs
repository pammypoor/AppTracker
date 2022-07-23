using AppTracker.Models.Contracts;
using System.Text.Json.Serialization;

namespace AppTracker.Models.Implementations
{
    public class Profile: IProfile, IEquatable<Profile>
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

        public bool Equals(Profile? obj)
        {
            if (obj != null)
            {
                return Position.Equals(obj.Position) && Company.Equals(obj.Company) && GraduationDate.Equals(obj.GraduationDate) && LocationCity.Equals(obj.LocationCity) && About.Equals(obj.About) ;
            }
            return false;
        }

        [JsonConstructor]
        public Profile(string pronouns, string position, string company, string degree, string school, string field, DateTime graduationDate, string locationCity, string locationCountry, string about)
        {
            Pronouns=pronouns;
            Position=position;
            Company=company;
            Degree=degree;
            School=school;
            Field=field;
            GraduationDate=graduationDate;
            LocationCity=locationCity;
            LocationCountry=locationCountry;
            About=about;
        }

        public Profile(string pronouns, string position, string company)
        {
            Pronouns=pronouns;
            Position=position;
            Company=company;
        }
    }
}
