
using System.ComponentModel;

namespace v3.climbingfish.be.Models
{
    public class ContactModel
    {
        [DisplayName("Voornaam en Naam")]
        public string FullName { get; set; }

        [DisplayName("Waarover wil je ons contacteren?")]
        public string Subject { get; set; }

        [DisplayName("Je email")]
        public string Email { get; set; }

        [DisplayName("Telefoon")]
        public string Telephone { get; set; }

        [DisplayName("Schrijf hieronder je bericht")]
        public string Message { get; set; }

        public string TargetPage { get; set; }
    }
}