using Microsoft.AspNetCore.Mvc;
using DoctorsOffice.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace DoctorsOffice.Controllers
{
  public class PatientsController : Controller
  {
  private readonly DoctorsOfficeContext _db;

    public PatientsController(DoctorsOfficeContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Patient> model = _db.Patients.ToList();
        return View(model);
    }
    public ActionResult Create()
    {
      ViewBag.DoctorId = new SelectList(_db.Doctors, "DoctorId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Patient patient, int DoctorId)
    {
      _db.Patients.Add(patient);
      if (DoctorId != 0)
      {
          _db.DoctorPatient.Add(new DoctorPatient() { DoctorId = DoctorId, PatientId = patient.PatientId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Details(int id)
    {
      var thisPatient = _db.Patients
      .Include(patients => patients.Doctors)
      .ThenInclude(join => join.Doctor)
      .FirstOrDefault(patient => patient.PatientId == id);
      return View(thisPatient);
    }
    public ActionResult Edit(int id)
    {
      var thisPatient = _db.Patients.FirstOrDefault(patients => patients.PatientId == id);
      ViewBag.DoctorId = new SelectList(_db.Doctors, "DoctorId", "Name");
      return View(thisPatient);
    }

    [HttpPost]
    public ActionResult Edit(Patient patient, int DoctorId)
    {
      if (DoctorId != 0)
      {
        bool tf = _db.DoctorPatient.Any( x => x.DoctorId == DoctorId && x.PatientId == patient.PatientId ); //if true patient has already been assigned to that doctor, if false it assigns the doctor
        if (!tf)
        {
          _db.DoctorPatient.Add(new DoctorPatient() { DoctorId = DoctorId, PatientId = patient.PatientId });
        }
      }
      _db.Entry(patient).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        var thisPatient = _db.Patients.FirstOrDefault(patients => patients.PatientId == id);
        return View(thisPatient);
    }
    
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
        var thisPatient = _db.Patients.FirstOrDefault(patients => patients.PatientId == id);
        _db.Patients.Remove(thisPatient);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
    public ActionResult DeleteDoctor(int joinId)
    {
      var joinEntry = _db.DoctorPatient.FirstOrDefault(entry => entry.DoctorPatientId == joinId);
      _db.DoctorPatient.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}
