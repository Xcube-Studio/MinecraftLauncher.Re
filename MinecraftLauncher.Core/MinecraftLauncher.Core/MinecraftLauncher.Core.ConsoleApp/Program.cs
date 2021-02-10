using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using MinecraftLauncher.Core.Standard.Service.Local;
using MinecraftLauncher.Core.Authenticator.UI;
using MinecraftLauncher.Core.Standard.Items;
using MinecraftLauncher.Core.Standard.Enums;
using Newtonsoft.Json;

namespace MinecraftLauncher.Core.ConsoleApp
{
    class Program
    {
        public static MinecraftLauncher minecraftLauncher;

        public static ConsoleColor color;

        public static LaunchConfiguration launchConfiguration;

        public static AuthenticationData authenticationData;

        static void Main(string[] args)
        {
            InitializeComponent();

            ProgressWindow.Load();

            /*
            using (minecraftLauncher = new MinecraftLauncher(launchConfiguration, authenticationData))
            {
                minecraftLauncher.ProcessOutputDateReceived += MinecraftLauncher_ProcessOutputDateReceived;
                minecraftLauncher.ProcessCrashed += delegate
                {
                    Console.WriteLine($"[{DateTime.Now}] [MinecraftLauncher.Core.MinecraftLauncher thread/INFO] Minecraft Process Crashed...");
                };
                minecraftLauncher.ProcessExited += delegate
                {
                    Console.WriteLine($"[{DateTime.Now}] [MinecraftLauncher.Core.MinecraftLauncher thread/INFO] Minecraft Process Exited...");
                };
                minecraftLauncher.LaunchStopped += delegate
                {
                    Console.Title = $"Minecraft.Launcher.Core.Console [State: {minecraftLauncher.State}]";
                };
                minecraftLauncher.Start();
                minecraftLauncher.MinecraftProcess.WaitForExit();
            }
            */

            Console.Read();
        }

        private static void MinecraftLauncher_ProcessOutputDateReceived(Standard.Events.ProcessOutputDateReceivedEventArgs e)
        {
            if (e == null)
                return;

            switch (e.ProcessOutputDateType)
            {
                case ProcessOutputDateType.Fatal:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case ProcessOutputDateType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case ProcessOutputDateType.Warn:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case ProcessOutputDateType.Debug:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case ProcessOutputDateType.Info:
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case ProcessOutputDateType.Last:
                    Console.ForegroundColor = color;
                    break;
                default:
                    break;
            }

            Console.WriteLine(e.Data);
            color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Gray;

            RefreshTitle();
        }

        private static void InitializeComponent()
        {
            launchConfiguration = JsonConvert.DeserializeObject<LaunchConfiguration>(File.ReadAllText("LaunchConfiguration.json"));

            if (File.Exists("AuthenticationData.json"))
                authenticationData = JsonConvert.DeserializeObject<AuthenticationData>(File.ReadAllText("AuthenticationData.json"));
            else authenticationData = new AuthenticationData()
            {
                AccessToken = "xxxxxxxxxxxxx",
                UserName = "Steve",
                UserType = UserType.Legacy,
                Uuid = Uuid.Parse("111111111111")
            };

            Console.Title = "Minecraft.Launcher.Core.Console";
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();

            SetCurrentFont();
        }

        private static void RefreshTitle()
        {
            try
            {
                Console.Title = $"Minecraft.Launcher.Core.Console [State: {minecraftLauncher.State}]";
                if (!minecraftLauncher.MinecraftProcess.HasExited)
                    if (minecraftLauncher.MinecraftProcess.MainWindowHandle.ToInt32() != 0)
                    {
                        StringBuilder title = new StringBuilder(50);
                        GetWindowText(minecraftLauncher.MinecraftProcess.MainWindowHandle, title, 50);

                        Console.Title += $" [MainWindowTitle: {title}] [Process File: {minecraftLauncher.MinecraftProcess.StartInfo.FileName}] [Process Id: {minecraftLauncher.MinecraftProcess.Id}]";
                    }
                    else Console.Title += $" [Process File: {minecraftLauncher.MinecraftProcess.StartInfo.FileName}] [Process Id: {minecraftLauncher.MinecraftProcess.Id}]";
            }
            catch
            {
                Console.WriteLine($"[{DateTime.Now}] [MinecraftLauncher.Core.MinecraftLauncher thread/Error] Get Minecraft Process Info Error...");
            }
        }

        #region DllImport

        private const int FixedWidthTrueType = 54;
        private const int StandardOutputHandle = -11;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);


        private static readonly IntPtr ConsoleOutputHandle = GetStdHandle(StandardOutputHandle);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct FontInfo
        {
            internal int cbSize;
            internal int FontIndex;
            internal short FontWidth;
            public short FontSize;
            public int FontFamily;
            public int FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.wc, SizeConst = 32)]
            public string FontName;
        }

        public static FontInfo[] SetCurrentFont(string font = "Consolas", short fontSize = 0)
        {
            Console.WriteLine("Set Current Font: " + font);

            FontInfo before = new FontInfo
            {
                cbSize = Marshal.SizeOf<FontInfo>()
            };

            if (GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref before))
            {

                FontInfo set = new FontInfo
                {
                    cbSize = Marshal.SizeOf<FontInfo>(),
                    FontIndex = 0,
                    FontFamily = FixedWidthTrueType,
                    FontName = font,
                    FontWeight = 400,
                    FontSize = fontSize > 0 ? fontSize : before.FontSize
                };

                // Get some settings from current font.
                if (!SetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref set))
                {
                    var ex = Marshal.GetLastWin32Error();
                    Console.WriteLine("Set error " + ex);
                    throw new System.ComponentModel.Win32Exception(ex);
                }

                FontInfo after = new FontInfo
                {
                    cbSize = Marshal.SizeOf<FontInfo>()
                };
                GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref after);

                return new[] { before, set, after };
            }
            else
            {
                var er = Marshal.GetLastWin32Error();
                Console.WriteLine("Get error " + er);
                throw new System.ComponentModel.Win32Exception(er);
            }
        }

        [DllImport("user32")]
        public extern static int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        #endregion
    }
}
