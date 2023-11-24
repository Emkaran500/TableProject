using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLib.Models
{
    public class Client
    {
        public int Id { get; set; }
        public Table Table { get; set; }
        public int TableId { get; set; }
    }
}