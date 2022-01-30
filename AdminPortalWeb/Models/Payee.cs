using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPortalWeb.Models;

public class Payee
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PayeeID { get; set; }

    [StringLength(50)]
    public string Name { get; set; }

    [StringLength(50)]
    public string Address { get; set; }

    [StringLength(40)]
    public string Suburb { get; set; }

    public State? State { get; set; }

    [RegularExpression(@"^(\d{4})$", ErrorMessage = "Postcode must be 4-digits")]
    public int Postcode { get; set; }

    [StringLength(14)]
    [RegularExpression(@"^(^\(0[\d]{1}\) [\d]{4} [\d]{4})$", ErrorMessage = "Must be of the format: (0X) XXXX XXXX")]
    public string Phone { get; set; }
}