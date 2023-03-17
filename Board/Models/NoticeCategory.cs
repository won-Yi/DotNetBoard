using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Board.Models
{
    public class NoticeCategory
    {
        public List<Notice>? Notices { get; set; }
        public SelectList? Categorys{ get; set; }
        public string? Category { get; set; }
        public string? SearchString { get; set; }
    }
}
