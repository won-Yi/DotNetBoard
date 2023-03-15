using Board.Models;

public class NoticeDto
{
    public int Id { get; set; }
    public string Title { get; set; }

    public string Content { get; set; }

    public string UserName { get; set; }

    public DateTime UpdateDate { get; set; }

    public List<Comments> comments { get; set; }
}