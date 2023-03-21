using System.Windows;
using unblur_upscale.Commands;

namespace unblur_upscale.ViewModel
{
    internal class ButtonViewModel
    {
        public ButtonCommand buttonCommand { get; set; }
        public string whatButton { get; set; }


        public ButtonViewModel()
        {
            buttonCommand = new ButtonCommand(this);
        }

        public void OnExecute()
        {
            //this.buttonCommand.Execute();
            MessageBox.Show("executed with no parameter");
        }

        internal void OnExecute(object parameter)
        {
            if (whatButton.Equals("pictureSelector"))
            {
                Picture pic = (Picture)parameter;
                if (MainWindow.instance != null)
                    MainWindow.instance.clicked_Picture(pic);
            }
        }
    }
}
