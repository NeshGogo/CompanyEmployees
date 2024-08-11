using Entities.Models;
using System.Linq.Dynamic.Core;
using Repository.Extensions.Utility;

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

    public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string sortTerm)
    {
        if(string.IsNullOrEmpty(sortTerm))
            return employees.OrderBy(p => p.Name);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(sortTerm);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return employees.OrderBy(e => e.Name);

        return employees.OrderBy(orderQuery);
    }
}
