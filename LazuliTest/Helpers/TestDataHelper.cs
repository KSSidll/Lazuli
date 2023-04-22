using LazuliLibrary.Data.Database;
using LazuliLibrary.Models;
using LazuliLibrary.Utils;

namespace LazuliTest.Helpers;

public static class TestDataHelper
{
	public static List<User> GetFakeDatabaseUserList()
	{
		return new List<User>
		{
			new()
			{
				Id = 1,
				Login = "John Doe",
				Password = CipherUtility.Encrypt("Johny", "John Doe"),
				BoundToUserId = 1
			},
			new()
			{
				Id = 2,
				Login = "John",
				Password = CipherUtility.Encrypt("Johny", "John"),
				BoundToUserId = 1
			},
			new()
			{
				Id = 3,
				Login = "Joe",
				Password = CipherUtility.Encrypt("Johny", "Joe"),
				BoundToUserId = 2
			}
		};
	}

	public static AuthenticatedUserModel GetFakeAuthUser(int id)
	{
		return new AuthenticatedUserModel
		{
			BoundToUserId = id.ToString(),
			Email = "test@test.com"
		};
	}

	public static List<UserModel> GetFakeUserModelList()
	{
		return new List<UserModel>
		{
			new()
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
			new()
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

	public static List<PostModel> GetFakePostModelList()
	{
		return new List<PostModel>
		{
			new()
			{
				Body =
					"quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto",
				Id = 1,
				Title = "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
				UserId = 1
			},
			new()
			{
				Body =
					"est rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus qui neque nisi nulla",
				Id = 2,
				Title = "qui est esse",
				UserId = 1
			},
			new()
			{
				Body =
					"delectus reiciendis molestiae occaecati non minima eveniet qui voluptatibus\naccusamus in eum beatae sit\nvel qui neque voluptates ut commodi qui incidunt\nut animi commodi",
				Id = 3,
				Title = "et ea vero quia laudantium autem",
				UserId = 2
			},
			new()
			{
				Body =
					"itaque id aut magnam\npraesentium quia et ea odit et ea voluptas et\nsapiente quia nihil amet occaecati quia id voluptatem\nincidunt ea est distinctio odio",
				Id = 4,
				Title = "in quibusdam tempore odit est dolorem",
				UserId = 2
			}
		};
	}

	public static List<CommentModel> GetFakeCommentModelList()
	{
		return new List<CommentModel>
		{
			new()
			{
				PostId = 1,
				Id = 1,
				Name = "id labore ex et quam laborum",
				Email = "Eliseo@gardner.biz",
				Body =
					"laudantium enim quasi est quidem magnam voluptate ipsam eos\ntempora quo necessitatibus\ndolor quam autem quasi\nreiciendis et nam sapiente accusantium"
			},
			new()
			{
				PostId = 1,
				Id = 2,
				Name = "quo vero reiciendis velit similique earum",
				Email = "Jayne_Kuhic@sydney.com",
				Body =
					"est natus enim nihil est dolore omnis voluptatem numquam\net omnis occaecati quod ullam at\nvoluptatem error expedita pariatur\nnihil sint nostrum voluptatem reiciendis et"
			},
			new()
			{
				PostId = 1,
				Id = 3,
				Name = "odio adipisci rerum aut animi",
				Email = "Nikita@garfield.biz",
				Body =
					"quia molestiae reprehenderit quasi aspernatur\naut expedita occaecati aliquam eveniet laudantium\nomnis quibusdam delectus saepe quia accusamus maiores nam est\ncum et ducimus et vero voluptates excepturi deleniti ratione"
			},
			new()
			{
				PostId = 1,
				Id = 4,
				Name = "alias odio sit",
				Email = "Lew@alysha.tv",
				Body =
					"non et atque\noccaecati deserunt quas accusantium unde odit nobis qui voluptatem\nquia voluptas consequuntur itaque dolor\net qui rerum deleniti ut occaecati"
			},
			new()
			{
				PostId = 1,
				Id = 5,
				Name = "vero eaque aliquid doloribus et culpa",
				Email = "Hayden@althea.biz",
				Body =
					"harum non quasi et ratione\ntempore iure ex voluptates in ratione\nharum architecto fugit inventore cupiditate\nvoluptates magni quo et"
			},
			new()
			{
				PostId = 2,
				Id = 6,
				Name = "et fugit eligendi deleniti quidem qui sint nihil autem",
				Email = "Presley.Mueller@myrl.com",
				Body =
					"doloribus at sed quis culpa deserunt consectetur qui praesentium\naccusamus fugiat dicta\nvoluptatem rerum ut voluptate autem\nvoluptatem repellendus aspernatur dolorem in"
			},
			new()
			{
				PostId = 2,
				Id = 7,
				Name = "repellat consequatur praesentium vel minus molestias voluptatum",
				Email = "Dallas@ole.me",
				Body =
					"maiores sed dolores similique labore et inventore et\nquasi temporibus esse sunt id et\neos voluptatem aliquam\naliquid ratione corporis molestiae mollitia quia et magnam dolor"
			},
			new()
			{
				PostId = 2,
				Id = 8,
				Name = "et omnis dolorem",
				Email = "Mallory_Kunze@marie.org",
				Body =
					"ut voluptatem corrupti velit\nad voluptatem maiores\net nisi velit vero accusamus maiores\nvoluptates quia aliquid ullam eaque"
			},
			new()
			{
				PostId = 2,
				Id = 9,
				Name = "provident id voluptas",
				Email = "Meghan_Littel@rene.us",
				Body =
					"sapiente assumenda molestiae atque\nadipisci laborum distinctio aperiam et ab ut omnis\net occaecati aspernatur odit sit rem expedita\nquas enim ipsam minus"
			},
			new()
			{
				PostId = 2,
				Id = 10,
				Name = "eaque et deleniti atque tenetur ut quo ut",
				Email = "Carmen_Keeling@caroline.name",
				Body =
					"voluptate iusto quis nobis reprehenderit ipsum amet nulla\nquia quas dolores velit et non\naut quia necessitatibus\nnostrum quaerat nulla et accusamus nisi facilis"
			},
			new()
			{
				PostId = 3,
				Id = 11,
				Name = "fugit labore quia mollitia quas deserunt nostrum sunt",
				Email = "Veronica_Goodwin@timmothy.net",
				Body =
					"ut dolorum nostrum id quia aut est\nfuga est inventore vel eligendi explicabo quis consectetur\naut occaecati repellat id natus quo est\nut blanditiis quia ut vel ut maiores ea"
			},
			new()
			{
				PostId = 3,
				Id = 12,
				Name = "modi ut eos dolores illum nam dolor",
				Email = "Oswald.Vandervort@leanne.org",
				Body =
					"expedita maiores dignissimos facilis\nipsum est rem est fugit velit sequi\neum odio dolores dolor totam\noccaecati ratione eius rem velit"
			},
			new()
			{
				PostId = 3,
				Id = 13,
				Name = "aut inventore non pariatur sit vitae voluptatem sapiente",
				Email = "Kariane@jadyn.tv",
				Body =
					"fuga eos qui dolor rerum\ninventore corporis exercitationem\ncorporis cupiditate et deserunt recusandae est sed quis culpa\neum maiores corporis et"
			},
			new()
			{
				PostId = 3,
				Id = 14,
				Name = "et officiis id praesentium hic aut ipsa dolorem repudiandae",
				Email = "Nathan@solon.io",
				Body =
					"vel quae voluptas qui exercitationem\nvoluptatibus unde sed\nminima et qui ipsam aspernatur\nexpedita magnam laudantium et et quaerat ut qui dolorum"
			},
			new()
			{
				PostId = 3,
				Id = 15,
				Name = "debitis magnam hic odit aut ullam nostrum tenetur",
				Email = "Maynard.Hodkiewicz@roberta.com",
				Body =
					"nihil ut voluptates blanditiis autem odio dicta rerum\nquisquam saepe et est\nsunt quasi nemo laudantium deserunt\nmolestias tempora quo quia"
			},
			new()
			{
				PostId = 4,
				Id = 16,
				Name = "perferendis temporibus delectus optio ea eum ratione dolorum",
				Email = "Christine@ayana.info",
				Body =
					"iste ut laborum aliquid velit facere itaque\nquo ut soluta dicta voluptate\nerror tempore aut et\nsequi reiciendis dignissimos expedita consequuntur libero sed fugiat facilis"
			},
			new()
			{
				PostId = 4,
				Id = 17,
				Name = "eos est animi quis",
				Email = "Preston_Hudson@blaise.tv",
				Body =
					"consequatur necessitatibus totam sed sit dolorum\nrecusandae quae odio excepturi voluptatum harum voluptas\nquisquam sit ad eveniet delectus\ndoloribus odio qui non labore"
			},
			new()
			{
				PostId = 4,
				Id = 18,
				Name = "aut et tenetur ducimus illum aut nulla ab",
				Email = "Vincenza_Klocko@albertha.name",
				Body =
					"veritatis voluptates necessitatibus maiores corrupti\nneque et exercitationem amet sit et\nullam velit sit magnam laborum\nmagni ut molestias"
			},
			new()
			{
				PostId = 4,
				Id = 19,
				Name = "sed impedit rerum quia et et inventore unde officiis",
				Email = "Madelynn.Gorczany@darion.biz",
				Body =
					"doloribus est illo sed minima aperiam\nut dignissimos accusantium tempore atque et aut molestiae\nmagni ut accusamus voluptatem quos ut voluptates\nquisquam porro sed architecto ut"
			},
			new()
			{
				PostId = 4,
				Id = 20,
				Name = "molestias expedita iste aliquid voluptates",
				Email = "Mariana_Orn@preston.org",
				Body =
					"qui harum consequatur fugiat\net eligendi perferendis at molestiae commodi ducimus\ndoloremque asperiores numquam qui\nut sit dignissimos reprehenderit tempore"
			}
		};
	}

	public static List<AlbumModel> GetFakeAlbumModelList()
	{
		return new List<AlbumModel>
		{
			new()
			{
				UserId = 1,
				Id = 1,
				Title = "distinctio laborum qui"
			},
			new()
			{
				UserId = 2,
				Id = 2,
				Title = "quam nostrum impedit mollitia quod et dolor"
			}
		};
	}

	public static List<PhotoModel> GetFakePhotoModelList()
	{
		return new List<PhotoModel>
		{
			new()
			{
				AlbumId = 1,
				Id = 1,
				Title = "accusamus beatae ad facilis cum similique qui sunt",
				Url = "https://via.placeholder.com/600/92c952",
				ThumbnailUrl = "https://via.placeholder.com/150/92c952"
			},
			new()
			{
				AlbumId = 2,
				Id = 2,
				Title = "non sunt voluptatem placeat consequuntur rem incidunt",
				Url = "https://via.placeholder.com/600/8e973b",
				ThumbnailUrl = "https://via.placeholder.com/150/8e973b"
			}
		};
	}
}