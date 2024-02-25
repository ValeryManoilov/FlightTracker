namespace FlightTracker;

using System;
using System.ComponentModel.DataAnnotations;
public class Flight
{

    public int FlightId { get; set; }   // Id рейса

    public DateTime DepartureTime { get; set; } // Время вылета

    public DateTime ArrivalTime { get; set; } // Время прилета

    public string DepartureCity { get; set; } // Город вылета
    
    public string ArrivalCity { get; set; } // Город прилета

    public string Airline { get; set; } // Авиакомпания

    public string PlaneBrand { get; set; } // Марка самолета


    public Flight() {}

    public Flight(int flightId, DateTime departureTime, DateTime arrivalTime, string departureCity, string arrivalCity, string airline, string planeBrand)
    {
        FlightId = flightId;
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
        DepartureCity = departureCity;
        ArrivalCity = arrivalCity;
        Airline = airline;
        PlaneBrand = planeBrand;
    }
}