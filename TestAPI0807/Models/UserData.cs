﻿namespace TestAPI0807.Models
{
    public class UserData
    {
        public long Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public int Gender { get; set; }
        public int Age { get; set; }
        public DateTime RegistorDate { get; set; }
        public string? Secret { get; set; }
    }
}
