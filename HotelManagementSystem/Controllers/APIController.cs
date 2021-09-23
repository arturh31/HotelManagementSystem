using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManagementSystem.Models;
using HotelManagementSystem.Models.ViewModels;
using HotelManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Controllers
{

    [Route("api")]
    public class APIController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IEmployeeService _employeeService;
        private readonly HMSdbContext _context;

        public APIController(IRoomService roomService, IEmployeeService employeeService, HMSdbContext context)
        {
            _roomService = roomService;
            _employeeService = employeeService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("rooms")]
        [HttpGet]
        public IActionResult GetAllRooms(List<Room> rooms)
        {
            return Ok(_roomService.GetAllRooms());
        }

        [Route("employee")]
        [HttpGet]
        public IActionResult GetEmployeeByMobileAppId(string appID)
        {
            var employees = _employeeService.GetAllEmployes();

            APIEmployeeViewModel model = new APIEmployeeViewModel();

            foreach (var item in employees)
            {
                if (item.MobileAppId == appID && appID != null)
                {
                    model.FirstName = item.FirstName;
                    model.LastName = item.LastName;
                    model.MobileAppId = item.MobileAppId;

                    return Ok(model);
                }
            }

            return BadRequest();


        }
        [Route("RoomStatus")]
        [HttpPost]
        public IActionResult UpdateRoomStatus([FromBody]Room room)
        {
            var rooms = _roomService.GetAllRooms();

            foreach(var item in rooms)
            {
                if(item.RoomNumber == room.RoomNumber && room.RoomNumber != 0)
                {
                    var roomToUpdate = _roomService.GetRoomById(room.RoomId);
                    roomToUpdate.FkRoomStatusId = room.FkRoomStatusId;
                    _roomService.Update(roomToUpdate);
                    return Ok(room);
                }
            }

            return BadRequest();
        }
    }
}