using ServerApp.Repositories;
using ServerApp.Repositories.Base;
using SimpleInjector;

namespace ServerApp.Functions
{
    public class ServerDbFunctions
    {
        public static void RegisterContainer(Container container)
        {
            container.RegisterSingleton<ITableRepository, TableEFRepository>();
            container.RegisterSingleton<IClientRepository, ClientEFRepository>();
            container.RegisterSingleton<IOperatorRepository, OperatorEFRepository>();

            container.Verify();
        }
    }
}