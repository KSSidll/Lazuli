using System.Text;
using Konscious.Security.Cryptography;

namespace LazuliLibrary.Utils;

public static class CipherUtility
{
	public static byte[] Encrypt(byte[] data, byte[] salt)
	{
		var argon = new Argon2id(data);

		argon.DegreeOfParallelism = 32;
		argon.MemorySize = 32768;
		argon.Iterations = 40;
		argon.Salt = salt;

		return argon.GetBytes(128);
	}

	public static byte[] Encrypt(string data, string salt)
	{
		return Encrypt(Encoding.ASCII.GetBytes(data), Encoding.ASCII.GetBytes(salt));
	}
}