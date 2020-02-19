using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace botUI
{
    class MemReadEng
    {
        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);
        [DllImport("Kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr handle, int lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesWritten);

        public static byte[] ReadBytes(IntPtr Handle, Int64 Address, uint BytesToRead)
        {
            IntPtr bytesRead;
            byte[] buffer = new byte[BytesToRead];
            ReadProcessMemory(Handle, new IntPtr(Address), buffer, BytesToRead, out bytesRead);
            return buffer;
        }

        public static int ReadInt32(Int64 Address, IntPtr Handle)
        {
            return BitConverter.ToInt32(ReadBytes(Handle, Address, 4), 0);
        }

        public static string StringReadPointer(Process _proc, UInt32 adr, UInt32[] offsets, uint length)
        {

            IntPtr ptrBytesRead;
            byte[] buffer = new byte[length];
            ReadProcessMemory(_proc.Handle, new IntPtr(adr + (UInt32)_proc.MainModule.BaseAddress), buffer, length, out ptrBytesRead);
            Int32 Res = BitConverter.ToInt32(buffer, 0);
            Int32 i = 0;

            foreach (Int32 offset in offsets)
            {
                if (i != offsets.Length)
                {
                    ReadProcessMemory(_proc.Handle, new IntPtr(Res + offset), buffer, length, out ptrBytesRead);
                    Res = BitConverter.ToInt32(buffer, 0);
                    i++;
                }
            }

            ReadProcessMemory(_proc.Handle, new IntPtr(Res + offsets[offsets.Length - 1]), buffer, length, out ptrBytesRead);
            return Encoding.Default.GetString(buffer);
        }
        public static string ReadString(long Address, IntPtr Handle, uint length = 32)
        {
            return ASCIIEncoding.Default.GetString(ReadBytes(Handle, Address, length)).Split('\0')[0];
        }

        public static void WriteString(IntPtr handle, UInt32 address, string value)
        {
            int written;
            byte[] data = Encoding.Default.GetBytes(value);

             WriteProcessMemory(handle, (int)address, data, data.Length, out written).ToString();
        }

        public static void WriteMemInt(IntPtr hProc, UInt32 address, long v)
        {
            var val = new byte[] { (byte)v };
            int wtf = 0;
            WriteProcessMemory(hProc, (int)address, val, (int)val.LongLength, out wtf);
        }


        public static void WriteStringPointer(Process _proc, UInt32 adr, string value, UInt32[] offsets, UInt32 length)
        {
            int written;
            byte[] buffer = new byte[length];
            byte[] data = Encoding.Default.GetBytes(value);
            IntPtr ptrBytesRead;
            ReadProcessMemory(_proc.Handle, new IntPtr(adr), buffer, length, out ptrBytesRead);
            Int32 Res = BitConverter.ToInt32(buffer, 0);
            Console.WriteLine(Res);
            Int32 i = 0;

            foreach (UInt32 offset in offsets)
            {
                ReadProcessMemory(_proc.Handle, new IntPtr(Res + offset), buffer, length, out ptrBytesRead);
                Res = BitConverter.ToInt32(buffer, 0);
                i++;
            }
            WriteProcessMemory(_proc.Handle, (int)Res, data, data.Length, out written);
        }
    }
}