namespace Patients.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Patients.Views;
    using Xamarin.Forms;

    public class MainViewModel
    {
        public PatientsViewModel Patients { get; set; }

        public AddPatientViewModel AddPatient { get; set; }

        public MainViewModel()
        {
            this.Patients = new PatientsViewModel();
        }

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
    }
}
