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

    public IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldsString)
    {
        var requiredProperties = GetRequiredProperties(fieldsString);

        return FetchData(entities, requiredProperties);
    }

    public ShapedEntity ShapeData(T entity, string fieldsString)
    {
        var requiredProperties = GetRequiredProperties(fieldsString);

        return FetchDataForShapedEntity(entity, requiredProperties);
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

    private IEnumerable<ShapedEntity> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> properties)
    {
        var shapedData = new List<ShapedEntity>();

        foreach (var ShapedEntity in entities)
        {
            var shapedObject = FetchDataForShapedEntity(ShapedEntity, properties);
            shapedData.Add(shapedObject);
        }

        return shapedData;
    }

    private ShapedEntity FetchDataForShapedEntity(T entity, IEnumerable<PropertyInfo> properties)
    {
        var shapedObj = new ShapedEntity();
        foreach (var property in properties)
        {
            var value = property.GetValue(entity);
            shapedObj.Entity.TryAdd(property.Name, value);
        }

        var objProp = entity.GetType().GetProperty("Id");
        shapedObj.Id = (Guid)objProp.GetValue(entity);

        return shapedObj;
    }
}
