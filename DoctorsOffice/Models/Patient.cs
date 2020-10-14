using System.Collections.Generic;

namespace DoctorsOffice.Models
{
    public class Patient
    {
        public Patient()
        {
            this.Doctors = new HashSet<DoctorPatient>();
        }

        public int PatientId { get; set; }
        public string DateOfBirth { get; set; }
        public string Name { get; set; }
        public virtual ICollection<DoctorPatient> Doctors { get; set; }
    }
}