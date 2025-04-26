using System.ComponentModel.DataAnnotations;

namespace Category.API.Models
{
    public class CategoryItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Type: 1 = Income, 2 = Expense
        [Required]
        public int Type { get; set; }

        public string? Icon { get; set; }

        // For system default categories, UserId will be null
        public string? UserId { get; set; }

        // System categories cannot be modified by users
        public bool IsSystem { get; set; } = false;
    }

    public class CategoryCreateModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public int Type { get; set; }

        public string? Icon { get; set; }
    }

    public class CategoryUpdateModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Icon { get; set; }
    }
}