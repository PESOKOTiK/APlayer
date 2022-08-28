
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.IO;

namespace media_player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
            
            
            playerr.LoadedBehavior = MediaState.Manual;
            playerr.UnloadedBehavior = MediaState.Manual;
            playerr.Volume = 0.15;
            //Hint.Visibility = Visibility.Hidden;

        }

        public MainWindow(FileInfo file)
        {

            InitializeComponent();

            
            playerr.LoadedBehavior = MediaState.Manual;
            playerr.UnloadedBehavior = MediaState.Manual;
            playerr.Volume = 0.15;
            try
            {
                playerr.Source = new Uri(file.FullName);
                playerr.Position = TimeSpan.Zero;
                playerr.Play();
                Hint.Visibility = Visibility.Hidden;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            Hint.Visibility = Visibility.Hidden;

        }


        bool isvid = false;
        //is video loaded to player?
        private TimeSpan TotalTime;
        //duration of the video
        bool ismax = false;
        //is fullscreen?
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if (pausebtn.Content == "▶")
            {
                pausebtn.Content = "⏸";
                playerr.Play();
            }
            else
            {
                pausebtn.Content = "▶";

                playerr.Pause();
            }
        }


        private void playerr_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "All files (*.*)|*.*|videos|*.mp4;*.avi;*.mkv;*.mov|audios|*.mp3;*.vaw;*.ogg|images|*.jpg;*.png";
                openFileDialog.ShowDialog();
                try
                {
                    playerr.Source = new Uri(openFileDialog.FileName);
                    playerr.Position = TimeSpan.Zero;
                    playerr.Play();
                }
                catch
                {
                    MessageBox.Show("no file selected, press right mouse button to select");
                }
                finally
                {
                    Hint.Visibility = Visibility.Hidden;
                }
            }
        }

        private void rebtn_Click(object sender, RoutedEventArgs e)
        {
            playerr.Position = TimeSpan.Zero;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void playerr_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (playerr.NaturalDuration.HasTimeSpan)
            {
                dura.Maximum = playerr.NaturalDuration.TimeSpan.TotalSeconds;
                maxTime.Text = playerr.NaturalDuration.TimeSpan.ToString() + "  ";
                isvid = true;

                TotalTime = playerr.NaturalDuration.TimeSpan;

                // Create a timer that will update the counters and the time slider
                var timerVideoTime = new DispatcherTimer();
                timerVideoTime.Interval = TimeSpan.FromSeconds(1);
                timerVideoTime.Tick += new EventHandler(timer_Tick);
                timerVideoTime.Start();
            }

        }

        private void dura_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isvid)
            {

                if (Mouse.LeftButton == MouseButtonState.Pressed && dura.IsMouseOver)
                {
                    //Thread.Sleep(130);
                    playerr.Pause();
                    playerr.Position = TimeSpan.FromSeconds(dura.Value);
                    curTime.Text = playerr.Position.ToString();
                    playerr.Play();
                }
                else
                {
                    playerr.Position = TimeSpan.FromSeconds(dura.Value);
                    curTime.Text = playerr.Position.ToString(@"hh\:mm\:ss");
                }

            }
        }
        void timer_Tick(object sender, EventArgs e)
        {
            // Check if the movie finished calculate it's total time
            if (playerr.NaturalDuration.HasTimeSpan)
            {
                if (playerr.NaturalDuration.TimeSpan.TotalSeconds > 0)
                {
                    if (TotalTime.TotalSeconds > 0)
                    {
                        // Updating time slider
                        dura.Value = playerr.Position.TotalSeconds;
                    }
                }
            }
        }

        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (!ismax)
            {
                this.WindowState = WindowState.Maximized;
                ismax = true;
                fscr.Content = "⸬";
            }
            else
            {
                this.WindowState = WindowState.Normal;
                ismax = false;
                fscr.Content = "⛶";
            }
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (isvid)
            {
                if (e.Key == Key.Right)
                {
                    if (dura.Maximum > 0 && (dura.Maximum - dura.Value) > 5)
                    {
                        playerr.Pause();
                        dura.Value += 5;
                        playerr.Play();
                    }
                    else
                    {
                        dura.Value = dura.Maximum;
                    }
                }
                if (e.Key == Key.Space)
                {
                    if (pausebtn.Content == "▶")
                    {
                        pausebtn.Content = "⏸";
                        playerr.Play();
                    }
                    else
                    {
                        pausebtn.Content = "▶";

                        playerr.Pause();
                    }
                }
                if (e.Key == Key.Left)
                {
                    if (dura.Maximum > 0 && (dura.Maximum - dura.Value) > 0)
                    {
                        playerr.Pause();
                        dura.Value -= 5;
                        playerr.Play();
                    }
                    else
                    {
                        dura.Value = 0;
                    }
                }
                if (e.Key == Key.Down)
                {
                    if (volslider.Value >= 0.05)
                    {
                        volslider.Value -= 0.05;
                    }
                    else
                    {
                        volslider.Value = 0;
                    }
                }
                if (e.Key == Key.Up)
                {
                    if (volslider.Value <= 0.95)
                    {
                        volslider.Value += 0.05;
                    }
                    else
                    {
                        volslider.Value = 1;
                    }
                }
            }
        }

    }
}

