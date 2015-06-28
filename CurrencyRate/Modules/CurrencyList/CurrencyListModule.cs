using CurrencyRate.Constants;
using CurrencyRate.Modules.CurrencyList.ViewModels;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace CurrencyRate.Modules.CurrencyList
{
	class CurrencyListModule : IModule
	{
		private readonly IUnityContainer _container;
		private readonly IRegionManager _regionManager;

		public CurrencyListModule(IUnityContainer container, IRegionManager regionManager)
		{
			_container = container;
			_regionManager = regionManager;
		}

		public void Initialize()
		{
			_container.RegisterType<object, Views.CurrencyList>(ViewNames.CurrencyListViewName,
				new ContainerControlledLifetimeManager());
			_container.RegisterType<CurrencyListViewModel>(new ContainerControlledLifetimeManager());
			_regionManager.RequestNavigate(RegionNames.ShellMainRegionName, ViewNames.CurrencyListViewName);
		}
	}
}
