using Court_Management.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Court_Management.Models
{
    public class CourtModels
    {
    }
    public enum CaseStatus
    {
        Submitted,
        InReview,
        Approved,
        Rejected,
        Closed
    }



    public class Case
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public CaseStatus Status { get; set; } = CaseStatus.Submitted;

        [Required]
        public string SubmittedById { get; set; }
        [ForeignKey(nameof(SubmittedById))]
        public ApplicationUser SubmittedBy { get; set; }

        [Required]
        public string AssignedToId { get; set; }
        [ForeignKey(nameof(AssignedToId))]
        public ApplicationUser AssignedTo { get; set; }

        public ICollection<CaseComment> Comments { get; set; }
    }

    public class CaseComment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime PostedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int CaseId { get; set; }
        public Case Case { get; set; }

        [Required]
        public string AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public ApplicationUser Author { get; set; }
    }
}
