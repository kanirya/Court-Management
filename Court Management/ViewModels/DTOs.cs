using Court_Management.Areas.Identity.Data;
using Court_Management.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Court_Management.ViewModels
{
    public class CaseDTO
    {
     
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string AssignedToId { get; set; }
        public CaseStatus Status { get; set; } = CaseStatus.Submitted;
    }

}
