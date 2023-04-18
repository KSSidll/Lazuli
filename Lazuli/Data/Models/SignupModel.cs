using System.ComponentModel.DataAnnotations;

namespace Lazuli.Data.Models;

public class SignupModel
{
	[Required] [MinLength(1)] public string? Login { get; set; }

	[Required] [MinLength(1)] public string? Password { get; set; }

	[Required] [Range(1, int.MaxValue)] public int BoundToUserId { get; set; }
}