using System.Text;
using LazuliLibrary.Utils;

namespace LazuliTest;

public class EncryptTest
{
	[Fact]
	public void TestEncryptConsistency()
	{
		var data = Encoding.ASCII.GetBytes("TESTDATA");
		var data2 = Encoding.ASCII.GetBytes("TESTDATA2");

		var salt = Encoding.ASCII.GetBytes("TESTSALT");
		var salt2 = Encoding.ASCII.GetBytes("TESTSALT2");

		var result1 = CipherUtility.Encrypt(data, salt);
		var result2 = CipherUtility.Encrypt(data2, salt2);
		var result3 = CipherUtility.Encrypt(data2, salt);
		var result4 = CipherUtility.Encrypt(data, salt2);

		Assert.NotEqual(result1, result2);
		Assert.NotEqual(result1, result3);
		Assert.NotEqual(result1, result4);

		Assert.NotEqual(result2, result1);
		Assert.NotEqual(result2, result3);
		Assert.NotEqual(result2, result4);

		Assert.NotEqual(result3, result1);
		Assert.NotEqual(result3, result2);
		Assert.NotEqual(result3, result4);

		Assert.NotEqual(result4, result1);
		Assert.NotEqual(result4, result2);
		Assert.NotEqual(result4, result3);

		Assert.Equal(result1, CipherUtility.Encrypt(data, salt));
		Assert.Equal(result2, CipherUtility.Encrypt(data2, salt2));
		Assert.Equal(result3, CipherUtility.Encrypt(data2, salt));
		Assert.Equal(result4, CipherUtility.Encrypt(data, salt2));

		Assert.Equal(result1, CipherUtility.Encrypt("TESTDATA", "TESTSALT"));
	}
}