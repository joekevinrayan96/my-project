using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CameraProject
{
    public class MainVM : ObservableObject, IDisposable
    {
        private CameraHandler _cameraHandler;
        private BitmapImage _cameraImage;
        //private Dispatcher _dispatcher;

        public MainVM()
        {
            //_dispatcher = Dispatcher.CurrentDispatcher;
            _cameraHandler = new CameraHandler();
            _cameraHandler.InitializeCamera();
            _cameraHandler.NewFrameReceived += OnNewFrameReceived;
        }
        
        private void OnNewFrameReceived(object? sender, BitmapImage e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CameraImage = e;
                });
                //_dispatcher.Invoke(() =>
                //{
                //});
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        

        public BitmapImage CameraImage
        {
            get => _cameraImage;
            set
            {
                //if (_cameraImage != value)
                //{
                //}
                    SetProperty(ref _cameraImage, value);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
