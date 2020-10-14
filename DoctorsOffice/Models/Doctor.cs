using System.Collections.Generic;

namespace DoctorsOffice.Models
{
  public class Doctor
  {
    public Doctor() //Constructor
    {
      this.Patients = new HashSet<DoctorPatient>();
      this.Specialties = new HashSet<DoctorSpecialty>();
    }

    public int DoctorId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<DoctorPatient> Patients { get; set; }
    public virtual ICollection<DoctorSpecialty> Specialties { get; set; }
  }
}