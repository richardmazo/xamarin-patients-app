namespace Patients.ViewModels
{
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Patients.Helpers;
    using Patients.Models;
    using Services;
    using Xamarin.Forms;

    public class PatientItemViewModel : Patient
    {

        #region Attributes
        private ApiService apiService;
        #endregion

        #region Constructors
        public PatientItemViewModel()
        {
            this.apiService = new ApiService();
        } 
        #endregion

        #region Commands
        public ICommand DeletePatientCommand
        {
            get
            {
                return new RelayCommand(DeletePatient);
            }
        }

        private async void DeletePatient()
        {
            var answer = await Application.Current.MainPage.DisplayAlert(
                Languages.Confirm, 
                Languages.DeleteConfirmation, 
                Languages.Yes, 
                Languages.No);

            if (!answer)
            {
                return;
            }

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlPatientsController"].ToString();
            var response = await this.apiService.Delete(url, prefix, controller, this.PatientId);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            var patientsViewModel = PatientsViewModel.GetInstance();

            var deletedPatient = patientsViewModel.Patients.Where(p => p.PatientId == this.PatientId).FirstOrDefault();
            if (deletedPatient != null)
            {
                patientsViewModel.Patients.Remove(deletedPatient);
            }


        }
        #endregion
    }
}
