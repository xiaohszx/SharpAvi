﻿using System.Windows;
using System.Windows.Forms;
using System;
using System.Windows.Interop;
using System.Collections.Generic;
using SharpAvi.Codecs;
using NAudio.Wave;

namespace SharpAvi.Sample
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, System.Windows.Forms.IWin32Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            InitAvailableCodecs();

            InitAvailableAudioSources();

            DataContext = this;

            WindowMoveBehavior.Attach(this);
        }

        private void InitAvailableCodecs()
        {
            var codecs = new List<CodecInfo>();
            codecs.Add(new CodecInfo(KnownFourCCs.Codecs.Uncompressed, "(none)"));
            codecs.Add(new CodecInfo(KnownFourCCs.Codecs.MotionJpeg, "Motion JPEG"));
            codecs.AddRange(Mpeg4VideoEncoder.GetAvailableCodecs());
            AvailableCodecs = codecs;
        }

        private void InitAvailableAudioSources()
        {
            var deviceList = new Dictionary<int, string>();
            deviceList.Add(-1, "(no sound)");
            for (var i = 0; i < WaveInEvent.DeviceCount; i++)
            {
                var caps = WaveInEvent.GetCapabilities(i);
                if (caps.SupportsWaveFormat(AudioWaveFormat))
                {
                    deviceList.Add(i, caps.ProductName);
                }
            }
            AvailableAudioSources = deviceList;
            SelectedAudioSourceIndex = -1;
        }


        public static readonly DependencyProperty FolderProperty =
            DependencyProperty.Register("Folder", typeof(string), typeof(SettingsWindow));

        public string Folder
        {
            get { return (string)GetValue(FolderProperty); }
            set { SetValue(FolderProperty, value); }
        }

        public static readonly DependencyProperty EncoderProperty =
            DependencyProperty.Register("Encoder", typeof(FourCC), typeof(SettingsWindow));

        public FourCC Encoder
        {
            get { return (FourCC)GetValue(EncoderProperty); }
            set { SetValue(EncoderProperty, value); }
        }

        public static readonly DependencyProperty QualityProperty =
            DependencyProperty.Register("Quality", typeof(int), typeof(SettingsWindow));

        public int Quality
        {
            get { return (int)GetValue(QualityProperty); }
            set { SetValue(QualityProperty, value); }
        }

        public static readonly DependencyProperty SelectedAudioSourceIndexProperty =
            DependencyProperty.Register("SelectedAudioSourceIndex", typeof(int), typeof(SettingsWindow));

        public int SelectedAudioSourceIndex
        {
            get { return (int)GetValue(SelectedAudioSourceIndexProperty); }
            set { SetValue(SelectedAudioSourceIndexProperty, value); }
        }

        public SupportedWaveFormat AudioWaveFormat
        {
            // TODO: Make wave format adjustable
            get { return SupportedWaveFormat.WAVE_FORMAT_44M16; }
        }

        public static readonly DependencyProperty MinimizeOnStartProperty =
            DependencyProperty.Register("MinimizeOnStart", typeof(bool), typeof(SettingsWindow));

        public bool MinimizeOnStart
        {
            get { return (bool)GetValue(MinimizeOnStartProperty); }
            set { SetValue(MinimizeOnStartProperty, value); }
        }

        public IEnumerable<CodecInfo> AvailableCodecs
        {
            get;
            private set;
        }

        public IEnumerable<KeyValuePair<int, string>> AvailableAudioSources
        {
            get;
            private set;
        }

        public bool Is64BitProcess
        {
            get { return IntPtr.Size == 8; }
        }


        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BrowseFolder_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog()
            {
                SelectedPath = Folder,
                ShowNewFolderButton = true,
                Description = "Select folder for screencasts"
            };

            if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                Folder = dlg.SelectedPath;
            }
        }

        IntPtr System.Windows.Forms.IWin32Window.Handle
        {
            get { return new WindowInteropHelper(this).Handle; }
        }
    }
}