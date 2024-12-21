using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Media
    {
        [Key]
        public required Guid Id { get; set; }

        [StringLength(500)]
        public required string File_Name { get; set; }

        [StringLength(10)]
        public required string File_Extension { get; set; }

        [StringLength(50)]
        public required string File_Size { get; set; }

        [StringLength(500)]
        public required string File_Path { get; set; }

        public required byte[] File { get; set; }
        
        
        
        // Связь один к одному с Dream для Image_Media
        public Dream DreamAsImage { get; set; } 
        
        // Связь один к одному с Dream для Preview_Media
        public Dream DreamAsPreview { get; set; }

    }
}