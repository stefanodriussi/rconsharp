using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRconClient.ViewModel.Design
{
	public class MainViewModel : IMainViewModel
	{
		private IEnumerable<Model.CommunicationChunk> _communicationChunks;
		public MainViewModel()
		{
			_communicationChunks = new List<SimpleRconClient.Model.CommunicationChunk>() 
			{
				new SimpleRconClient.Model.CommunicationChunk()
				{
					Message = "original message. don't know if it will be displayed",
					MessageSent = DateTime.Now,
					Response = "Respose to command 1 sent from this simple client",
					ResponseReceived = DateTime.Now
				},
				new SimpleRconClient.Model.CommunicationChunk()
				{
					Message = "original message. don't know if it will be displayed",
					MessageSent = DateTime.Now,
					Response = "Respose to command 1 sent from this simple client but this time this string will be slightly longer",
					ResponseReceived = DateTime.Now
				},
				new SimpleRconClient.Model.CommunicationChunk()
				{
					Message = "original message. don't know if it will be displayed",
					MessageSent = DateTime.Now,
					Response = "Respose to command 1 sent from this simple client\nThis response is splitted\nbetween multiple lines\nof text\nlet's see how\nit will display\non the item\nwith so many lines\nok i admit it\ni don't know what to write",
					ResponseReceived = DateTime.Now
				},

			};
		}
		public string Host
		{
			get
			{
				return "myserver.address.com";
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
				return 12345;
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
				return "supersecretpassword";
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
			get { return "minecraft command"; }
			set { throw new NotImplementedException(); }
		}

		public IEnumerable<Model.CommunicationChunk> CommunicationChunks
		{
			get { return _communicationChunks; }
		}


		public System.Windows.Input.ICommand DisconnectCommand
		{
			get { throw new NotImplementedException(); }
		}

		public System.Windows.Input.ICommand ClearLogsCommand
		{
			get { throw new NotImplementedException(); }
		}

		public bool IsWorking
		{
			get { return true; }
		}

		public bool IsConnected
		{
			get { return true; }
		}


		public bool IsPanelOpen
		{
			get
			{
				return false;
			}
			set
			{
				throw new NotImplementedException();
			}
		}


		public bool IsError
		{
			get { return true; }
		}

		public string ErrorMessage
		{
			get { return "Connection error"; }
		}
	}
}
