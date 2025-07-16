// using System.Text.Json;
// using CoreProject.Models;
// using CoreProject.interfaces;
// namespace CoreProject.Services;
// public class ShoesService : IShoesService
// {
//     List<Shoes> Shoes { get; }
//     private static string fileName = "Shoes.json";
//     private string filePath;

//     public ShoesService(IHostEnvironment env)
//     {
//         filePath = Path.Combine(env.ContentRootPath, "Data", fileName);

//         using (var jsonFile = File.OpenText(filePath))
//         {
//             Shoes = JsonSerializer.Deserialize<List<Shoes>>(jsonFile.ReadToEnd(),
//             new JsonSerializerOptions
//             {
//                 PropertyNameCaseInsensitive = true
//             });
//         }
//     }

//     private void saveToFile()
//     {
//         File.WriteAllText(filePath, JsonSerializer.Serialize(Shoes));
//     }
//     public Shoes Get(int id) => Shoes.FirstOrDefault(p => p.Id == id);
//     public List<Shoes> Get() => Shoes;
//     public int Add(Shoes shoes)
//     {
//         if (shoes == null)
//             return -1;
//         int maxId = Shoes.Max(p => p.Id);
//         shoes.Id = maxId + 1;
//         Shoes.Add(shoes);
//         saveToFile();
//         return shoes.Id;
//     }
//     public bool Update(int id, Shoes shoes)
//     {
//         if (shoes == null
//             || string.IsNullOrWhiteSpace(shoes.Description)
//             || shoes.Id != id)
//         {
//             return false;
//         }

//         Shoes shs = Shoes.FirstOrDefault(p => p.Id == id);
//         if (shs == null)
//             return false;

//         var index = Shoes.IndexOf(shs);
//         Shoes[index] = shoes;
//         saveToFile();
//         return true;
//     }
//     public bool Delete(int id)
//     {
//         Shoes shoes = Get(id);
//         if (shoes is null)
//             return false;
//         Shoes.Remove(shoes);
//         saveToFile();
//         return true;
//     }
// }

// public static class ShoesUtilities
// {
//     public static void AddShoesConst(this IServiceCollection services)
//     {
//         services.AddSingleton<IShoesService, ShoesService>();
//     }
// }
