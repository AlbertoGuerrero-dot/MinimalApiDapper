using MinimalAPIPeliculas.Utilities;

namespace MinimalAPIPeliculas.DTOs
{
    public class MovieFilterDTO
    {
        public int Pagina { get; set; }
        public int RecordsPorPagina { get; set; }
        public paginationDto paginationDTO { get 
            {
                return new paginationDto()
                {
                    Page = Pagina,
                    RecordsPerPage = RecordsPorPagina
                };
            }
        }
        public string? Titulo { get; set; }
        public int GeneroId { get; set; }
        public bool EnCines { get; set; }   
        public bool ProximoEstrenos { get; set; }
        public string? CampoOrdenar { get; set; }
        public bool OrdenAscendente { get; set; } = true;
        public static ValueTask<MovieFilterDTO> BindAsync(HttpContext httpContext )
        {
            var page = httpContext.ExtractDefaultValue(nameof(Pagina), 1);
            var recordsPerPage = httpContext.ExtractDefaultValue(nameof(RecordsPorPagina), 10);
            var generoId = httpContext.ExtractDefaultValue(nameof(GeneroId), 0);
            var titulo = httpContext.ExtractDefaultValue(nameof(Titulo), string.Empty);
            var enCines = httpContext.ExtractDefaultValue(nameof(EnCines), false);
            var proximoEstrenos = httpContext.ExtractDefaultValue(nameof(ProximoEstrenos), false);
            var campoOrdenar = httpContext.ExtractDefaultValue(nameof(CampoOrdenar), string.Empty);
            var ordenAscendente = httpContext.ExtractDefaultValue(nameof(OrdenAscendente), true);
            var result = new MovieFilterDTO
            {
                Pagina = page,
                RecordsPorPagina = recordsPerPage,
                GeneroId = generoId,
                Titulo = titulo,
                EnCines = enCines,
                ProximoEstrenos = proximoEstrenos,
                CampoOrdenar = campoOrdenar,
                OrdenAscendente = ordenAscendente
             };
            return ValueTask.FromResult(result);
             
        } 

    }
}
