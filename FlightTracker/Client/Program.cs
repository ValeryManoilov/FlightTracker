using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace Client
{


    [Serializable]
    public class Ticket
    {

        public int ticketId { get; set; } // Id билета

        public int flightId { get; set; } // Id рейса

        public int userId { get; set; } // Id пользователя

        public int ticketNumber { get; set; } // Номер билета

        public string seat { get; set; } // Место в самолете

        public int price { get; set; } // Цена билета

        public Ticket() {}

        public Ticket(int TicketId, int FlightId, int UserId, int TicketNumber, string Seat, int Price)
        {
            ticketId = TicketId;
            flightId = FlightId;
            userId = UserId;
            ticketNumber = TicketNumber;
            seat = Seat;
            price = Price;
        }
    }

    [Serializable]
    public class Flight
    {

        public int flightId { get; set; }   // Id рейса

        public DateTime departureTime { get; set; } // Время вылета

        public DateTime arrivalTime { get; set; } // Время прилета

        public string departureCity { get; set; } // Город вылета
        
        public string arrivalCity { get; set; } // Город прилета

        public string airline { get; set; } // Авиакомпания

        public string planeBrand { get; set; } // Марка самолета


        public Flight() {}

        public Flight(int FlightId, DateTime DepartureTime, DateTime ArrivalTime, string DepartureCity, string ArrivalCity, string Airline, string PlaneBrand)
        {
            flightId = FlightId;
            departureTime = DepartureTime;
            arrivalTime = ArrivalTime;
            departureCity = DepartureCity;
            arrivalCity = ArrivalCity;
            airline = Airline;
            planeBrand = PlaneBrand;
        }
    }

    public class Program
    {
        private const string BaseUrl = "http://localhost";
        private const string Port = "5150";

        private const string ShowFlightsMethod = "/flights/show";
        private const string ShowFlightsByDepCityMethod = "/flights/showflightsbydepcity";
        private const string ShowFlightsByArrCityMethod = "/flights/showflightsbyarrcity";
        private const string ShowFlightsByAirlineMethod = "/flights/showflightsbyairline";
        private const string ShowFlightByIdMethod = "/flights/showflightbyid";
        private const string AddFlightMethod = "/flights/add";
        private const string DeleteFlightMethod = "/flights/delete";

        private const string ShowTicketsMethod = "/tickets/show";
        private const string ShowTicketsByDepCityMethod = "/tickets/showticketsbydepcity";
        private const string ShowTicketsByArrCityMethod = "/tickets/showticketsbyarrcity";
        private const string ShowTicketsByUserIdMethod = "/tickets/showticketsbyuserid";
        private const string ShowTicketsByFlightIdMethod = "/tickets/showticketsbyflightid";
        private const string ShowTicketByIdMethod = "/tickets/showticketbyid";
        private const string AddTicketMethod = "/tickets/add";
        private const string DeleteTicketMethod = "/tickets/delete";


        private static bool IsAuthorized = false;
        private static readonly HttpClient Client = new HttpClient();

        // Увидеть все рейсы
        private static void DisplayFlights()
        {
            var url = $"{BaseUrl}:{Port}{ShowFlightsMethod}";

            var response = Client.GetAsync(url).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result; 
            var flights = JsonSerializer.Deserialize<List<Flight>>(responseContent);


            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("| Id                   | Departure Time       | Arrival Time         | Departure City       | Arrival City         | AirLine              | Plane Brand          |");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");

            foreach (var flight in flights)
            {
                Console.WriteLine($"| {flight.flightId, -20} | {flight.departureTime, -20} | {flight.arrivalTime, -20} | {flight.departureCity, -20} | {flight.arrivalCity, -20} | {flight.airline, -20} | {flight.planeBrand, -20} |"); 
            }
            
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
        }

        // Увидеть все билеты
        private static void DisplayTickets()
        {
            var url = $"{BaseUrl}:{Port}{ShowTicketsMethod}";

            var response = Client.GetAsync(url).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result; 
            var tickets = JsonSerializer.Deserialize<List<Ticket>>(responseContent);


            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("| Ticket Id            | Flight Id            | User Id              | Ticket Number        | Seat                 | Price               |");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------");

            foreach (var ticket in tickets)
            {
                Console.WriteLine($"| {ticket.ticketId, -20} | {ticket.flightId, -20} | {ticket.userId, -20} | {ticket.ticketNumber, -20} | {ticket.seat, -20} |{ticket.price, -20} |"); 
            }

            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------");
        }

        private static void DisplayFlightsByParameter(string Method, string ParName)
        {
            string Parameter = Console.ReadLine();
            var url = $"{BaseUrl}:{Port}{Method}?{ParName}={Parameter}";
            var response = Client.GetAsync(url).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var flights = JsonSerializer.Deserialize<List<Flight>>(responseContent);

            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("| Id                   | Departure Time       | Arrival Time         | Departure City       | Arrival City         | AirLine              | Plane Brand          |");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");

            foreach (var flight in flights)
            {
                Console.WriteLine($"| {flight.flightId, -20} | {flight.departureTime, -20} | {flight.arrivalTime, -20} | {flight.departureCity, -20} | {flight.arrivalCity, -20} | {flight.airline, -20} | {flight.planeBrand, -20} |"); 
            }

            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
        }
        
        private static void DisplayTicketsByParameter(string Method, string ParName)
        {
            string Parameter = Console.ReadLine();
            var url = $"{BaseUrl}:{Port}{Method}?{ParName}={Parameter}";
            var response = Client.GetAsync(url).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result; 
            var tickets = JsonSerializer.Deserialize<List<Ticket>>(responseContent);

            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("| Ticket Id            | Flight Id            | User Id              | Ticket Number        | Seat                 | Price               |");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------");

            foreach (var ticket in tickets)
            {
                Console.WriteLine($"| {ticket.ticketId, -20} | {ticket.flightId, -20} | {ticket.userId, -20} | {ticket.ticketNumber, -20} | {ticket.seat, -20} |{ticket.price, -20} |"); 
            }

            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------");
        }

        


        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("Выберите опцию:");
                Console.WriteLine("1. Авторизация");
                Console.WriteLine("2. Взаимодействие с рейсами");
                Console.WriteLine("3. Взаимодействие с билетами");
                Console.WriteLine("4. Выйти");
                Console.WriteLine("Введите ваш выбор: ");

                var FirstChoice = Console.ReadLine();

                switch (FirstChoice)
                {
                    case "1":
                        Console.WriteLine("Авторизация");
                        break;
                    case "2":
                        Console.WriteLine("Выберите опцию:");
                        Console.WriteLine("1. Увидеть все рейсы");
                        Console.WriteLine("-- Увидеть все рейсы по... --");
                        Console.WriteLine("2. Городу вылета");
                        Console.WriteLine("3. Городу прилета");
                        Console.WriteLine("4. Авиакомпании");
                        Console.WriteLine("-- Изменение параметров рейса --");
                        Console.WriteLine("5. Изменить авиакомпанию по Id");
                        Console.WriteLine("5. Изменить модель самолета по Id");
                        Console.WriteLine("7. Создать новый рейс");
                        Console.WriteLine("8. Удалить рейс по Id");
                        Console.WriteLine("9. Выйти");
                        Console.Write("Введите ваш выбор: ");

                        var ChoiceFlightMethod = Console.ReadLine();
                        switch (ChoiceFlightMethod)
                        {
                            case "1":
                                DisplayFlights();
                                break;
                            case "2":
                                Console.WriteLine("Введите город вылета:");
                                DisplayFlightsByParameter(ShowFlightsByDepCityMethod, "DepartureCity");
                                break;
                            case "3":
                                Console.WriteLine("Введите город прилета:");
                                DisplayFlightsByParameter(ShowFlightsByArrCityMethod, "ArriveCity");
                                break;
                            case "4":
                                Console.WriteLine("Введите авиакомпанию:");
                                DisplayFlightsByParameter(ShowFlightsByAirlineMethod, "Airline");
                                break;
                            case "5":
                                Console.WriteLine("Реализован в сваггере");
                                break;
                            case "6":
                                Console.WriteLine("Реализован в сваггере");
                                break;
                            case "7":
                                Console.WriteLine("Реализован в сваггере");
                                break;
                            case "8":
                                Console.WriteLine("Реализован в сваггере");
                                break;
                            case "9":
                                return;
                        }
                        break;
                    case "3":
                        Console.WriteLine("Выберите опцию:");
                        Console.WriteLine("1. Увидеть все билеты");
                        Console.WriteLine("-- Увидеть все билеты по... --");
                        Console.WriteLine("2. Городу вылета");
                        Console.WriteLine("3. Городу прилета");
                        Console.WriteLine("4. Покупателю (все билеты покупателя)");
                        Console.WriteLine("5. Рейсу (все билеты на рейс)");
                        Console.WriteLine("-- Изменение параметров билета --");
                        Console.WriteLine("6. Изменить цену по Id");
                        Console.WriteLine("7. Изменить посадочное место по Id");
                        Console.WriteLine("8. Создать новый билет");
                        Console.WriteLine("9. Удалить билет по Id");
                        Console.WriteLine("10. Выйти");
                        Console.Write("Введите ваш выбор: ");

                        var ChoiceTicketMethod = Console.ReadLine();
                        switch (ChoiceTicketMethod)
                        {
                            case "1":
                                DisplayTickets();
                                break;
                            case "2":
                                Console.WriteLine("Введите город вылета:");
                                DisplayTicketsByParameter(ShowTicketsByDepCityMethod, "DepartureCity");
                                break;
                            case "3":
                                Console.WriteLine("Введите город прилета:");
                                DisplayTicketsByParameter(ShowTicketsByArrCityMethod, "ArriveCity");
                                break;
                            case "4":
                                Console.WriteLine("Введите id покупателя:");
                                DisplayTicketsByParameter(ShowTicketsByUserIdMethod, "UserId");
                                break;
                            case "5":
                                Console.WriteLine("Введите id рейса:");
                                DisplayTicketsByParameter(ShowTicketsByFlightIdMethod, "FlightId");
                                break;
                            case "6":
                                Console.WriteLine("Реализован в сваггере");
                                break;
                            case "7":
                                Console.WriteLine("Реализован в сваггере");
                                break;
                            case "8":
                                Console.WriteLine("Реализован в сваггере");
                                break;
                            case "9":
                                Console.WriteLine("Реализован в сваггере");
                                break;
                            case "10":
                                return;
                        }
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }
    }
}
