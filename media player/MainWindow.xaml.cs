﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

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
            playerr.Volume = 0.5;
            //Hint.Visibility = Visibility.Hidden;
        }


        bool isvid = false;
        //is video loaded to player?
        private TimeSpan TotalTime;
        //duration of the video
        bool ismax = false;
        //is fullscreen?
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if(pausebtn.Content == "▶")
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

        //private void playerr_Loaded(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "mp4 videos|*.mp4|All files (*.*)|*.*";
        //    openFileDialog.ShowDialog();
        //    try
        //    {
        //        playerr.Source = new Uri(openFileDialog.FileName);
        //        playerr.Position = TimeSpan.Zero;
        //    }
        //    catch
        //    {
        //        MessageBox.Show("no file selected, press right mouse button to select");
        //    }
        //}

        private void playerr_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.RightButton==MouseButtonState.Pressed)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter= "mp4 videos|*.mp4|avi videos|*.avi|mkv videos|*.mkv|All files (*.*)|*.*";
                openFileDialog.ShowDialog();
                try
                {
                    playerr.Source = new Uri(openFileDialog.FileName);
                    playerr.Position = TimeSpan.Zero;
                    
                    playerr.Play();
                    
                        
                    
                }
                catch(Exception ex)
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
            dura.Maximum = playerr.NaturalDuration.TimeSpan.TotalSeconds;
            maxTime.Text = playerr.NaturalDuration.TimeSpan.ToString()+"  ";
            isvid = true;

            TotalTime = playerr.NaturalDuration.TimeSpan;

            // Create a timer that will update the counters and the time slider
            var timerVideoTime = new DispatcherTimer();
            timerVideoTime.Interval = TimeSpan.FromSeconds(1);
            timerVideoTime.Tick += new EventHandler(timer_Tick);
            timerVideoTime.Start();

        }

        private void dura_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isvid)
            {
                
                if(Mouse.LeftButton==MouseButtonState.Pressed)
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
            if (playerr.NaturalDuration.TimeSpan.TotalSeconds > 0)
            {
                if (TotalTime.TotalSeconds > 0)
                {
                    // Updating time slider
                    dura.Value = playerr.Position.TotalSeconds;
                }
            }
        }

        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            if(!ismax)
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
            if(isvid)
            {
                if (e.Key == Key.Right)
                {
                    if (dura.Maximum > 0 && (dura.Maximum - dura.Value) > 5)
                    {
                        dura.Value += 5;
                    }
                    else
                    {
                        dura.Value = dura.Maximum;
                    }
                }
                if (e.Key == Key.Left)
                {
                    if (dura.Maximum > 0 && (dura.Maximum - dura.Value) > 0)
                    {
                        dura.Value -= 5;
                    }
                    else
                    {
                        dura.Value = 0;
                    }
                }
            }
        }
    }
}
