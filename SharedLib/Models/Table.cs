using System.Collections.ObjectModel;

namespace SharedLib.Models
{
    public class Table
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public TableOperator TableOperator { get; set; }
        public ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();
    }
}
