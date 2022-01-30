using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AdminPortalWeb.Models;

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

    public State? State { get; set; }

    [JsonProperty("PostCode")]
    [RegularExpression(@"^(\d{4})$", ErrorMessage = "Postcode must be 4-digits")]
    public int? Postcode { get; set; }

    [StringLength(12)]
    [RegularExpression(@"^(^04([\d]{2}) [\d]{3} [\d]{3})$", ErrorMessage = "Must be of the format: 04XX XXX XXX")]
    public string? Mobile { get; set; }

    public virtual List<Account> Accounts { get; set; }

    public virtual Login Login { get; set; }
}
