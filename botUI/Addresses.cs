using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace botUI
{
    public class Addresses
    {

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);



        const uint WM_KEYDOWN = 0x0100;
        const uint WM_KEYUP = 0x0101;

        public static Process p = Process.GetProcessesByName("Xerazx-OTS")[0];
        static UInt32 Base = (UInt32)p.MainModule.BaseAddress;

        public static UInt32 LightAdr = Base + 0x62C11C;

        public static UInt32 XorAdr = Base + 0x4380D0;



        #region player
        public static UInt32 CID = Base + 0x5D5034;
        public static UInt32 LevelAdr = Base + 0x4380E8;  
        public static UInt32 ManaAdr = Base + 0x438100;
        public static UInt32 HealthAdr = Base + 0x5D5000;
        public static UInt32 ChatAdr = Base + 0x671F94; // offsets 7C,30,10,2C,0
        public static UInt32 NameAdr = Base + 0x62C25C;
        public static UInt32 OutfitAdr = NameAdr + 0x5C;
        public static UInt32 YposAdr = Base + 0x5D503C;
        #endregion


        #region Methods
        public static int getPlayerMP()
        {
            return MemReadEng.ReadInt32(XorAdr, p.Handle) ^ MemReadEng.ReadInt32(ManaAdr, p.Handle);
        }
        public static int getPlayerHP()
        {
            return MemReadEng.ReadInt32(XorAdr, p.Handle) ^ MemReadEng.ReadInt32(HealthAdr, p.Handle);
        }
        public static int getPlayerLevel()
        {
            return MemReadEng.ReadInt32(LevelAdr, p.Handle);
        }
        public static int getPlayerYPos()
        {
            return MemReadEng.ReadInt32(YposAdr, p.Handle);
        }
        public static int getPlayerID()
        {
            return MemReadEng.ReadInt32(CID, p.Handle);
        }
        public static void SendIngameMessage(string msg)
        {
            MemReadEng.WriteStringPointer(p, ChatAdr, msg, new UInt32[] { 0x7C, 0x30, 0x10, 0x2C }, (UInt32)msg.Length);
            PostMessage(p.MainWindowHandle, WM_KEYDOWN, (int)Keys.Enter, 0);

        }
        public static void SendIngameWalk(string dir)
        {
            if (dir == "Up") PostMessage(p.MainWindowHandle, WM_KEYDOWN, (int)Keys.Up, 0);
            else if (dir == "Down") PostMessage(p.MainWindowHandle, WM_KEYDOWN, (int)Keys.Down, 0);

        }
        public static void clearAdr()
        {
            MemReadEng.WriteStringPointer(p, ChatAdr, "\n ", new UInt32[] { 0x7C, 0x30, 0x10, 0x2C }, (UInt32)15);
        }

        public static void IssueRelog()
        {

            PostMessage(p.MainWindowHandle, WM_KEYDOWN, (int)Keys.Shift, 0);
            PostMessage(p.MainWindowHandle, WM_KEYDOWN, (int)Keys.Q, 0);
            System.Threading.Thread.Sleep(150);
            PostMessage(p.MainWindowHandle, WM_KEYDOWN, (int)Keys.Enter, 0);
        }
        #endregion



        #region proc related
        public static IntPtr GetProcessHandle()
        {
            return p.Handle;
        }

        public static UInt32 GetProcessBase(string baseadress)
        {
            UInt32 baseadr = 0x0;
            foreach (ProcessModule m in p.Modules)
            {
                if (m.FileName.Contains(baseadress))
                {
                    baseadr = (UInt32)m.BaseAddress;
                    Console.WriteLine($"Found baseadr for {baseadress}, now assigning. {baseadress + 0xCB158}");
                    break;
                }
            }
            return baseadr;
        }

        public static UInt32 getPlayerIdAdr()
        {
                for (int i = 0; i < 100; i++)
                {
                    Console.WriteLine("Entry: " + i + " " + MemReadEng.ReadString((UInt32)p.MainModule.BaseAddress + 0x62C0A4 + (0xDC * i), p.Handle) + " CID: " + MemReadEng.ReadInt32((UInt32)p.MainModule.BaseAddress + (0x62C0A4 + (0xDC * i)) - 0x4, p.Handle));
                if (MemReadEng.ReadInt32((UInt32)p.MainModule.BaseAddress + 0x62C0A4 + (0xDC * i) - 0x4, p.Handle) == Addresses.getPlayerID())
                    return (UInt32)p.MainModule.BaseAddress + 0x62C0A4 + (0xDC * (UInt32)i);
                }
            return 0x0;
        }
        #endregion


    }
}
