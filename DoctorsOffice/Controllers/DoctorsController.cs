using Microsoft.AspNetCore.Mvc;
using DoctorsOffice.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace DoctorsOffice.Controllers
{
  public class DoctorsController : Controller
  {
  private readonly DoctorsOfficeContext _db;

    public DoctorsController(DoctorsOfficeContext db)
    {
      _db = db;
    }

    public ActionResult Index() //returns list of all doctors
    {
      List<Doctor> model = _db.Doctors.ToList();
        return View(model);
    }
    public ActionResult Create() //returns page with form for new doctor
    {
      ViewBag.SpecialtyId = new SelectList(_db.Specialties, "SpecialtyId", "SpecialtyName"); //gives info to dropdown list in form so doctors can be assigned a specialty
      return View();
    }
    [HttpPost]
    public ActionResult Create(Doctor doctor, int SpecialtyId) //Create the new doctor from the form submit
    {
      if (SpecialtyId != 0)
      {
        _db.DoctorSpecialty.Add(new DoctorSpecialty() { DoctorId = doctor.DoctorId, SpecialtyId = SpecialtyId });
      }
      _db.Doctors.Add(doctor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Details(int id) //Displays details on a specific doctor
    {
      var thisDoctor = _db.Doctors
        .Include(doctors => doctors.Patients) //
        .ThenInclude(join => join.Patient)
        .Include(doctors => doctors.Specialties)
        .ThenInclude(join => join.Specialty)
        .FirstOrDefault(doctor => doctor.DoctorId == id);
        return View(thisDoctor);
    }
    public ActionResult Edit(int id) //Edits info for a specific doctor
    {
      var thisDoctor = _db.Doctors.FirstOrDefault(doctors => doctors.DoctorId == id);
      ViewBag.SpecialtyId = new SelectList(_db.Specialties, "SpecialtyId", "SpecialtyName"); //displays dropdown list to edit doctor's specialty
      return View(thisDoctor);
    }
    [HttpPost]
    public ActionResult Edit(Doctor doctor, int SpecialtyId)
    {
      if (SpecialtyId != 0)
      {
        bool tf = _db.DoctorSpecialty.Any( x => x.SpecialtyId == SpecialtyId && x.DoctorId == doctor.DoctorId );
        if (!tf)
        {
          _db.DoctorSpecialty.Add(new DoctorSpecialty() { DoctorId = doctor.DoctorId, SpecialtyId = SpecialtyId });
        }
      }
      _db.Entry(doctor).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Delete(int id)
    {
        var thisDoctor = _db.Doctors.FirstOrDefault(doctors => doctors.DoctorId == id);
        return View(thisDoctor);
    }
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
        var thisDoctor = _db.Doctors.FirstOrDefault(doctors => doctors.DoctorId == id);
        _db.Doctors.Remove(thisDoctor);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
    public ActionResult DeletePatient(int joinId, int DoctorId)
    {
      var joinEntry = _db.DoctorPatient.FirstOrDefault(entry => entry.DoctorPatientId == joinId);
      _db.DoctorPatient.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = DoctorId});
    }
    // public ActionResult Completed(int joinId)
    // {
    //     var joinEntry = _db.DoctorPatient.FirstOrDefault(entry => entry.DoctorPatientId == joinId);
    //     joinEntry.Completed = true;
    //     _db.Entry(joinEntry).State = EntityState.Modified;
    //     _db.SaveChanges();
    //     return RedirectToAction("Details", new { id = joinEntry.DoctorId });
    // }
  }
}
