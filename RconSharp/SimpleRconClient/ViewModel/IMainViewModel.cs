using GalaSoft.MvvmLight;
using SimpleRconClient.Model;
using System.Collections.Generic;
using System.Windows.Input;

namespace SimpleRconClient.ViewModel
{
	public interface IMainViewModel
	{
		string Host { get; set; }
		int Port { get; set; }
		string Password { get; set; }
		ICommand ConnectCommand { get; }
		ICommand DisconnectCommand { get; }
		ICommand ExecuteCommand { get; }
		ICommand ClearLogsCommand { get; }
		bool IsWorking { get; }
		bool IsConnected { get; }
		string MessageBody { get; }
		IEnumerable<CommunicationChunk> CommunicationChunks { get; }
	}
}
