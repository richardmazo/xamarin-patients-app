namespace Patients.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Patients.Models;
    using Services;
    using Xamarin.Forms;

    public class PatientsViewModel : BaseViewModel
    {

        private ApiService apiService;

        private ObservableCollection<Patient> patients;

        public ObservableCollection<Patient> Patients
        {
            get { return this.patients; }
            set { this.SetValue(ref this.patients, value); }
        }

        public PatientsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadPatients();
        }

        private async void LoadPatients()
        {
            var response = await this.apiService.GetList<Patient>("https://pratice1-2018-iiapi.azurewebsites.net", "/api", "/Patients");
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            var list = (List<Patient>)response.Result;
            this.Patients = new ObservableCollection<Patient>(list);
        }
    }
}
