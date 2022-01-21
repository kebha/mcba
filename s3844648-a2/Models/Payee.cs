using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace s3844648_a2.Models;

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

    [StringLength(3)]
    public string State { get; set; }

    [StringLength(4)]
    public string PostCode { get; set; }

    [StringLength(14)]
    //format
    public string Phone { get; set; }
}