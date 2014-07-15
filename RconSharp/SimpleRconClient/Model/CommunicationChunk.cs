using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRconClient.Model
{
	public class CommunicationChunk
	{
		public string Message { get; set; }
		public string Response { get; set; }
		public DateTime MessageSent { get; set; }
		public DateTime ResponseReceived { get; set; }
	}
}
