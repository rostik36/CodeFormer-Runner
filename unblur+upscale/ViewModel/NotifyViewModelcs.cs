using System.Windows.Forms;
using System.Windows.Input;
using unblur_upscale.Commands;

namespace unblur_upscale.ViewModel
{
    public class NotifyViewModel : ViewModelBase
    {
        public ICommand NotifyCommand { get; }

        public NotifyViewModel(NotifyIcon notifyIcon)
        {
            NotifyCommand = new NotifyCommand(notifyIcon);
        }
    }
}
