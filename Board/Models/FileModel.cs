using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Execution;
using System.ComponentModel.DataAnnotations;

namespace Board.Models
{
    public class FileModel
    {
        [Key]
        public int Id { get; set; }
        public int NoticeId { get; set; }

        public string FileNames { get; set; }
        public string FilePath { get; set; }

    }
}

