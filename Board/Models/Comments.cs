using Microsoft.Build.Execution;
using System.ComponentModel.DataAnnotations;

namespace Board.Models
{
    public class Comments
    {
        [Key]
        public int Id { get; set; }

        public int Notice_id { get; set; }
        public string UserName { get; set; }

        public string Comment { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UpdateTime { get; set; }
    }
}
