using System.Collections.ObjectModel;
using ServerApp.Repositories.Base;
using SharedLib.Models;

namespace ServerApp.Repositories
{
    public class TableEFRepository : ITableRepository
    {
        public ObservableCollection<Client> GetClients()
        {
            throw new NotImplementedException();
        }

        public int GetId()
        {
            throw new NotImplementedException();
        }

        public Operator GetOperator()
        {
            throw new NotImplementedException();
        }
    }
}