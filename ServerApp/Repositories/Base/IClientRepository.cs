using SharedLib.Models;

namespace ServerApp.Repositories.Base
{
    public interface IClientRepository
    {
        public IEnumerable<Client> GetAll();
        public void Add(Client order);
    }
}