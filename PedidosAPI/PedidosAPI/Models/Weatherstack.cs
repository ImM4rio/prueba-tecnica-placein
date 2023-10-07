namespace PedidosAPI.Models
{
    public class Weatherstack
    {
        public Location location { get; set; }
        public CurrentWeather current {  get; set; }
    }

    public class Location
    {
        public string name { get; set; }
        public string country { get; set; }
        public string region { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string timezone_id { get; set; }
        public string localtime { get; set; }
        public long localtime_epoch { get; set; }
        public string utc_offset { get; set; }
    }

    public class CurrentWeather 
    {
        public string observation_time { get; set; }
        public int temperature { get; set; }
        public int weather_code { get; set; }
        public string[] weather_icons {  get; set; }
        public string[] weather_descriptions { get; set; }
        public int wind_speed { get; set; }
        public int wind_degree { get; set; }
        public string wind_dir {  get; set; }
        public int pressure { get; set; }
        public int precip {  get; set; }
        public int humidity { get; set; }
        public int cloudcover { get; set; }
        public int feelslike { get; set; }
        public int uv_index {  get; set; }
        public int visibility {  get; set; }
        public string is_day {  get; set; }
    }
}

