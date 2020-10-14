using System.Collections.Generic;

namespace DoctorsOffice.Models
{
  public class Specialty
  {
public Specialty()
    {
        this.Doctors = new HashSet<Doctor>();
    }

    public int SpecialtyId { get; set; }
    public string SpecialtyName { get; set; }
    public virtual ICollection<Doctor> Doctors { get; set; }
    public virtual ICollection<DoctorSpecialty> Specialties { get; set; }
  }
}