using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using HotelManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using ReflectionIT.Mvc.Paging;
using X.PagedList;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        private readonly IRoomService _service;

        public RoomController(IRoomService service)
        {
            _service = service;
        }
        
        public IActionResult Search()
        {            
            return View();
        }
        [HttpPost]
        public IActionResult SearchById(Room room)
        {
            var x = Convert.ToInt32(Request.Form["RoomNumber"]);
            
            var roomList = _service.GetAllRooms();
            foreach(var item in roomList)
            {
                if(x == item.RoomNumber)
                {
                    room.RoomId = item.RoomId;
                }
            }

            if(room.RoomId == 0)
            {
                return RedirectToAction("Search");
            }
            else
            {
                return Redirect("Details/" + room.RoomId);
            }           
            
        }
        public IActionResult Index(int? page)
        {
            var allRooms = _service.GetAllRooms();
            var pageNumber = page ?? 1;
            var pageSize = 10;
   
            return View(allRooms.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult Details(int id)
        {
            var room = _service.GetRoomById(id);
            return View(room);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create(int id)
        {
            return View(_service.GetRoomById(id));
        }
        public IActionResult Created(Room room)
        {
            if (ModelState.IsValid)
            {
                _service.Create(room);
                return RedirectToAction("Index");
            }
            else
                return View();
        }
        public IActionResult Edit(int id)
        {
            return View(_service.GetRoomById(id));
        }


        public IActionResult Edited(Room room)
        {
            if (ModelState.IsValid)
            {
                _service.Update(room);
                return RedirectToAction("Index");
            }
            else
            return View();
        }
        public IActionResult Delete(int id)
        {          
            return View(_service.GetRoomById(id));
        }
        public IActionResult Deleted(Room room)
        {
             int x;           
             x = room.RoomId;
             _service.Delete(x);
             return RedirectToAction("Index");           
        }




    }
}