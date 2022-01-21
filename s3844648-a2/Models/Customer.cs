﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace s3844648_a2.Models;

public class Customer
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int CustomerID { get; set; }

    [StringLength(50)]
    public string Name { get; set; }

    [StringLength(11)]
    public string? TFN { get; set; }

    [StringLength(50)]
    public string? Address { get; set; }

    [JsonProperty("City")]
    [StringLength(40)]
    public string? Suburb { get; set; }

    [StringLength(3)]
    public string? State { get; set; }

    [StringLength(4)]
    public string? PostCode { get; set; }

    [StringLength(12)]
    //format
    public string? Mobile { get; set; }

    public virtual List<Account> Accounts { get; set; }

    [NotMapped]
    public virtual Login Login { get; set; }
}
