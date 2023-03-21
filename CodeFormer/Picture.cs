using System;
using System.Windows.Media.Imaging;

namespace unblur_upscale
{
    public class Picture: IDisposable
    {
        // Track whether Dispose has been called.
        private bool disposed = false;

        public Picture instance { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string path { get; set; }
        public BitmapImage ImageData { get; set; }
        public bool isSelected { get; set; }
        public float fidelity_weight
        {
            get { return (float)Math.Round(fidelity_weight_, 2); }
            set { fidelity_weight_ = value; }
        }
        private float fidelity_weight_;
        public int upscale { get; set; }


        public Picture(BitmapImage bitmapImage, string path)
        {
            instance = this;
            isSelected = false;
            width = (int)bitmapImage.Width;
            height = (int)bitmapImage.Height;
            this.path = path;
            ImageData = bitmapImage;
            this.fidelity_weight = 0.7f;
            this.upscale = 1;
        }

        
        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(disposing: true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SuppressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    //component.Dispose();
                    ImageData = null;
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                ImageData = null;
                // If disposing is false,
                // only the following code is executed.
                //CloseHandle(handle);
                //handle = IntPtr.Zero;

                // Note disposing has been done.
                disposed = true;
            }
        }

        ~Picture()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(disposing: false) is optimal in terms of
            // readability and maintainability.
            Dispose(disposing: false);
        }
    }
}
