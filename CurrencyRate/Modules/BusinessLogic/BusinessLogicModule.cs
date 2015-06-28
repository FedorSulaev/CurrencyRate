using CurrencyRate.Modules.BusinessLogic.Services;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace CurrencyRate.Modules.BusinessLogic
{
	class BusinessLogicModule : IModule
	{
		private readonly IUnityContainer _container;

		public BusinessLogicModule(IUnityContainer container)
		{
			_container = container;
		}

		public void Initialize()
		{
			_container.RegisterType<ApiService>(new ContainerControlledLifetimeManager());
		}
	}
}
