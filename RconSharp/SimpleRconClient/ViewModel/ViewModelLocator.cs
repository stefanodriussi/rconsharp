/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:SimpleRconClient"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using RconSharp;
using RconSharp.Net45;
using System;

namespace SimpleRconClient.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
		private readonly IKernel _kernel;
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
			_kernel = new StandardKernel();
			if (ViewModelBase.IsInDesignModeStatic)
			{
				// Create design time view services and models
				_kernel.Bind<IMainViewModel>().To<SimpleRconClient.ViewModel.Design.MainViewModel>();
			}
			else
			{
				_kernel.Load(AppDomain.CurrentDomain.GetAssemblies());
				_kernel.Bind<IMainViewModel>()
					.To<SimpleRconClient.ViewModel.Runtime.MainViewModel>()
					.InSingletonScope();
			}
        }

        public IMainViewModel Main
        {
            get
            {
				return _kernel.Get<IMainViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}