namespace Patients.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Patients.Views;
    using Xamarin.Forms;

    public class MainViewModel
    {
        #region Properties
        public EditPatientViewModel EditPatient { get; set; }

        public PatientsViewModel Patients { get; set; }

        public AddPatientViewModel AddPatient { get; set; }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.Patients = new PatientsViewModel();
        }
        #endregion

        #region Singleton
        public static MainViewModel instance { get; set; }

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion

        #region Commands
        public ICommand AddPatientCommand
        {
            get
            {
                return new RelayCommand(GoToAddPatient);
            }
        }

        private async void GoToAddPatient()
        {
            this.AddPatient = new AddPatientViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new AddPatientPage());
        } 
        #endregion
    }
}
