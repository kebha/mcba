using System.ComponentModel.DataAnnotations;

namespace AdminPortalWeb.Models;
public class SelectPeriodModel
{
    public int AccountID { get; set; }

    [Display(Name = "Start Date")]
    public DateTime? StartDate { get; set; }

    [Display(Name = "End Date")]
    public DateTime? EndDate { get; set; }
}
