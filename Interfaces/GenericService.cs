using CoreProject.Models;
namespace CoreProject.interfaces;
public interface IGenericService<T> where T : GenericId 
{
    T Get(int id);
    List<T> Get();
    int Add(T shoes);
    bool Update(int id, T shoes);
    bool Delete(int id);
}