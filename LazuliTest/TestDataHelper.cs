using Lazuli.Data.Database;
using LazuliLibrary.Models;
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

    public static List<UserModel> GetFakeUserModelList()
    {
        return new List<UserModel>
        {
            new UserModel
            {
                Id = 1,
                Name = "Leanne Graham",
                Username = "Bret",
                Email = "Sincere@april.bix",
                Address = new AddressModel
                {
                    Street = "Kulas Light",
                    Suite = "Apt. 556",
                    City = "Gwenborough",
                    Zipcode = "92998-3874",
                    GeoLocation = new GeoLocationModel
                    {
                        Latitude = "-37.3159",
                        Longitude = "81.1496"
                    }
                },
                Phone = "1-770-736-8031 x56442",
                Website = "hildegard.org",
                Company = new CompanyModel
                {
                    Name = "Romaguera-Crona",
                    CatchPhrase = "Multi-layered client-server neural-net",
                    BusinessStrategy = "harness real-time e-markets"
                }
            },
            new UserModel
            {
                Id = 2,
                Name = "Ervin Howell",
                Username = "Antonette",
                Email = "Shanna@melissa.tv",
                Address = new AddressModel
                {
                    Street = "Victor Plains",
                    Suite = "Suite 879",
                    City = "Wisokyburgh",
                    Zipcode = "90566-7771",
                    GeoLocation = new GeoLocationModel
                    {
                        Latitude = "-43.9509",
                        Longitude = "-34.4618"
                    }
                },
                Phone = "010-692-6593 x09125",
                Website = "anastasia.net",
                Company = new CompanyModel
                {
                    Name = "Deckow-Crist",
                    CatchPhrase = "Proactive didactic contingency",
                    BusinessStrategy = "synergize scalable supply-chains"
                }
            }
        };
    }
}
