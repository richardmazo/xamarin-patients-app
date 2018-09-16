namespace Patients.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Patients.Models;
    using Patients.Services;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Xamarin.Forms;

    public class AddPatientViewModel : BaseViewModel
    {
        #region Attributes

        private MediaFile file;

        private ApiService apiService;

        private bool isRunning;

        private bool isEnabled;

        private ImageSource imageSource;

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

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }


        #endregion

        #region Constructors
        public AddPatientViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
            this.ImageSource = "nopatients";
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

        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.ImageSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.NewPicture);

            if (source == Languages.Cancel)
            {
                this.file = null;
                return;
            }

            if (source == Languages.NewPicture)
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = this.file.GetStream();
                    return stream;
                });
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

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var patient = new Patient
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Address = this.Address,
                Phone = this.Phone,
                PatientSince = DateTime.Now.ToUniversalTime(),
                TreatmentDescription = this.TreatmentDescription,
                HasAllergies = true,
                ImageArray = imageArray,
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
