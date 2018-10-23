using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfClient.Model
{
    public class CommandHandler : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public CommandHandler(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
             //bool result=false;
             bool res1 = parameter!=null;
             bool res2 = this.canExecute != null;
             bool res3 = execute != null;
              //result =!(this.canExecute == null && this.canExecute(parameter));
            return res1 && res2 && res3;
           // return true;
           //return (this.canExecute == null || this.canExecute(parameter));
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
