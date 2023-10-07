using Microsoft.AspNetCore.Mvc;
using PedidosAPI.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Text.Json;

namespace PedidosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly string cadenaSQL;
        private readonly string cadenaSQLProd;
        private readonly string weatherURL;

        public PedidosController(IConfiguration configuration)
        {
            cadenaSQLProd = configuration.GetConnectionString("CadenaSQLProd");
            cadenaSQL = configuration.GetConnectionString("CadenaSQL");
            weatherURL = configuration.GetConnectionString("WeatherURL");

        }

        [HttpGet]
        [Route("lista")]
        public IActionResult lista()
        {
            List<Pedido> lista = new List<Pedido>();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL)) { 
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_SeleccionarPedidos", conexion)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Pedido()
                            {
                                IdPedido = rd["IdPedido"].ToString(),
                                Nombre = rd["Nombre"].ToString(),
                                Descripcion = rd["Descripcion"].ToString(),
                                Ubicacion = rd["Ubicacion"].ToString(),
                                Temperatura = rd.IsDBNull(rd.GetOrdinal("Temperatura")) ? 0 : rd.GetInt32(rd.GetOrdinal("Temperatura")),
                                Humedad = rd.IsDBNull(rd.GetOrdinal("Humedad")) ? 0 : rd.GetInt32(rd.GetOrdinal("Humedad"))
                            }) ;
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });

            }catch(Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

        [HttpPost]
        [Route("registro/{sandbox}")]
        public async Task<IActionResult> Registro([FromBody] Pedido pedido, string sandbox)
        {
            pedido.IdPedido = Guid.NewGuid().ToString();
            string ciudad = pedido.Ubicacion;
            int temperatura;
            int humedad;

            try
            {
                (temperatura, humedad) = await WeatherUtility.ConsultarTiempoAsync(ciudad, weatherURL);

                SqlConnection conexion;

                if (sandbox == "true")
                {
                    conexion = new SqlConnection(cadenaSQL);
                }
                else if (sandbox == "false")
                {
                    conexion = new SqlConnection(cadenaSQLProd);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "El valor de 'sandbox' no es válido." });
                }
                using (conexion)
                        {
                            conexion.Open();
                            SqlCommand cmd = new SqlCommand("sp_InsertarPedido", conexion);

                            cmd.Parameters.AddWithValue("IdPedido", pedido.IdPedido);
                            cmd.Parameters.AddWithValue("Nombre", pedido.Nombre);
                            cmd.Parameters.AddWithValue("Descripcion", pedido.Descripcion);
                            cmd.Parameters.AddWithValue("Ubicacion", pedido.Ubicacion);
                            cmd.Parameters.AddWithValue("Temperatura", temperatura);
                            cmd.Parameters.AddWithValue("Humedad", humedad);

                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.ExecuteNonQuery();

                        }
                
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok"});
                

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

    }

    public class WeatherstackController : ControllerBase
    {
        
        private readonly string weatherURL;
        private readonly HttpClient httpClient;
        public WeatherstackController(IConfiguration configuration)
        {
            weatherURL = configuration.GetConnectionString("WeatherURL");

        }

        [HttpGet]
        [Route("tiempo/{city}")]
        public async Task<IActionResult> GetWeatherData(string city)
        {
            int temperatura;
            int humedad;
            try
            {
                (temperatura, humedad) = await WeatherUtility.ConsultarTiempoAsync(city, weatherURL);

                    string weatherDescription = $"En {city} hay una temperatura de {temperatura} ºC con una humedad del {humedad} %.";

                    return Ok(weatherDescription);

            } 
                catch (Exception error)
            {
                return StatusCode(500, $"Error: {error.Message}");
            }
        }

    }

    public static class WeatherUtility
    {
        public static async Task<(int temperatura, int humedad)> ConsultarTiempoAsync(string ciudad, string weatherURL)
        {
            int temperatura = 0;
            int humedad = 0;

            try
            {
                string query = $"{weatherURL}?city={ciudad}";
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(query);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        Weatherstack weatherData = JsonSerializer.Deserialize<Weatherstack>(json);

                        temperatura = weatherData.current.temperature;
                        humedad = weatherData.current.humidity;
                    }
                }
            }
            catch (Exception error)
            {
                throw new Exception($"Error al consultar el tiempo: {error.Message}");
            }

            return (temperatura, humedad);
        }
    }

}
