using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace s3844648_a2.Models;

public class Payee
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int PayeeID { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(50)]
    public string Address { get; set; }

    [Required]
    [StringLength(40)]
    public string Suburb { get; set; }

    [Required]
    [StringLength(3)]
    public string State { get; set; }

    [Required]
    [StringLength(4)]
    public string PostCode { get; set; }

    
    [StringLength(14)]
    public string Phone { get; set; }
}