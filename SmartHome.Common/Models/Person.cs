namespace SmartHome.Common.Models;

public record Person(string Name, int Age);

public interface IDatabase
{
    public Task<Person?> GetPersonByName(string name);
    public Task<Person?> GetPersonByAge(int age);
    public Task AddPerson(Person person);
}
public class MemoryDatabase : IDatabase
{
    private List<Person> _persons = new();
    public async Task AddPerson(Person person)
    {
        await Task.Delay(500);
        _persons.Add(person);
    }

    public async Task<Person?> GetPersonByAge(int age)
    {
        await Task.Delay(500);
        return _persons.FirstOrDefault(p => p.Age == age);
    }

    public async Task<Person?> GetPersonByName(string name)
    {
        await Task.Delay(500);
        return _persons.FirstOrDefault(p => p.Name == name);
    }
}