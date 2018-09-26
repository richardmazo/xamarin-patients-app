namespace Patients.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Models;
    using Services;
    using Xamarin.Forms;

    public class PatientsViewModel : BaseViewModel
    {

        #region Attributes

        private ObservableCollection<PatientItemViewModel> patients;

        private ApiService apiService;

        private bool isRefreshing;
        #endregion

        #region Properties

        public List<Patient> MyPatients { get; set; }

        public ObservableCollection<PatientItemViewModel> Patients
        {
            get { return this.patients; }
            set { this.SetValue(ref this.patients, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        #endregion

        #region Constructors
        public PatientsViewModel()
        {
            instance = this;
            this.apiService = new ApiService();
            this.LoadPatients();
        }
        #endregion

        #region Singleton
        public static PatientsViewModel instance { get; set; }

        public static PatientsViewModel GetInstance()
        {
            if (instance==null)
            {
                return new PatientsViewModel();
            }

            return instance;
        }
        #endregion

        #region Private Methods
        private async void LoadPatients()
        {
            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlPatientsController"].ToString();
            var response = await this.apiService.GetList<Patient>(url, prefix, controller);
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            this.MyPatients = (List<Patient>)response.Result;
            this.RefreshList();
            this.IsRefreshing = false;
        }

        public void RefreshList()
        {

            var myListPatientItemViewModel = MyPatients.Select(p => new PatientItemViewModel
            {
                PatientId = p.PatientId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Address = p.Address,
                PatientSince = p.PatientSince,
                Phone = p.Phone,
                TreatmentDescription = p.TreatmentDescription,
                ImagePath = p.ImagePath,
                HasAllergies = p.HasAllergies,               
                ImageArray = p.ImageArray,
            });

            this.Patients = new ObservableCollection<PatientItemViewModel>(
                myListPatientItemViewModel.OrderBy(p => p.FirstName));
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadPatients);
            }
        } 
        #endregion
    }
}
