using Ninject.Modules;
using RconSharp;
using RconSharp.Net45;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRconClient
{
	public class NinjectBindings : NinjectModule
	{
		public override void Load()
		{
			Kernel.Bind<INetworkSocket>().To<RconSocket>();
		}
	}
}
