namespace Patients.Helpers
{
    using Xamarin.Forms;
    using Interfaces;
    using Resources;

    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept
        {
            get { return Resource.Accept; }
        }

        public static string Error
        {
            get { return Resource.Error; }
        }

        public static string NoInternet
        {
            get { return Resource.NoInternet; }
        }

        public static string Patients
        {
            get { return Resource.Patients; }
        }

        public static string TurnOnInternet
        {
            get { return Resource.TurnOnInternet; }
        }

        public static string AddPatient
        {
            get { return Resource.AddPatient; }
        }

        public static string FirstName
        {
            get { return Resource.FirstName; }
        }

        public static string FirstNamePlaceholder
        {
            get { return Resource.FirstNamePlaceholder; }
        }

        public static string LastName
        {
            get { return Resource.LastName; }
        }

        public static string LastNamePlaceholder
        {
            get { return Resource.LastNamePlaceholder; }
        }

        public static string Address
        {
            get { return Resource.Address; }
        }

        public static string AddressPlaceholder
        {
            get { return Resource.AddressPlaceholder; }
        }

        public static string Phone
        {
            get { return Resource.Phone; }
        }

        public static string PhonePlaceholder
        {
            get { return Resource.PhonePlaceholder; }
        }

        public static string TreatmentDescription
        {
            get { return Resource.TreatmentDescription; }
        }

        public static string TreatmentDescriptionPlaceholder
        {
            get { return Resource.TreatmentDescriptionPlaceholder; }
        }

        public static string Save
        {
            get { return Resource.Save; }
        }

        public static string ChangeImage
        {
            get { return Resource.ChangeImage; }
        }

        public static string FirstNameError
        {
            get { return Resource.FirstNameError; }
        }

        public static string LastNameError
        {
            get { return Resource.LastNameError; }
        }

        public static string PhoneError
        {
            get { return Resource.PhoneError; }
        }

        public static string AddressError
        {
            get { return Resource.AddressError; }
        }

        public static string TreatmentDescriptionError
        {
            get { return Resource.TreatmentDescriptionError; }
        }
    }
}
