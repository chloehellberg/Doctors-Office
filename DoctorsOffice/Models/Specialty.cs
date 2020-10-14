using System.Collections.Generic;

namespace DoctorsOffice.Models
{
  public class Specialty
  {
public Specialty()
    {
      this.Doctors = new HashSet<DoctorSpecialty>();
    }
    public int SpecialtyId { get; set; }
    public string SpecialtyName { get; set; }
    public virtual ICollection<DoctorSpecialty> Doctors { get; set; }
  }
}