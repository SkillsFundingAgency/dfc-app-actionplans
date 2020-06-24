namespace DFC.App.ActionPlans.Models
{
    public class ContactDetails
    {
        private string _webchatLink= "https://nationalcareers.service.gov.uk/webchat/chat/";
        private string _phone="0800 100 900";
        private string _contactDaysTime ="8am to 10pm, 7 days a week";

        public string Phone
        {
            get => _phone;
            set => _phone = value;
        }

        public string WebchatLink
        {
            get => _webchatLink;
            set => _webchatLink = value;
        }

        public string ContactDaysTime
        {
            get => _contactDaysTime;
            set => _contactDaysTime = value;
        }
    }
}
