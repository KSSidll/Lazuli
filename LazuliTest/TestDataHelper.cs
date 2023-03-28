using Lazuli.Data.Database;
using LazuliLibrary.Utils;

namespace LazuliTest.TestDataHelper;

public static class TestDataHelper
{
    public static List<User> GetFakeDatabaseUserList()
    {
        return new List<User>()
        {
            new User
            {
                Id = 1,
                Login = "John Doe",
                Password = CipherUtility.Encrypt("Johny", "John Doe"),
                BoundToUserId = 1
            },
            new User
            {
                Id = 2,
                Login = "John",
                Password = CipherUtility.Encrypt("Johny", "John"),
                BoundToUserId = 1
            },
            new User
            {
                Id = 3,
                Login = "Joe",
                Password = CipherUtility.Encrypt("Johny", "Joe"),
                BoundToUserId = 2
            }
        };
    }
}
