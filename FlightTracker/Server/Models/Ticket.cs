namespace FlightTracker;

using System;
using System.ComponentModel.DataAnnotations;

public class Ticket
{

    public int TicketId { get; set; } // Id билета

    public int FlightId { get; set; } // Id рейса

    public int UserId { get; set; } // Id пользователя

    public int TicketNumber { get; set; } // Номер билета

    public string Seat { get; set; } // Место в самолете

    public int Price { get; set; } // Цена билета

    public Ticket() {}

    public Ticket(int ticketId, int flightId, int userId, int ticketNumber, string seat, int price)
    {
        TicketId = ticketId;
        FlightId = flightId;
        UserId = userId;
        TicketNumber = ticketNumber;
        Seat = seat;
        Price = price;
    }
}