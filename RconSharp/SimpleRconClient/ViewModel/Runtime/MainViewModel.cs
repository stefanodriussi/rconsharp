using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRconClient.ViewModel.Runtime
{
	public class MainViewModel : IMainViewModel
	{
		public string Host
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public int Port
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public string Password
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public System.Windows.Input.ICommand ConnectCommand
		{
			get { throw new NotImplementedException(); }
		}

		public System.Windows.Input.ICommand ExecuteCommand
		{
			get { throw new NotImplementedException(); }
		}

		public string MessageBody
		{
			get { throw new NotImplementedException(); }
		}

		public IEnumerable<Model.CommunicationChunk> CommunicationChunks
		{
			get { throw new NotImplementedException(); }
		}
	}
}
