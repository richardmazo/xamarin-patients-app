﻿namespace Patients.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Models;
    using Services;
    using Xamarin.Forms;

    public class PatientsViewModel : BaseViewModel
    {

        private ApiService apiService;

        private bool isRefreshing;

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

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

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

            var list = (List<Patient>)response.Result;
            this.Patients = new ObservableCollection<Patient>(list);
            this.IsRefreshing = false;
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadPatients);
            }
        }
    }
}
