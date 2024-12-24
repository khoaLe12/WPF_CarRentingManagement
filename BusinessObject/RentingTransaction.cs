using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject;

public partial class RentingTransaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RentingTransationId { get; set; }

    public DateTime? RentingDate { get; set; }

    public decimal? TotalPrice { get; set; }


    public byte? RentingStatus { get; set; }

    public Customer? Customer { get; set; }
    public int CustomerId { get; set; }

    public virtual ICollection<RentingDetail> RentingDetails { get; set; } = new List<RentingDetail>();
}
