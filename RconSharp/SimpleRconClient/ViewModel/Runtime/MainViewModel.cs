using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RconSharp;
using SimpleRconClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleRconClient.ViewModel.Runtime
{
	public class MainViewModel : ViewModelBase, IMainViewModel
	{
		private string _host;
		private int _port;
		private string _password;
		private AsyncRelayCommand _connectCommand;
		private RelayCommand _disconnectCommand;
		private AsyncRelayCommand _executeCommand;
		private RelayCommand _clearLogsCommand;
		private string _messageBody;
		private bool _isWorking;
		private bool _isConnected;
		private bool _isError;
		private string _errorMessage;
		private bool _isPanelOpen;
		private ObservableCollection<CommunicationChunk> _communicationChunks;
		private RconMessenger _messenger;

		public MainViewModel(RconMessenger messenger)
		{
			_messenger = messenger;
			_communicationChunks = new ObservableCollection<CommunicationChunk>();
			_connectCommand = new AsyncRelayCommand(
				async () => 
				{
					IsWorking = true;
					ClearErrors();
					try
					{
						bool connected = await _messenger.ConnectAsync(_host, _port);
						if (connected)
						{
							bool authenticated = await _messenger.AuthenticateAsync(_password);
							if (authenticated)
							{
								IsConnected = true;
								IsPanelOpen = false;
							}
							else
							{
								ShowError("Wrong password. Unable to authenticate");
							}
						}
						else
						{
							ShowError("Connection error. Remote host refused the connection");
						}
					}
					catch (Exception)
					{
						ShowError("Communication error. Unable to reach remote host");
					}
					IsWorking = false;
				},
				() => 
				{ 
					return !_isConnected && !string.IsNullOrEmpty(_host) && !IsWorking; 
				});

			_disconnectCommand = new RelayCommand(
				() =>
				{
					IsWorking = true;
					
					_messenger.CloseConnection();

					IsWorking = false;
				},
				() => { return _isConnected && !IsWorking; });

			_executeCommand = new AsyncRelayCommand(
				async () =>
				{
					IsWorking = true;
					var chunk = new CommunicationChunk() 
					{
						Message = _messageBody,
						MessageSent = DateTime.Now
					};
					try
					{
						var response = await _messenger.ExecuteCommandAsync(_messageBody);
						chunk.Response = response;
						chunk.ResponseReceived = DateTime.Now;
						_communicationChunks.Add(chunk);
					}
					catch (Exception)
					{ 
						
					}
					IsWorking = false;
				},
				() => 
				{
					return _isConnected && !string.IsNullOrEmpty(_messageBody) && !IsWorking; 
				});

			_clearLogsCommand = new RelayCommand(
				() =>
				{
					_communicationChunks.Clear();
				},
				() => { return _communicationChunks.Count > 0; });
		}

		private void ShowError(string errorMessage)
		{
			IsError = true;
			ErrorMessage = errorMessage;
			_messenger.CloseConnection();
		}

		private void ClearErrors()
		{
			IsError = false;
			ErrorMessage = string.Empty;
		}

		public string Host
		{
			get { return _host; }
			set
			{
				_host = value;
				RaisePropertyChanged(() => Host);
			}
		}

		public int Port
		{
			get { return _port; }
			set
			{
				_port = value;
				RaisePropertyChanged(() => Port);
			}
		}

		public string Password
		{
			get { return _password; }
			set
			{
				_password = value;
				RaisePropertyChanged(() => Password);
			}
		}

		public ICommand ConnectCommand
		{
			get { return _connectCommand; }
		}

		public ICommand ExecuteCommand
		{
			get { return _executeCommand; }
		}

		public string MessageBody
		{
			get { return _messageBody; }
			set
			{
				_messageBody = value;
				RaisePropertyChanged(() => MessageBody);
			}
		}

		public IEnumerable<CommunicationChunk> CommunicationChunks
		{
			get { return _communicationChunks; }
		}


		public ICommand DisconnectCommand
		{
			get { return _disconnectCommand; }
		}

		public ICommand ClearLogsCommand
		{
			get { return _clearLogsCommand; }
		}

		public bool IsWorking
		{
			get { return _isWorking; }
			private set
			{
				_isWorking = value;
				RaisePropertyChanged(() => IsWorking);
			}
		}

		public bool IsConnected
		{
			get { return _isConnected; }
			private set
			{
				_isConnected = value;
				RaisePropertyChanged(() => IsConnected);
			}
		}


		public bool IsPanelOpen
		{
			get { return _isPanelOpen; }
			set
			{
				_isPanelOpen = value;
				RaisePropertyChanged(() => IsPanelOpen);
			}
		}


		public bool IsError
		{
			get { return _isError; }
			private set
			{
				_isError = value;
				RaisePropertyChanged(() => IsError);
			}
		}

		public string ErrorMessage
		{
			get { return _errorMessage; }
			private set
			{
				_errorMessage = value;
				RaisePropertyChanged(() => ErrorMessage);
			}
		}
	}
}
