namespace FlightTracker;

using System.Data.SQLite;
using System.Collections.Generic;

public class SQLiteFlightTrackerRepository : IFlightTrackerRepository
{
    private string _connectionString;
    private List<Flight> flights = new List<Flight>();
    private List<Ticket> tickets = new List<Ticket>();
    private List<User> users = new List<User>();

    // Создание таблицы рейсов
    private const string CreateTableQueryFlights = @"
        CREATE TABLE IF NOT EXISTS Flights (
            FlightId INT PRIMARY KEY,
            DepartureTime DATETIME NOT NULL,
            ArrivalTime DATETIME NOT NULL,
            DepartureCity TEXT NOT NULL,
            ArrivalCity TEXT NOT NULL,
            Airline TEXT NOT NULL,
            PlaneBrand TEXT NOT NULL
        )";

    // Создание таблицы билетов
    private const string CreateTableQueryTickets = @"
        CREATE TABLE IF NOT EXISTS Tickets (
            TicketId INT PRIMARY KEY,
            FlightId INT NOT NULL,
            UserId INT NOT NULL,
            TicketNumber INT NOT NULL,
            Seat TEXT NOT NULL,
            Price INT NOT NULL
        )";

    private const string CreateTableQueryUsers = @"
        CREATE TABLE IF NOT EXISTS Users (
            UserId INT PRIMARY KEY,
            FullName TEXT NOT NULL,
            Nationality TEXT NOT NULL
        )";

    public SQLiteFlightTrackerRepository(string connectionString){
        _connectionString = connectionString;
        InitializeDatabase();
        ReadDataFromDatabase();
    }

    private void ReadDataFromDatabase()
    {
        flights = GetAllFlights();
        tickets = GetAllTickets();
        users = GetAllUsers();
    }

    private void InitializeDatabase()
    {
        SQLiteConnection connection = new SQLiteConnection(_connectionString); 
        Console.WriteLine("База данных :  " + _connectionString + " создана");
        connection.Open();
        SQLiteCommand command1 = new SQLiteCommand(CreateTableQueryFlights, connection);
        command1.ExecuteNonQuery();
        SQLiteCommand command2 = new SQLiteCommand(CreateTableQueryTickets, connection);
        command2.ExecuteNonQuery();
        SQLiteCommand command3 = new SQLiteCommand(CreateTableQueryUsers, connection);
        command3.ExecuteNonQuery();
    }

