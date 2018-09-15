using System;
using System.Collections.Generic;
using System.Text;

namespace Patients.ViewModels
{
    public class MainViewModel
    {
        public PatientsViewModel Patients { get; set; }

        public MainViewModel()
        {
            this.Patients = new PatientsViewModel();
        }
    }
}
