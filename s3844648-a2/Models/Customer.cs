using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace s3844648_a2.Models;

public enum State
{
    NSW = 1,
    QLD = 2,
    SA  = 3,
    TAS = 4,
    VIC = 4,
    WA  = 5
}

public class Customer
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Range(4, 4)]
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
    public State? State { get; set; }

    [Range(4, 4)]
    [StringLength(4)]
    public string? PostCode { get; set; }

    [StringLength(12)]
    [DisplayFormat(DataFormatString = "{0;04## ### ###")]
    public string? Mobile { get; set; }

    public virtual List<Account> Accounts { get; set; }

    [NotMapped]
    public virtual Login Login { get; set; }
}
