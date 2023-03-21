using System;
using System.Windows.Input;
using unblur_upscale.ViewModel;

namespace unblur_upscale.Commands
{
    internal class ButtonCommand : ICommand
    {
        private ButtonViewModel buttonViewModel;

        public ButtonCommand(ButtonViewModel buttonViewModel)
        {
            this.buttonViewModel = buttonViewModel;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            buttonViewModel.OnExecute(parameter);
            //MessageBox.Show("ssssss");
        }
    }
}
