using Entities.Models;

namespace Repository.Extensions;

public static class RepositoryEmployeeExtensions
{
    public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> query, uint mingAge, uint maxAge) => 
        query.Where(p => p.Age >= mingAge && p.Age <= maxAge);

    public static IQueryable<Employee> Search(this IQueryable<Employee> query, string? searchTerm)
    {
        if (searchTerm == null)
            return query;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return query.Where(p => p.Name.ToLower().Contains(lowerCaseTerm));
    }
}
