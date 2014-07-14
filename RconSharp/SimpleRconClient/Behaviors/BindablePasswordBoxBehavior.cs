using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace SimpleRconClient.Behaviors
{
	public class BindablePasswordBoxBehavior : Behavior<PasswordBox>
	{
		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.PasswordChanged += (s, e) =>
			{
				var pb = s as PasswordBox;
				SecureString ss = new SecureString();
				Array.ForEach(pb.Password.ToCharArray(), ss.AppendChar);
				IntPtr valuePtr = IntPtr.Zero;
				try
				{
					valuePtr = Marshal.SecureStringToGlobalAllocUnicode(ss);
					(pb.DataContext as IHavePassword).Password = Marshal.PtrToStringUni(valuePtr);
				}
				finally
				{
					Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
				}
			};
		}
	}
}
