using Entities.LinkModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api")]
[ApiController]
public class RootController : ControllerBase
{
    private readonly LinkGenerator _linkGenerator;

    public RootController(LinkGenerator linkGenerator) => _linkGenerator = linkGenerator;

    [HttpGet(Name = "GetRoot")]
    public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediatype)
    {
        if (mediatype.Contains("vnd.neshgogo.apiroot"))
        {
            var list = new List<Link>
            {
                new Link
                {
                    Href = _linkGenerator.GetUriByName(HttpContext, nameof(GetRoot), new{}),
                    Rel = "self",
                    Method = "GET",
                },
                new Link
                {
                    Href = _linkGenerator.GetUriByName(HttpContext, "GetCompanies", new{}),
                    Rel = "companies",
                    Method = "GET",
                },
                new Link
                {
                    Href = _linkGenerator.GetUriByName(HttpContext, "CreateCompany", new{}),
                    Rel = "create_companies",
                    Method = "POST",
                },
                new Link
                {
                    Href = _linkGenerator.GetUriByName(HttpContext, "CreateCompanyCollection", new{}),
                    Rel = "create_companies_collection",
                    Method = "POST",
                },
            };
            return Ok(list);
        }
        return NoContent();
    }
}
