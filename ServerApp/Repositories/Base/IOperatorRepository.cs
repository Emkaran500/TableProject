using SharedLib.Models;

namespace ServerApp.Repositories.Base
{
    public interface IOperatorRepository
    {
        public Operator GetById(int id);
    }
}