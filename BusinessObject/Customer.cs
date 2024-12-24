using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public string? Telephone { get; set; }

    public string Email { get; set; } = null!;

    public DateTime? CustomerBirthday { get; set; }

    public byte? CustomerStatus { get; set; }

    public string? Password { get; set; }

    public IEnumerable<RentingTransaction>? RentingTransactions { get; set; }

    public override string ToString()
    {
        return "Id: " + CustomerId + ", Name: " + CustomerName + ", Email: " + Email;
    }
}
