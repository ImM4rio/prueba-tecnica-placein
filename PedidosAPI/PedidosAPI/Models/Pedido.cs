namespace PedidosAPI.Models
{
    public class Pedido
    {
        public string IdPedido {  get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Ubicacion { get; set;}
        public int Temperatura { get; set;}
        public int Humedad {  get; set;}

    }
}
