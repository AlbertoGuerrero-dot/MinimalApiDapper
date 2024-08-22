using Microsoft.IdentityModel.Tokens;
using MinimalAPIPeliculas.Utilities;

namespace MinimalAPIPeliculas.DTOs
{
    public class paginationDto
    {
        private const int InitialPage = 1;
        private const int InitialRecordsPerPage = 10;
        public int Page { get; set; } = InitialPage;
        public int recordsPerPage { get; set; } = InitialRecordsPerPage;
        private readonly int maxRecordsPerPage  = 50;

        public int RecordsPerPage 
        {
            get { return recordsPerPage; }
            set { recordsPerPage = (value > maxRecordsPerPage) ? maxRecordsPerPage : value; }
        } 
        public static ValueTask<paginationDto> BindAsync (HttpContext httpContext )
        {
            var page = httpContext.ExtractDefaultValue(nameof(Page), InitialPage); 
            var recordsPerPage = httpContext.ExtractDefaultValue(nameof(RecordsPerPage), InitialRecordsPerPage); 

            var result = new paginationDto { Page = page, RecordsPerPage = recordsPerPage };
            return ValueTask.FromResult(result);
        }
    }
}
