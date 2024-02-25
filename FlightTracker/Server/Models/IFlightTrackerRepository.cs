namespace FlightTracker;

using System.Data.SQLite; 
using System.Collections.Generic; 

public interface IFlightTrackerRepository
{ 
    List<Flight> GetAllFlights();
    List<Flight> GetAllFlightsByDepartureCity(string DepartureCity);
    List<Flight> GetAllFlightsByArrivalCity(string ArrivalCity);
    List<Flight> GetAllFlightsByAirline(string Airline);
    Flight GetFlightById(string FlightId);
    void UpdateAirline(string FlightId, string newAirline);
    void UpdatePlaneBrand(string FlightId, string newPlaneBrand);
    void AddFlight(Flight flight);
    void DeleteFlight(string flightId);

    List<Ticket> GetAllTickets();
    List<Ticket> GetAllTicketsByDepartureCity(string DepartureCity);
    List<Ticket> GetAllTicketsByArrivalCity(string ArrivalCity);
    List<Ticket> GetAllTicketsByUserId(string UserId);
    List<Ticket> GetAllTicketsByFlightId(string FlightId);
    Ticket GetTicketById(string TicketId);
    void UpdateTicketPrice(string TicketId, int newPrice);
    void UpdateSeat(string TicketId, string newSeat);
    void AddTicket(Ticket ticket);
    void DeleteTicket(string ticketId);

    List<User> GetAllUsers();
    List<User> GetAllUsersByFlightId(string FlightId);
    User GetUserByUserId(string UserId);
    void AddUser(User user);
    void DeleteUser(string UserId);
}
