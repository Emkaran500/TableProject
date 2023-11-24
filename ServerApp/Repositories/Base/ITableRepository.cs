using System.Collections.ObjectModel;
using SharedLib.Models;

namespace ServerApp.Repositories.Base
{
    public interface ITableRepository
    {
        public ObservableCollection<Client> GetClients();

        public Operator GetOperator();

        public int GetId();
    }
}