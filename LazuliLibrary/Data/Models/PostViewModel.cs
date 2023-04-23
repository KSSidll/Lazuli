namespace LazuliLibrary.Data.Models;

public class PostViewModel
{
	public int Id { get; set; }
	public int UserId { get; set; }
	public string? Username { get; set; }
	public string? Title { get; set; }
	public string? Body { get; set; }
	public List<CommentViewModel>? Comments { get; set; }
}