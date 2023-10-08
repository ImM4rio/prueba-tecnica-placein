using Microsoft.AspNetCore.Mvc;
using PedidosAPI.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Text.Json;

namespace PedidosAPI.Controllers
{

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

                return Ok(lista);

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
            int temperatura;
            int humedad;
            string weatherURL = "http://api.weatherstack.com/current?access_key=c190f8aeb7838b5dc98e1e2cb169471e&query=";
            SqlConnection conexion;

            try
            {
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
                    return BadRequest("El valor de 'sandbox' no es válido.");
                }



                (temperatura, humedad) = await WeatherUtility.CheckWeatherAsync(pedido.Ubicacion, weatherURL);

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
                
                return Ok(pedido);
                

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = error.Message });
            }
        }

    }

    public class WeatherstackController : ControllerBase
    {
        
        private readonly string weatherURL;

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
                (temperatura, humedad) = await WeatherUtility.CheckWeatherAsync(city, weatherURL);

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
        public static async Task<(int temperatura, int humedad)> CheckWeatherAsync(string ciudad, string weatherURL)
        {
            int temperatura = 0;
            int humedad = 0;

            try
            {

                string query = $"http://api.weatherstack.com/current?access_key=c190f8aeb7838b5dc98e1e2cb169471e&query={ciudad}";
                using (HttpClient httpClient = new())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(query);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        Weatherstack weatherData = JsonSerializer.Deserialize<Weatherstack>(json);

                        if (weatherData != null && weatherData.current != null)
                        {
                            temperatura = weatherData.current.temperature;
                            humedad = weatherData.current.humidity;
                        }
                        else
                        {
                            throw new Exception("Los datos del clima no son válidos o están incompletos.");
                        }
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
