using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using System.Linq;
using MessageBox = System.Windows.Forms.MessageBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace unblur_upscale
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow instance = null;
        private const string FilePath = "data.txt";

        private List<Picture> pictures = new List<Picture>();
        private List<Picture> selectedPictures = new List<Picture>();
        private string app_PathValue;
        private string input_DirValue;
        private string output_DirValue;
        private string output_DirSuffixValue="";  // add the suffix its a temporary folder or path optional
        // output_DirValue\output_DirSuffixValue
        

        private int successfullyProcessed = 0; // count  the number of successfully processed images

        Process process;
        StreamWriter input;


        public MainWindow()
        {
            InitializeComponent();
            instance = this;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Started");

            if (File.Exists(FilePath))
            {
                var data = File.ReadAllLines(FilePath);
                if (data.Length >= 3)
                {
                    app_PathValue = data[0];
                    input_DirValue = data[1];
                    output_DirValue = data[2];
                    textbox_app_path.Text = app_PathValue;
                    textbox_input_dir.Text = input_DirValue;
                    textbox_output_dir.Text = output_DirValue;
                }

                loadScanForPictures();
            }

            
        }

        void loadScanForPictures()
        {
            pictures.Clear();
            selectedPictures.Clear();
            listView_pictures.ItemsSource = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(pictures);
            pictures = new List<Picture>();
            

            if (textbox_input_dir.Text != "")
            {
                string folderPath = textbox_input_dir.Text; // The path to the folder containing the pictures
                                                             //Directory dir = new Directory().;
                Debug.WriteLine("cehcking");
                if (Directory.Exists(folderPath))
                {
                    folderPath = folderPath + "\\";
                    string[] filePaths = Directory.GetFiles(folderPath , "*.*").Where(file => 
                        file.ToLower().EndsWith(".jpg") ||
                        file.ToLower().EndsWith(".jpeg") ||
                        file.ToLower().EndsWith(".png") ).ToArray();// Get a list of JPEG and PNG files in the folder

                    Debug.WriteLine("path exists: "+ folderPath);

                    List<BitmapImage> images = new List<BitmapImage>(); // Create a list to hold the BitmapImage objects

                    foreach (string filePath in filePaths)
                    {
                        //Debug.WriteLine("image loop");
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(filePath);
                        bitmapImage.EndInit();
                        //images.Add(bitmapImage);

                        pictures.Add(new Picture(bitmapImage, filePath) );
                    }
                    listView_pictures.ItemsSource = pictures;
                }
            }
        }

        private void button_start_Click(object sender, RoutedEventArgs e)
        {
            if (app_PathValue == null || app_PathValue == "")
            {
                MessageBox.Show("Please set python app runner file (.py file)");
                return;
            }
            if (!File.Exists(app_PathValue))
            {
                MessageBox.Show("Some problem this path to app runner dont exists, '" + app_PathValue + "'");
                return;
            }


            if (input_DirValue == null || input_DirValue == "")
            {
                MessageBox.Show("Please set input directory.");
                return;
            }
            if (!Directory.Exists(input_DirValue))
            {
                MessageBox.Show("Some problem this input directory dont exists, '" + input_DirValue + "'");
                return;
            }


            if (output_DirValue == null || output_DirValue == "")
            {
                MessageBox.Show("Please set output directory.");
                return;
            }
            if (!Directory.Exists(output_DirValue))
            {
                MessageBox.Show("Some problem this output directory dont exists, '"+output_DirValue+"'");
                return;
            }


            if (selectedPictures.Count == 0)
            {
                MessageBox.Show("No images selected, please select images to proccess.");
                return;
            }

            checkbox_all.IsEnabled = false;
            button_start.IsEnabled = false;


            Task.Run(() => {

                DateTime now;
                string final_output_folder = output_DirValue;

                if (output_DirSuffixValue != "")
                    final_output_folder += "\\" + output_DirSuffixValue; // add the suffix its a temporary folder or path for this task

                /*process.Start();
                process.StandardInput.WriteLine("python your_python_script.py");
                process.StandardInput.Flush();
                process.StandardInput.Close();
                process.WaitForExit();*/

                 
                try
                {
                    if (process != null && process.HasExited)
                    {
                        // Exited process..
                        process.Kill();
                        process.Close();
                        process.Dispose();
                        process = null;
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }

                    if (process != null)
                    {
                        // Exited process..
                        process.Kill();
                        process.Close();
                        process.Dispose();
                        process = null;
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                }
                catch(InvalidOperationException e)
                {

                }
                process = null;

                process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.EnableRaisingEvents = true;

                process.OutputDataReceived += (sender, args) => Display(args.Data, "Print");
                process.ErrorDataReceived += (sender, args) => Display(args.Data, "Error");

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // Update the UI on the main thread
                Dispatcher.Invoke(() =>
                {
                    label_status.Content = "Started working (0/" + selectedPictures.Count + ")..";
                });

                input = process.StandardInput;


                input.WriteLine("cd \"C:\\Users\\rosti\\CodeFormer\"");
                input.WriteLine("conda activate codeformer");

                
                foreach (Picture pic in selectedPictures)
                {
                    //input.WriteLine("python \"C:\\Users\\rosti\\CodeFormer\\inference_codeformer.py\" -w 0.7 -s 1 --input_path \"D:\\pic\\hguybiu.png\"");
                    input.WriteLine("python \""+app_PathValue+"\" -w " + pic .fidelity_weight + " -s " + pic.upscale + " --input_path \"" + pic.path + "\" --output_path \"" + final_output_folder + "\"");
                }


                input.Close();

                process.WaitForExit(); //you need this in order to flush the output buffer

                //string error = process.StandardError.ReadToEnd();
                process.Close();

                // Update the UI on the main thread
                Dispatcher.Invoke(() => {
                    MessageBox.Show("Finished! successfully processed "+ successfullyProcessed+"/"+selectedPictures.Count+".");
                    button_start.IsEnabled = true;
                    checkbox_all.IsChecked = false;
                    successfullyProcessed = 0; // reset the counter
                    label_status.Content = "";

                    selectedPictures.Clear();
                    foreach (Picture pic in pictures)
                        pic.isSelected = false;

                    listView_pictures.ItemsSource = null;
                    listView_pictures.ItemsSource = pictures;
                });
            });
        }

        void Display(string output, string type)
        {
            Debug.WriteLine(type+": "+output);

            if (type == "Print")
            {
               if(output != null && output.Contains("All results are saved in"))
                {
                    //MessageBox.Show("Finished");
                    successfullyProcessed++; // count successfully Processed image
                    
                    // Update the UI on the main thread
                    Dispatcher.Invoke(() =>
                    {
                        label_status.Content = "Started working (" + successfullyProcessed + "/" + selectedPictures.Count + ")..";
                    });
                }
            }
        }


        private void dir_python_app_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of the OpenFileDialog class
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            // Set the title and filter for the dialog
            openFileDialog.Title = "Select the app runner";
            openFileDialog.Filter = "Python file (*.py)|*.py";

            // Show the dialog and get the result
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                // The user selected a file, so get the file path
                app_PathValue = openFileDialog.FileName;
                // Do something with the file path, such as displaying it in a text box
                textbox_app_path.Text = app_PathValue;
            }
        }

        private void button_dir_input_Click(object sender, RoutedEventArgs e)
        {
            /// Create a new instance of FolderBrowserDialog
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            // Set the properties of the dialog box
            folderDialog.Description = "Select a Input folder to scan for images.";
            folderDialog.ShowNewFolderButton = false; // Optional: Hide the "New Folder" button

            // Show the dialog box and get the result
            DialogResult result = folderDialog.ShowDialog();
            Debug.WriteLine("wait to select folder");
            // Check if the user clicked OK
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                input_DirValue = folderDialog.SelectedPath;
                // Do something with the selected folder path
                textbox_input_dir.Text = input_DirValue;
                Debug.WriteLine("selected");
                loadScanForPictures();
            }
        }

        private void button_dir_output_Click(object sender, RoutedEventArgs e)
        {
            /// Create a new instance of FolderBrowserDialog
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            // Set the properties of the dialog box
            folderDialog.Description = "Select Output folder.";
            folderDialog.ShowNewFolderButton = false; // Optional: Hide the "New Folder" button

            // Show the dialog box and get the result
            DialogResult result = folderDialog.ShowDialog();

            // Check if the user clicked OK
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                output_DirValue = folderDialog.SelectedPath;
                // Do something with the selected folder path
                textbox_output_dir.Text = output_DirValue;
            }
        }

        public void clicked_Picture(Picture pic)
        {
            if (button_start.IsEnabled)
            {  // only if enabled allow to add or remove items, because if not means in process
                if (!selectedPictures.Contains(pic))
                {
                    pic.isSelected = true;
                    selectedPictures.Add(pic);
                }
                else
                {
                    pic.isSelected = false;
                    selectedPictures.Remove(pic);
                    if ((bool)checkbox_all.IsChecked)
                        checkbox_all.IsChecked = false;
                }

                listView_pictures.ItemsSource = null;
                listView_pictures.ItemsSource = pictures;
                Debug.WriteLine("Selected: " + selectedPictures.Count);
            }
        }

        private void checkbox_all_Click(object sender, RoutedEventArgs e)
        {
            selectedPictures.Clear();

            if (pictures != null && pictures.Count>0)
            {
                if ((bool)checkbox_all.IsChecked)
                {
                    foreach (Picture picture in pictures)
                        picture.isSelected = true;

                    selectedPictures.AddRange(pictures);
                }
                else
                {
                    foreach (Picture picture in pictures)
                        picture.isSelected = false;
                }

                listView_pictures.ItemsSource = null;
                listView_pictures.ItemsSource = pictures;
                Debug.WriteLine("Selected: " + selectedPictures.Count);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            File.WriteAllLines(FilePath, new[] { app_PathValue==null?"":app_PathValue, input_DirValue == null ? "" : input_DirValue, output_DirValue == null ? "" : output_DirValue });
        }

        private void checkbox_suffix_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)checkbox_suffix.IsChecked)
            {
                DateTime now = DateTime.Now;
                output_DirSuffixValue = now.Ticks.ToString();
                textbox_suffix_output.Text = output_DirSuffixValue;
                textbox_suffix_output.IsEnabled = true;
            }
            else
            {
                output_DirSuffixValue = "";
                textbox_suffix_output.Text = output_DirSuffixValue;
                textbox_suffix_output.IsEnabled = false;
            }
        }

        private void textbox_suffix_output_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            output_DirSuffixValue = textbox_suffix_output.Text;
        }
    }
}
