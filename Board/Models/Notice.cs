using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Execution;
using System.ComponentModel.DataAnnotations;

namespace Board.Models
{
    public class Notice
    {
        public int Id { get; set; }
        public int Views_Number { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }

        public string UserName { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UpdateDate { get; set; }
        
        public string Category { get; set; }

        public string? fileAttachMent { get; set; }

        public string? FileName { get; set; }

        public byte[]? fileContents { get; set; }


    }
}
