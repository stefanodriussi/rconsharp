using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleRconClient.ViewModel
{
	public class AsyncRelayCommand : ICommand
	{
		private readonly RelayCommand _internalCommand;

		private readonly Func<Task> _executeMethod;

		public event EventHandler CanExecuteChanged
		{
			add { _internalCommand.CanExecuteChanged += value; }
			remove { _internalCommand.CanExecuteChanged -= value; }
		}

		public AsyncRelayCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod)
		{
			if (executeMethod == null)
			{
				throw new ArgumentNullException("executeMethod");
			}

			_executeMethod = executeMethod;
			_internalCommand = new RelayCommand(() => { }, canExecuteMethod);
		}

		public AsyncRelayCommand(Func<Task> executeMethod) : this(executeMethod, () => true) { }

		public Task ExecuteAsync(object parameter)
		{
			return _executeMethod();
		}

		void ICommand.Execute(object parameter)
		{
			ExecuteAsync(parameter);
		}

		public bool CanExecute(object parameter)
		{
			return _internalCommand.CanExecute(parameter);
		}

		public void RaiseCanExecuteChanged()
		{
			_internalCommand.RaiseCanExecuteChanged();
		}
	}

	public class AsyncRelayCommand<T> : ICommand
	{
		private readonly RelayCommand<T> _internalCommand;
		private readonly Func<T, Task> _executeMethod;

		public event EventHandler CanExecuteChanged
		{
			add { _internalCommand.CanExecuteChanged += value; }
			remove { _internalCommand.CanExecuteChanged -= value; }
		}

		public AsyncRelayCommand(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod)
		{
			if (executeMethod == null)
			{
				throw new ArgumentNullException("executeMethod");
			}

			_executeMethod = executeMethod;
			_internalCommand = new RelayCommand<T>(p => { }, canExecuteMethod);
		}

		public AsyncRelayCommand(Func<T, Task> executeMethod) : this(executeMethod, _ => true) { }

		void ICommand.Execute(object parameter)
		{
			if (parameter is T)
			{
				ExecuteAsync((T)parameter);
			}

			else throw new ArgumentException("Parameter should be of type " + typeof(T));
		}

		public Task ExecuteAsync(T parameter)
		{
			return _executeMethod(parameter);
		}

		public bool CanExecute(object parameter)
		{
			return _internalCommand.CanExecute((T)parameter);
		}

		public void RaiseCanExecuteChanged()
		{
			_internalCommand.RaiseCanExecuteChanged();
		}
	}
}
