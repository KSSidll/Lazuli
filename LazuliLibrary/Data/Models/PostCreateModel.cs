using System.ComponentModel.DataAnnotations;

namespace LazuliLibrary.Data.Models;

public class PostCreateModel
{
	[Required] [MinLength(1)] public string? Title { get; set; }
	[Required] [MinLength(1)] public string? Body { get; set; }
}