namespace FlightTracker;

using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.IO; 
using System.Net.Http;
using System.Text; 
using System.Threading.Tasks;
using System.Collections.Generic;


[ApiController]
public class StoreController : ControllerBase
{
    
    private readonly IFlightTrackerRepository _flightTrackerRepository;

    public StoreController(IFlightTrackerRepository flightTrackerRepository)
    {
        _flightTrackerRepository = flightTrackerRepository;
    }

        [HttpGet]
        [Route("/flights/show")]
        public IActionResult ShowFlights()
        {
            return Ok(_flightTrackerRepository.GetAllFlights());
        }

        [HttpGet]
        [Route("/flights/showflightsbydepcity")]
        public IActionResult ShowFlightsByDepCity(string DepartureCity)
        {
            return Ok(_flightTrackerRepository.GetAllFlightsByDepartureCity(DepartureCity));
        }

        [HttpGet]
        [Route("/flights/showflightsbyarrcity")]
        public IActionResult ShowFlightsByArrCity(string ArrivalCity)
        {
            return Ok(_flightTrackerRepository.GetAllFlightsByArrivalCity(ArrivalCity));
        }

        [HttpGet]
        [Route("/flights/showflightsbyairline")]
        public IActionResult ShowFlightsByAirline(string Airline)
        {
            return Ok(_flightTrackerRepository.GetAllFlightsByAirline(Airline));
        }

        [HttpGet]
        [Route("/flights/showflightbyid")]
        public IActionResult ShowFlightById(string FlightId)
        {
            return Ok(_flightTrackerRepository.GetFlightById(FlightId));
        }

        [HttpPost]
        [Route("/flights/add")]
        public IActionResult AddFlight([FromBody] Flight flight)
        {
            _flightTrackerRepository.AddFlight(flight);
            return Ok(_flightTrackerRepository.GetAllFlights());
        }

        [HttpPost]
        [Route("/flights/updateairline")]
        public IActionResult UpdateAirline(string flightId, string newAirline)
        {
            _flightTrackerRepository.UpdateAirline(flightId, newAirline);
            return Ok(_flightTrackerRepository.GetFlightById(flightId));
        }

        [HttpPost]
        [Route("/flights/updateplanebrand")]
        public IActionResult UpdatePlaneBrand(string flightId, string newPlaneBrand)
        {
            _flightTrackerRepository.UpdatePlaneBrand(flightId, newPlaneBrand);
            return Ok(_flightTrackerRepository.GetFlightById(flightId));
        }

        [HttpPost]
        [Route("/flights/delete")]
        public IActionResult DeleteFlight(string flightId)
        {
            _flightTrackerRepository.DeleteFlight(flightId);
            return Ok(_flightTrackerRepository.GetAllFlights());
        }




        [HttpGet]
        [Route("/tickets/show")]
        public IActionResult ShowTickets()
        {
            return Ok(_flightTrackerRepository.GetAllTickets());
        }

        [HttpGet]
        [Route("/tickets/showticketsbydepcity")]
        public IActionResult ShowTicketsByDepCity(string DepartureCity)
        {
            return Ok(_flightTrackerRepository.GetAllTicketsByDepartureCity(DepartureCity));
        }

        [HttpGet]
        [Route("/tickets/showticketsbyarrcity")]
        public IActionResult ShowTicketsByArrCity(string ArrivalCity)
        {
            return Ok(_flightTrackerRepository.GetAllTicketsByArrivalCity(ArrivalCity));
        }

        [HttpGet]
        [Route("/tickets/showticketsbyuserid")]
        public IActionResult ShowTicketsByUserId(string UserId)
        {
            return Ok(_flightTrackerRepository.GetAllTicketsByUserId(UserId));
        }

        [HttpGet]
        [Route("/tickets/showticketsbyflightid")]
        public IActionResult ShowTicketsByFlightId(string FlightId)
        {
            return Ok(_flightTrackerRepository.GetAllTicketsByFlightId(FlightId));
        }

        [HttpGet]
        [Route("/tickets/showticketbyid")]
        public IActionResult ShowTicketById(string TicketId)
        {
            return Ok(_flightTrackerRepository.GetTicketById(TicketId));
        }

        [HttpPost]
        [Route("/tickets/updateseat")]
        public IActionResult UpdateSeat(string ticketId, string newSeat)
        {
            _flightTrackerRepository.UpdateSeat(ticketId, newSeat);
            return Ok(_flightTrackerRepository.GetTicketById(ticketId));
        }

        [HttpPost]
        [Route("/tickets/updateticketprice")]
        public IActionResult UpdateTicketPrice(string ticketId, int newPrice)
        {
            _flightTrackerRepository.UpdateTicketPrice(ticketId, newPrice);
            return Ok(_flightTrackerRepository.GetTicketById(ticketId));
        }


        [HttpPost]
        [Route("/tickets/add")]
        public IActionResult AddTicket([FromBody] Ticket ticket)
        {
            _flightTrackerRepository.AddTicket(ticket);
            return Ok(_flightTrackerRepository.GetAllTickets());
        }

        [HttpPost]
        [Route("/tickets/delete")]
        public IActionResult DeleteTicket(string ticketId)
        {
            _flightTrackerRepository.DeleteTicket(ticketId);
            return Ok(_flightTrackerRepository.GetAllTickets());
        }




        [HttpGet]
        [Route("/users/show")]
        public IActionResult ShowUsers()
        {
            return Ok(_flightTrackerRepository.GetAllUsers());
        }

        [HttpPost]
        [Route("/users/add")]
        public IActionResult AddUser([FromBody] User user)
        {
            _flightTrackerRepository.AddUser(user);
            return Ok(_flightTrackerRepository.GetAllUsers());
        }

        [HttpPost]
        [Route("/users/delete")]
        public IActionResult DeleteUser(string userId)
        {
            _flightTrackerRepository.DeleteTicket(userId);
            return Ok(_flightTrackerRepository.GetAllUsers());
        }
      

}