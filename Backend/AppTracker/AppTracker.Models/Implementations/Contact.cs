using AppTracker.Models.Contracts;

namespace AppTracker.Models.Implementations
{
    public class Contact: IContact, IEquatable<Contact>
    {
        public long ContactID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string PhoneCountry { get; set; }
        public string PhoneArea { get; set; }
        public string PhoneNumber { get; set; }
        public bool Deleted { get; set; }
        public bool Equals(Contact? obj)
        {
            if (obj != null)
            {
                return ContactID.Equals(obj.ContactID);
            }
            return false;
        }

        public Contact(string firstName, string lastName, string title, string company, string email, string phoneCountry, string phoneArea, string phoneNumber, bool deleted)
        {
            FirstName=firstName;
            LastName=lastName;
            Title=title;
            Company=company;
            Email=email;
            PhoneCountry=phoneCountry;
            PhoneArea=phoneArea;
            PhoneNumber=phoneNumber;
            Deleted=deleted;
        }
    }
}
