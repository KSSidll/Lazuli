using System.ComponentModel.DataAnnotations;

namespace LazuliLibrary.Data.Models;

public class PostFilterModel
{
	public string? Phrase { get; set; }

	[Range(0, int.MaxValue)] public int? MinCharCount { get; set; }

	[Range(0, int.MaxValue)] public int? MaxCharCount { get; set; }
}