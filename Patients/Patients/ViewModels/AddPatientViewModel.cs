using System;
using System.Collections.Generic;
using System.Text;

namespace Patients.ViewModels
{
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Patients.Models;
    using Patients.Services;
    using Xamarin.Forms;

    public class AddPatientViewModel : BaseViewModel
    {
        #region Attributes

        private ApiService apiService;

        private bool isRunning;

        private bool isEnabled;

        #endregion

        #region Properties

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string TreatmentDescription { get; set; }  

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }

        #endregion

        #region Constructors
        public AddPatientViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.FirstNameError,
                    Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(this.LastName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.LastNameError,
                    Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(this.Address))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.AddressError,
                    Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(this.Phone))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PhoneError,
                    Languages.Accept);
                return;
            }

            /*var phone = decimal.Parse(this.Phone);

            if (phone < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PhoneError,
                    Languages.Accept);
                return;
            }*/

            if (string.IsNullOrEmpty(this.Phone))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PhoneError,
                    Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(this.TreatmentDescription))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.TreatmentDescriptionError,
                    Languages.Accept);
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            var patient = new Patient
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Address = this.Address,
                Phone = this.Phone,
                PatientSince = DateTime.Now.ToUniversalTime(),
                HasAllergies = true,
                TreatmentDescription = this.TreatmentDescription,
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlPatientsController"].ToString();

            var response = await this.apiService.Post(url, prefix, controller, patient);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            var newPatient = (Patient)response.Result;
            var viewModel = PatientsViewModel.GetInstance();
            viewModel.Patients.Add(newPatient);

            this.IsRunning = false;
            this.IsEnabled = true;

            await Application.Current.MainPage.Navigation.PopAsync();

        }
        #endregion
    }
}
