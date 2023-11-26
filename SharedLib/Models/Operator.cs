namespace SharedLib.Models
{
    public class Operator
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
        public TableOperator TableOperator { get; set; }
    }
}