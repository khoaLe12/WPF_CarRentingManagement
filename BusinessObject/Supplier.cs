﻿using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string SupplierName { get; set; } = null!;

    public string? SupplierDescription { get; set; }

    public string? SupplierAddress { get; set; }

    public virtual ICollection<CarInformation> CarInformations { get; set; } = new List<CarInformation>();

    public override string ToString()
    {
        return "Id: " + SupplierId + ", Name: " +SupplierName + ", Address: " + SupplierAddress;
    }
}
