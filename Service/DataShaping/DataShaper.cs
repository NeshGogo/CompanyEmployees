using Contracts;
using Entities.Models;
using System.Dynamic;
using System.Reflection;

namespace Service.DataShaping;

public class DataShaper<T> : IDataShaper<T> where T : class
{

    public PropertyInfo[] Properties { get; set; }

    public DataShaper()
    {
        Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    public IEnumerable<Entity> ShapeData(IEnumerable<T> entities, string fieldsString)
    {
        var requiredProperties = GetRequiredProperties(fieldsString);

        return FetchData(entities, requiredProperties);
    }

    public Entity ShapeData(T entity, string fieldsString)
    {
        var requiredProperties = GetRequiredProperties(fieldsString);

        return FetchDataForEntity(entity, requiredProperties);
    }

    private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldsString)
    {
        var requiredProperties = new List<PropertyInfo>();

        if (!string.IsNullOrEmpty(fieldsString))
        {
            var fields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var field in fields)
            {
                var property = Properties.FirstOrDefault(p => p.Name.Equals(field.Trim(), 
                    StringComparison.InvariantCultureIgnoreCase));
                if (property == null)
                    continue;

                requiredProperties.Add(property);
            }
        } 
        else
        {
            requiredProperties = Properties.ToList();
        }

        return requiredProperties;
    }

    private IEnumerable<Entity> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> properties)
    {
        var shapedData = new List<Entity>();

        foreach (var entity in entities)
        {
            var shapedObject = FetchDataForEntity(entity, properties);
            shapedData.Add(shapedObject);
        }

        return shapedData;
    }

    private Entity FetchDataForEntity(T entity, IEnumerable<PropertyInfo> properties)
    {
        var shapedObj = new Entity();
        foreach (var property in properties)
        {
            var value = property.GetValue(entity);
            shapedObj.TryAdd(property.Name, value);
        }

        return shapedObj;
    }
}
