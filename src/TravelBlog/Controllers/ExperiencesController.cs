﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelBlog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace TravelBlog.Controllers
{
    public class ExperiencesController : Controller
    {
        private TravelBlogDbContext db = new TravelBlogDbContext();
        public IActionResult Index()
        {
            return View(db.Experiences.Include(experiences => experiences.Location).ToList());
        }
        public IActionResult Details(int id)
        {
            var thisExperience = db.Experiences
                .Include(experiences => experiences.Location)
                .Include(experiences => experiences.PeopleExperiences)
                .ThenInclude(experiences => experiences.People)
                .FirstOrDefault(experiences=>experiences.ExperienceId == id);
            return View(thisExperience);
        }
        public IActionResult Create()
        {
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "CityName");
            ViewBag.PersonId = new SelectList(db.Peoples, "PersonId", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Experience experience, People person)
        {
            db.Experiences.Add(experience);
            var newPeopleExperience = new PeopleExperience{  PersonId = person.PersonId, ExperienceId = experience.ExperienceId };
            db.PeopleExperiences.Add(newPeopleExperience);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var thisExperience = db.Experiences.FirstOrDefault(experiences => experiences.ExperienceId == id);
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "CityName");
            return View(thisExperience);
        }
        [HttpPost]
        public IActionResult Edit(Experience experience)
        {
            db.Entry(experience).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var thisExperience = db.Experiences.FirstOrDefault(experiences => experiences.ExperienceId == id);
            return View(thisExperience);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var thisExperience = db.Experiences.FirstOrDefault(experiences => experiences.ExperienceId == id);
            db.Experiences.Remove(thisExperience);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
