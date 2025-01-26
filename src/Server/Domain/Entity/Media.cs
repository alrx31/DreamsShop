using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Media
    {
        [Key]
        public required Guid Id { get; set; }

        [StringLength(500)]
        public required string FileName { get; set; }

        [StringLength(10)]
        public required string FileExtension { get; set; }

        [StringLength(50)]
        public required int FileSize { get; set; }

        [StringLength(500)]
        public required string FilePath { get; set; }

        public required byte[] File { get; set; }
        
        
        public Dream DreamAsImage { get; set; } 
        public Dream DreamAsPreview { get; set; }

    }
}