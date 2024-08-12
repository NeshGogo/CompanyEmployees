using Contracts;
using Entities.LinkModels;
using Entities.Models;
using Shared.DataTransferObjects;
using System.Net.Http.Headers;

namespace CompanyEmployees.Utility;

public class EmployeeLinks : IEmployeeLinks
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IDataShaper<EmployeeDto> _dataShaper;

    public EmployeeLinks(LinkGenerator linkGenerator, IDataShaper<EmployeeDto> dataShaper)
    {
        _linkGenerator = linkGenerator;
        _dataShaper = dataShaper;
    }

    public LinkResponse TryGenerateLinks(IEnumerable<EmployeeDto> employeeDtos, string fields,
        Guid companyId, HttpContext httpContext)
    {
        var shapedEmployees = ShapeData(employeeDtos, fields);

        if (ShouldGenerateLinks(httpContext))
            return ReturnLinkedEmployees(employeeDtos, fields, companyId, httpContext, shapedEmployees);

        return ReturnShapedEmployees(shapedEmployees);
    }

    private List<Entity> ShapeData(IEnumerable<EmployeeDto> employeeDtos, string fields) =>
        _dataShaper.ShapeData(employeeDtos, fields)
            .Select(p => p.Entity)
            .ToList();

    private bool ShouldGenerateLinks(HttpContext httpContext)
    {
        var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];

        return mediaType.MediaType.Contains("hateoas",
                StringComparison.InvariantCultureIgnoreCase);
    }

    private LinkResponse ReturnShapedEmployees(List<Entity> shapedEmployees) =>
            new LinkResponse { ShapedEntities = shapedEmployees };

    private LinkResponse ReturnLinkedEmployees(IEnumerable<EmployeeDto> employeeDtos, 
        string fields, Guid companyId, HttpContext httpContext, List<Entity> shapedEmployees)
    {
        var employeeDtoList = employeeDtos.ToList();
        var length = employeeDtoList.Count();

        for (int i = 0; i < length; i++)
        {
            var employeeLinks = CreateLinksForEmployees(httpContext, companyId, 
                employeeDtoList[i].Id, fields);
            shapedEmployees[i].Add("Links", employeeLinks);
        }

        var employeeCollection = new LinkCollectionWrapper<Entity>(shapedEmployees);
        var linkedEmployees = CreateLinksForEmployees(httpContext, employeeCollection);

        return new LinkResponse { HasLinks = true, LinkedEntities = linkedEmployees };
    }

    private List<Link> CreateLinksForEmployees(HttpContext httpContext, Guid companyId, Guid id, string fields = "")
    {
        var links = new List<Link>
        {
            new Link(_linkGenerator.GetUriByAction(httpContext, "GetEmployeesForCompany", 
                values: new {companyId, id, fields}), "self", "GET"),

             new Link(_linkGenerator.GetUriByAction(httpContext, "DeleteEmployee",
                values: new {companyId, id}), "delete_employee", "DELETE"),

             new Link(_linkGenerator.GetUriByAction(httpContext, "UpdateEmployee",
                values: new {companyId, id}), "update_employee", "PUT"),

             new Link(_linkGenerator.GetUriByAction(httpContext, "PartiallyUpdateEmployeeForCompany",
                values: new {companyId, id}), "partially_update_employee", "PATCH"),
        };

        return links;
    }

    private LinkCollectionWrapper<Entity> CreateLinksForEmployees(HttpContext httpContext, 
        LinkCollectionWrapper<Entity> employeesWrapper)
    {
        employeesWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext,
            "GetEmployeesForCompany", values: new { }),
            "self",
            "GET"));

        return employeesWrapper;
    }
}
