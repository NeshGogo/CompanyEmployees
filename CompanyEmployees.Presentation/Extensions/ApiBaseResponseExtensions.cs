using Entities.Responses;

namespace CompanyEmployees.Presentation.Extensions
{
    public static class ApiBaseResponseExtensions 
    {
        public static TResultType GetResult<TResultType>(this ApiBaseResponse response) => 
            ((ApiOkResponse<TResultType>)response).Result;
    }
}
