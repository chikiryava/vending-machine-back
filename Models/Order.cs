﻿using System.Text.Json.Serialization;

namespace WendingApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }
        public List<OrderItem> Items { get; set; } = new();
    }
}
