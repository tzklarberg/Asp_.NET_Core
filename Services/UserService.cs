// using System.Text.Json;
// using CoreProject.Models;
// using CoreProject.interfaces;
// namespace CoreProject.Services;
// public class UserService : IUserService
// {
//     List<User> Users { get; }
//     private static string fileName = "user.json";
//     private string filePath;

//     public UserService(IHostEnvironment env)
//     {
//         filePath = Path.Combine(env.ContentRootPath, "Data", fileName);

//         using (var jsonFile = File.OpenText(filePath))
//         {
//             Users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
//             new JsonSerializerOptions
//             {
//                 PropertyNameCaseInsensitive = true
//             });
//         }
//     }

//     private void saveToFile()
//     {
//         File.WriteAllText(filePath, JsonSerializer.Serialize(Users));
//     }
//     public User Get(int id) => Users.FirstOrDefault(p => p.Id == id);
//     public List<User> Get() => Users;
//     public String Add(User user)
//     {
//         if (user == null)
//             return null;
//         Users.Add(user);
//         saveToFile();
//         return user.Password;
//     }
//     public bool Update(String password, User user)
//     {
//         if (user == null
//             || user.Password != password)
//         {
//             return false;
//         }

//         User shs = Users.FirstOrDefault(p => p.Password == password);
//         if (shs == null)
//             return false;

//         var index = Users.IndexOf(shs);
//         Users[index] = user;
//         saveToFile();
//         return true;
//     }
//     public bool Delete(int id)
//     {
//         User shoes = Get(id);
//         if (shoes is null)
//             return false;
//         Shoes.Remove(shoes);
//         saveToFile();
//         return true;
//     }
// }

// public static class UserUtilities
// {
//     public static void AddShoesConst(this IServiceCollection services)
//     {
//         services.AddSingleton<IShoesService, UserService>();
//     }
// }