    public List<Flight> GetAllFlights()
    {
        List<Flight> flights = new List<Flight>();
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Flights";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Flight flight = new Flight(
                            Convert.ToInt32(reader["FlightId"]), 
                            Convert.ToDateTime(reader["DepartureTime"]), 
                            Convert.ToDateTime(reader["ArrivalTime"]), 
                            reader["DepartureCity"].ToString(), 
                            reader["ArrivalCity"].ToString(), 
                            reader["Airline"].ToString(), 
                            reader["PlaneBrand"].ToString());
                        flights.Add(flight);
                    }
                }
            }
        }
        return flights;
    }

    public List<Flight> GetAllFlightsByParameter(string ParName, string ParValue)
    {
        List<Flight> flights = new List<Flight>();
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = $@"
            SELECT *
            FROM Flights
            WHERE {ParName} = @{ParName}";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue($"@{ParName}", ParValue);
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Flight flight = new Flight(
                            Convert.ToInt32(reader["FlightId"]), 
                            Convert.ToDateTime(reader["DepartureTime"]), 
                            Convert.ToDateTime(reader["ArrivalTime"]), 
                            reader["DepartureCity"].ToString(), 
                            reader["ArrivalCity"].ToString(), 
                            reader["Airline"].ToString(), 
                            reader["PlaneBrand"].ToString());
                        flights.Add(flight);
                    }
                }
            }
        }
        return flights;
    }

    public List<Flight> GetAllFlightsByDepartureCity(string DepartureCity)
    {
        List<Flight> LstFlights = GetAllFlightsByParameter("DepartureCity", DepartureCity);
        return LstFlights;
    }
    public List<Flight> GetAllFlightsByArrivalCity(string ArrivalCity)
    {
        List<Flight> LstFlights = GetAllFlightsByParameter("ArrivalCity", ArrivalCity);
        return LstFlights;
    }
    public List<Flight> GetAllFlightsByAirline(string Airline)
    {
        List<Flight> LstFlights = GetAllFlightsByParameter("Airline", Airline);
        return LstFlights;
    }
    public Flight GetFlightById(string FlightId)
    {
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = 
            @"
            SELECT *
            FROM Flights
            WHERE FlightId = @FlightId;
            ";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FlightId", FlightId);
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        Flight flight = new Flight(
                            Convert.ToInt32(reader["FlightId"]), 
                            Convert.ToDateTime(reader["DepartureTime"]), 
                            Convert.ToDateTime(reader["ArrivalTime"]), 
                            reader["DepartureCity"].ToString(), 
                            reader["ArrivalCity"].ToString(), 
                            reader["Airline"].ToString(), 
                            reader["PlaneBrand"].ToString()
                        );
                        return flight;
                    }
                    return null;
                }
            }
        }
    }
    
    public void UpdateAirline(string FlightId, string newAirline)
    {
        Flight flight = GetFlightById(FlightId);
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = 
            @"
            UPDATE Flights
            SET 
            FlightId = @FlightId, DepartureTime = @DepartureTime, 
            ArrivalTime = @ArrivalTime, DepartureCity = @DepartureCity, 
            ArrivalCity = @ArrivalCity, Airline = @Airline,
            PlaneBrand = @PlaneBrand
            WHERE FlightId = @FlightId;
            ";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FlightId", flight.FlightId);
                command.Parameters.AddWithValue("@DepartureTime", flight.DepartureTime);
                command.Parameters.AddWithValue("@ArrivalTime", flight.ArrivalTime);
                command.Parameters.AddWithValue("@DepartureCity", flight.DepartureCity);
                command.Parameters.AddWithValue("@ArrivalCity", flight.ArrivalCity);
                command.Parameters.AddWithValue("@Airline", newAirline);
                command.Parameters.AddWithValue("@PlaneBrand", flight.PlaneBrand);
                command.ExecuteNonQuery();
            }
        }
    }
    public void UpdatePlaneBrand(string FlightId, string newPlaneBrand)
    {
        Flight flight = GetFlightById(FlightId);
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = 
            @"
            UPDATE Flights
            SET 
            FlightId = @FlightId, DepartureTime = @DepartureTime, 
            ArrivalTime = @ArrivalTime, DepartureCity = @DepartureCity, 
            ArrivalCity = @ArrivalCity, Airline = @Airline,
            PlaneBrand = @PlaneBrand
            WHERE FlightId = @FlightId;
            ";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FlightId", flight.FlightId);
                command.Parameters.AddWithValue("@DepartureTime", flight.DepartureTime);
                command.Parameters.AddWithValue("@ArrivalTime", flight.ArrivalTime);
                command.Parameters.AddWithValue("@DepartureCity", flight.DepartureCity);
                command.Parameters.AddWithValue("@ArrivalCity", flight.ArrivalCity);
                command.Parameters.AddWithValue("@Airline", flight.Airline);
                command.Parameters.AddWithValue("@PlaneBrand", newPlaneBrand);
                command.ExecuteNonQuery();
            }
        }
    }
    
    public void AddFlight(Flight flight)
    {
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = @"INSERT INTO Flights (FlightId, DepartureTime, ArrivalTime, DepartureCity, ArrivalCity, Airline, PlaneBrand)
             VALUES (@FlightId, @DepartureTime, @ArrivalTime, @DepartureCity, @ArrivalCity, @Airline, @PlaneBrand)";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FlightId", flight.FlightId);
                command.Parameters.AddWithValue("@DepartureTime", flight.DepartureTime);
                command.Parameters.AddWithValue("@ArrivalTime", flight.ArrivalTime);
                command.Parameters.AddWithValue("@DepartureCity", flight.DepartureCity);
                command.Parameters.AddWithValue("@ArrivalCity", flight.ArrivalCity);
                command.Parameters.AddWithValue("@Airline", flight.Airline);
                command.Parameters.AddWithValue("@PlaneBrand", flight.PlaneBrand);
                command.ExecuteNonQuery();
            }
        }
    }
    public void DeleteFlight(string flightId)
    {
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = @"DELETE FROM Flights WHERE FlightId = @FlightId";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FlightId", flightId);
                command.ExecuteNonQuery();
            }
        }
    }





    public List<Ticket> GetAllTickets()
    {
        List<Ticket> tickets = new List<Ticket>();
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Tickets";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Ticket ticket = new Ticket(
                            Convert.ToInt32(reader["TicketId"]), 
                            Convert.ToInt32(reader["FlightId"]),
                            Convert.ToInt32(reader["UserId"]), 
                            Convert.ToInt32(reader["TicketNumber"]), 
                            reader["Seat"].ToString(), 
                            Convert.ToInt32(reader["Price"]));
                        tickets.Add(ticket);
                    }
                }
            }
        }
        return tickets;
    }

    public List<Ticket> GetAllTicketsByParameterTwoTables(string ParName, string ParValue)
    {
        List<Ticket> tickets = new List<Ticket>();
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = $@"
            SELECT *
            FROM Flights
            INNER JOIN Tickets ON Flights.FlightId = Tickets.FlightId
            WHERE {ParName} = @{ParName};
            ";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue($"@{ParName}", ParValue);
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Ticket ticket = new Ticket(
                            Convert.ToInt32(reader["TicketId"]), 
                            Convert.ToInt32(reader["FlightId"]),  
                            Convert.ToInt32(reader["UserId"]),
                            Convert.ToInt32(reader["TicketNumber"]),
                            reader["Seat"].ToString(),
                            Convert.ToInt32(reader["Price"])
                            );
                        tickets.Add(ticket);
                    }
                }
            }
        }
        return tickets;
    }

    public List<Ticket> GetAllTicketsByParameterOneTable(string ParName, string ParValue)
    {
        List<Ticket> tickets = new List<Ticket>();
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = $@"
            SELECT *
            FROM Tickets
            WHERE {ParName} = @{ParName};
            ";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue($"@{ParName}", ParValue);
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Ticket ticket = new Ticket(
                            Convert.ToInt32(reader["TicketId"]), 
                            Convert.ToInt32(reader["FlightId"]),  
                            Convert.ToInt32(reader["UserId"]),
                            Convert.ToInt32(reader["TicketNumber"]),
                            reader["Seat"].ToString(),
                            Convert.ToInt32(reader["Price"])
                            );
                        tickets.Add(ticket);
                    }
                }
            }
        }
        return tickets;
    }

    public List<Ticket> GetAllTicketsByDepartureCity(string DepartureCity)
    {
        List<Ticket> tickets = GetAllTicketsByParameterTwoTables("DepartureCity", DepartureCity);
        return tickets;
    }

    public List<Ticket> GetAllTicketsByArrivalCity(string ArrivalCity)
    {
        List<Ticket> tickets = GetAllTicketsByParameterTwoTables("ArrivalCity", ArrivalCity);
        return tickets;
    }
    public List<Ticket> GetAllTicketsByUserId(string UserId)
    {
        List<Ticket> tickets = GetAllTicketsByParameterOneTable("UserId", UserId);
        return tickets;
    }
    public List<Ticket> GetAllTicketsByFlightId(string FlightId)
    {
        List<Ticket> tickets = GetAllTicketsByParameterOneTable("FlightId", FlightId);
        return tickets;
    }
    public Ticket GetTicketById(string TicketId)
    {
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = 
            @"
            SELECT *
            FROM Tickets
            WHERE TicketId = @TicketId;
            ";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TicketId", TicketId);
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        Ticket ticket = new Ticket(
                            Convert.ToInt32(reader["TicketId"]), 
                            Convert.ToInt32(reader["FlightId"]),  
                            Convert.ToInt32(reader["UserId"]),
                            Convert.ToInt32(reader["TicketNumber"]),
                            reader["Seat"].ToString(),
                            Convert.ToInt32(reader["Price"])
                        );
                        return ticket;
                    }
                    return null;
                }
            }
        }
    }
    public void UpdateTicketPrice(string TicketId, int newPrice)
    {
        Ticket ticket = GetTicketById(TicketId);
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = 
            @"
            UPDATE Tickets
            SET 
            TicketId = @TicketId, FlightId = @FlightId, 
            UserId = @UserId, TicketNumber = @TicketNumber, 
            Seat = @Seat, Price = @Price
            WHERE TicketId = @TicketId;
            ";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TicketId", ticket.TicketId);
                command.Parameters.AddWithValue("@FlightId", ticket.FlightId);
                command.Parameters.AddWithValue("@UserId", ticket.UserId);
                command.Parameters.AddWithValue("@TicketNumber", ticket.TicketNumber);
                command.Parameters.AddWithValue("@Seat", ticket.Seat);
                command.Parameters.AddWithValue("@Price", newPrice);
                command.ExecuteNonQuery();
            }
        }
    }
    public void UpdateSeat(string TicketId, string newSeat)
    {
        Ticket ticket = GetTicketById(TicketId);
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = 
            @"
            UPDATE Tickets
            SET 
            TicketId = @TicketId, FlightId = @FlightId, 
            UserId = @UserId, TicketNumber = @TicketNumber, 
            Seat = @Seat, Price = @Price
            WHERE TicketId = @TicketId;
            ";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TicketId", ticket.TicketId);
                command.Parameters.AddWithValue("@FlightId", ticket.FlightId);
                command.Parameters.AddWithValue("@UserId", ticket.UserId);
                command.Parameters.AddWithValue("@TicketNumber", ticket.TicketNumber);
                command.Parameters.AddWithValue("@Seat", newSeat);
                command.Parameters.AddWithValue("@Price", ticket.Price);
                command.ExecuteNonQuery();
            }
        }
    }
    public void AddTicket(Ticket ticket)
    {
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = @"INSERT INTO Tickets (TicketId, FlightId, UserId, TicketNumber, Seat, Price)
             VALUES (@TicketId, @FlightId, @UserId, @TicketNumber, @Seat, @Price)";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TicketId", ticket.TicketId);
                command.Parameters.AddWithValue("@FlightId", ticket.FlightId);
                command.Parameters.AddWithValue("@UserId", ticket.UserId);
                command.Parameters.AddWithValue("@TicketNumber", ticket.TicketNumber);
                command.Parameters.AddWithValue("@Seat", ticket.Seat);
                command.Parameters.AddWithValue("@Price", ticket.Price);
                command.ExecuteNonQuery();
            }
        }
    }
    public void DeleteTicket(string ticketId)
    {
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = @"DELETE FROM Tickets WHERE TicketId = @TicketId";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TicketId", ticketId);
                command.ExecuteNonQuery();
            }
        }
    }




    public List<User> GetAllUsers()
    {
        List<User> users = new List<User>();
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Users";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        User user = new User(
                            Convert.ToInt32(reader["UserId"]), 
                            reader["FullName"].ToString(), 
                            reader["Nationality"].ToString());
                        users.Add(user);
                    }
                }
            }
        }
        return users;
    }

    public List<User> GetAllUsersByFlightId(string FlightId)
    {
        List<User> users = new List<User>();
        return users;
    }
    public User GetUserByUserId(string UserId)
    {
        User user = new User();
        return user;
    }
    public void AddUser(User user)
    {
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = @"INSERT INTO Users (UserId, FullName, Nationality)
             VALUES (@UserId, @FullName, @Nationality)";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", user.UserId);
                command.Parameters.AddWithValue("@FullName", user.FullName);
                command.Parameters.AddWithValue("@Nationality", user.Nationality);
                command.ExecuteNonQuery();
            }
        }
    }
    public void DeleteUser(string UserId)
    {
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = @"DELETE FROM Users WHERE UserId = @UserId";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", UserId);
                command.ExecuteNonQuery();
            }
        }
    }
}