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
    VIC = 5,
    WA  = 6
}

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
    public State? State { get; set; }

    [JsonProperty("PostCode")]
    public int? Postcode { get; set; }

    [StringLength(12)]
    [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong mobile")]
    public string? Mobile { get; set; }

    public virtual List<Account> Accounts { get; set; }

    [NotMapped]
    public virtual Login Login { get; set; }
}
