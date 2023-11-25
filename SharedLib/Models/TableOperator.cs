namespace SharedLib.Models
{
    public class TableOperator
    {
        public int Id { get; set; }
        public Table Table { get; set; }
        public int TableId { get; set; }
        public Operator Operator { get; set; }
        public int OperatorId { get; set; }
    }
}