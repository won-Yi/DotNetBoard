using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Board.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string UserNickName { get; set; }

        public string Email { get; set; }
        public string HashPassword { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UpdateDate { get; set; }
    }
}
