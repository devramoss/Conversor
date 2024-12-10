using Conversor.Models;
using CoordinateSharp;
using Microsoft.AspNetCore.Mvc;

namespace Conversor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoordenadaController : ControllerBase
    {
        [HttpPost]

        public IActionResult ConverterParaGraus([FromBody] Coordenada coordenada)
        {
            try
            {
                string Zona = coordenada.Zona;
                double Leste = coordenada.Leste;
                double Norte = coordenada.Norte;


                if(string.IsNullOrEmpty(Zona) || Leste <= 0 || Norte <= 0)
                    return BadRequest(new { error = "Dados inválidos. Certifique-se de fornecer zone, easting e northing." });

                Coordinate coordenadaGraus = new Coordinate();
                UniversalTransverseMercator utm = new UniversalTransverseMercator(Zona, Leste, Norte);
                coordenadaGraus = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);

                return Ok(new
                {
                    latitude = coordenadaGraus.Latitude.DecimalDegree,
                    longitude = coordenadaGraus.Longitude.DecimalDegree
                });

            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno no servidor.", details = ex.Message });
            }
        }

    }
}
