using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CameraProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private FilterInfoCollection _videoDevices;
        private VideoCaptureDevice _videoSource;
        private Bitmap currentFrame;
        private readonly object currentFrameLock = new object();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVM();

            //if (_videoSource != null && _videoSource.IsRunning)
            //{
            //    _videoSource.SignalToStop();
            //    currentFrame?.Dispose();
            //}

            //_videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            //if (_videoDevices.Count > 0)
            //{
            //    foreach (FilterInfo deviceInfo in _videoDevices)
            //    {
            //        try
            //        {
            //            VideoCaptureDevice device = new VideoCaptureDevice(deviceInfo.MonikerString);
            //            device.NewFrame += VideoSource_NewFrame;
            //            device.Start();

            //            if (device.IsRunning)
            //            {
            //                _videoSource = device;
            //                break;
            //            }
            //            else
            //            {
            //                device.SignalToStop();
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show("Exception while starting device " + deviceInfo.Name + ": " + ex.ToString());
            //        }
            //    }

            //    if (_videoSource == null)
            //    {
            //        MessageBox.Show("No active camera detected");
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("No camera detected");
            //}
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            using (Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone())
            {
                //lock (currentFrameLock)
                //{
                //}
                //BitmapImage bitmapImage = BitmapImageToSource(bitmap);
                this.Dispatcher.Invoke(() =>
                {
                    currentFrame?.Dispose();
                    currentFrame = (Bitmap)bitmap.Clone();
                    CameraView.Source = BitmapImageToSource(bitmap);
                });

                //Application.Current.Dispatcher.Invoke(() =>
                //{
                //    NewFrameReceived?.Invoke(this, bitmapImage);
                //});
                //_dispatcher.Invoke(() =>
                //{
                //    NewFrameReceived?.Invoke(this, bitmapImage);
                //});
            }
        }

        public void StopCamera()
        {
            if (_videoSource != null)
            {
                if (_videoSource.IsRunning)
                {
                    _videoSource.SignalToStop();
                }
            }

            currentFrame?.Dispose();
            currentFrame = null;
        }

        public Bitmap GetCurrentFrame()
        {
            lock (currentFrameLock)
            {
                return (Bitmap)currentFrame?.Clone();
            }
        }

        private BitmapImage BitmapImageToSource(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        public void Dispose()
        {
            StopCamera();
            if (_videoSource != null)
            {
                _videoSource.NewFrame -= VideoSource_NewFrame;
            }
            currentFrame?.Dispose();
        }
    }
}
