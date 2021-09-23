using HotelManagementSystem.Models.ViewModels;
using HotelManagementSystem.Models;
using System.Collections.Generic;

namespace HotelManagementSystem.Services
{
	public interface IReservationService
	{
		List<RoomReservationViewModel> GetReservations();
		List<RoomReservationViewModel> GetReservationsById(int id);
		RoomReservationViewModel GetReservationById(int id);
		Reservation ReservationCreator(RoomReservationViewModel viewModel);
		bool DateValidator(RoomReservationViewModel viewModel);
		public Reservation Update(RoomReservationViewModel viewModel, Reservation reservation);
		void Delete(int id);
	}
}