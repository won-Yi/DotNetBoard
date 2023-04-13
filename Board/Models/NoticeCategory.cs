using Board.Models;
using Castle.Components.DictionaryAdapter;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace Board.Models {

    [Keyless]
    public class NoticeCategory
    {
        public List<Notice>? Notices { get; set; }
        public SelectList? Categorys { get; set; }
        public string? Category { get; set; }
        public string? SearchString { get; set; }
    }

}
