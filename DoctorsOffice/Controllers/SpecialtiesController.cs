using Microsoft.AspNetCore.Mvc;
using DoctorsOffice.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace DoctorsOffice.Controllers
{
  public class SpecialtiesController : Controller
  {
    private readonly DoctorsOfficeContext _db;

    public SpecialtiesController(DoctorsOfficeContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Specialties.ToList());
    }
    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Specialty specialty, int DoctorId)
    {
      if (DoctorId != 0)
      {
        _db.DoctorSpecialty.Add(new DoctorSpecialty() { DoctorId = DoctorId, SpecialtyId = specialty.SpecialtyId });
      }
      _db.Specialties.Add(specialty);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Edit(int id)
    {
      var thisSpecialty = _db.Specialties.FirstOrDefault(specialty => specialty.SpecialtyId == id);
      return View(thisSpecialty);
    }

    [HttpPost]
    public ActionResult Edit(Specialty specialty, int DoctorId)
    {
      if (DoctorId != 0)
      {
        bool tf = _db.DoctorSpecialty.Any( x => x.SpecialtyId == specialty.SpecialtyId && x.DoctorId == DoctorId );
        if (!tf)
        {
          _db.DoctorSpecialty.Add(new DoctorSpecialty() { DoctorId = DoctorId, SpecialtyId = specialty.SpecialtyId });
        }
      }
      _db.Entry(specialty).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisSpecialty = _db.Specialties
        .Include(specialty => specialty.Doctors)
        .ThenInclude(join => join.Doctor)
        .FirstOrDefault(specialty => specialty.SpecialtyId == id);
      return View(thisSpecialty);
    }
    public ActionResult Delete(int id)
    {
      var thisSpecialty = _db.Specialties.FirstOrDefault(specialty => specialty.SpecialtyId == id);
      return View(thisSpecialty);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisSpecialty = _db.Specialties.FirstOrDefault(specialty => specialty.SpecialtyId == id);
      _db.Specialties.Remove(thisSpecialty);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}