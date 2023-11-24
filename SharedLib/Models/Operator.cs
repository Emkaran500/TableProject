namespace SharedLib.Models
{
    public class Operator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Table Table { get; set; }
        public int TableId { get; set; }
    }
}