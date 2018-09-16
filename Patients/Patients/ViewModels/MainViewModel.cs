namespace Patients.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Patients.Views;
    using Xamarin.Forms;

    public class MainViewModel
    {
        public PatientsViewModel Patients { get; set; }

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
            await Application.Current.MainPage.Navigation.PushAsync(new AddPatientPage());
        }
    }
}
