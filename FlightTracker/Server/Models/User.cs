namespace FlightTracker;

using System;
using System.ComponentModel.DataAnnotations;

public class User
{

    public int UserId { get; set; } // Id пользователя

    public string FullName { get; set; } // Полное имя

    public string Nationality { get; set; } // Гражданство


    public User() {}

    public User(int userId, string fullName, string nationality)
    {
        UserId = userId;
        FullName = fullName;
        Nationality = nationality;
    }
}