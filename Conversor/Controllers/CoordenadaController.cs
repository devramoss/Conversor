using Conversor.Models;
using CoordinateSharp;
using Microsoft.AspNetCore.Mvc;

namespace Conversor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoordenadaController : ControllerBase
    {
        [HttpPost("converter-graus")]

        public IActionResult ConverterParaGraus([FromBody] Coordenada coordenada)
        {
            try
            {
                if (string.IsNullOrEmpty(coordenada.Zona) || !coordenada.Leste.HasValue || !coordenada.Norte.HasValue)
                    return BadRequest(new { error = "Dados inválidos. Certifique-se de fornecer os campos zona, leste e norte." });

                Coordinate coordenadaGraus = new Coordinate();
                UniversalTransverseMercator utm = new UniversalTransverseMercator(coordenada.Zona, coordenada.Leste.Value, coordenada.Norte.Value);
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

        [HttpPost("converter-utm")]
        public IActionResult ConverterParaUtm([FromBody] Coordenada coordenada)
        {
            try
            {
                if (!coordenada.Latitude.HasValue || !coordenada.Longitude.HasValue)
                    return BadRequest(new { error = "Dados inválidos. Certifique-se de fornecer os campos de latitude e longitude!" });

                Coordinate coordenadaGraus = new Coordinate(coordenada.Latitude.Value, coordenada.Longitude.Value);
                UniversalTransverseMercator utm = coordenadaGraus.UTM;

                return Ok(new
                {
                    zona = utm.LongZone + utm.LatZone,
                    leste = utm.Easting,
                    norte = utm.Northing
                });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Erro interno no servidor.",
                    details = ex.Message
                });
            }

        }
    }
}