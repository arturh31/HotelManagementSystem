using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManagementSystem.Models;
using HotelManagementSystem.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{
    
    [Authorize]
    public class GuestController : Controller
    {
        private readonly IGuestService _guestService;
        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;
        }

        public IActionResult Index()
        {
            var guests = _guestService.GetAllGuests();
            return View(guests);
        }
        public IActionResult Details(int id)
        {
            return View(_guestService.GetGuestById(id));
        }
        public IActionResult Create(int id)
        {
            return View(_guestService.GetGuestById(id));
        }
        public IActionResult Created(Guest newGuest)
        {
            if (ModelState.IsValid)
            {
                _guestService.CreateGuest(newGuest);
                return RedirectToAction("Index");
            }
            else
                return View();
        }
        public IActionResult Edit(int id)
        {
            return View(_guestService.GetGuestById(id));
        }
        public IActionResult Edited(Guest updatedGuest)
        {
            if (ModelState.IsValid)
            {
                _guestService.UpdateGuest(updatedGuest);
                return RedirectToAction("Index");
            }
            else
                return View();
        }

        public IActionResult Delete(int id)
        {
            return View(_guestService.GetGuestById(id));
        }

        public IActionResult Deleted(Guest deletedGuest)
        {
            int x = deletedGuest.GuestId;
            if (x != 0)
            {
                _guestService.DeleteGuest(x);
                return RedirectToAction(nameof(Index));
            }
            else
                return View();
        }
    }
}