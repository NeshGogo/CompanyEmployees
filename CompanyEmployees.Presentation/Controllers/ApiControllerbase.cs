using Entities.ErrorModel;
using Entities.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Presentation.Controllers;

public class ApiControllerbase : ControllerBase
{
    public IActionResult ProcessError(ApiBaseResponse baseResponse)
    {
        return baseResponse switch
        {
            ApiNotFoundResponse => NotFound(new ErrorDetails
            {
                Message = ((ApiNotFoundResponse)baseResponse).Message,
                StatusCode = StatusCodes.Status404NotFound
            }),
            ApiBadRequestResponse => NotFound(new ErrorDetails
            {
                Message = ((ApiBadRequestResponse)baseResponse).Message,
                StatusCode = StatusCodes.Status400BadRequest
            }),
            _ => throw new NotImplementedException()

        };
    }
}
