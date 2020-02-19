using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace botUI
{
    public partial class Form1 : Form
    {
        Process proc;
        public Form1()
        {
            InitializeComponent();
            onAttachProcess();
            for(int i = 0; i < 100; i++)
            {
                Console.WriteLine("Entry: " + i + " " + MemReadEng.ReadString((UInt32)proc.MainModule.BaseAddress + 0x62C0A4 + (0xDC *i), proc.Handle) + " CID: " + MemReadEng.ReadInt32((UInt32)proc.MainModule.BaseAddress + (0x62C0A4 + (0xDC * i)) - 0x4, proc.Handle));
                if(MemReadEng.ReadInt32((UInt32)proc.MainModule.BaseAddress + 0x62C0A4 + (0xDC * i) - 0x4, proc.Handle) == Addresses.getPlayerID()) Text = MemReadEng.ReadString((UInt32)proc.MainModule.BaseAddress + 0x62C0A4 + (0xDC * i), proc.Handle);
            }
        }

        bool onAttachProcess()
        {
            try
            {
               
                proc = Process.GetProcessesByName("Xerazx-OTS")[0];
                timer1.Start();
                return true;
            }
            catch
            {
                Text = "Can't find HighExp Client.";
                return false;
            }
        }

        private void Settings_click(object sender, EventArgs e)
        {
            if (onAttachProcess())
            {
                SettingsForm SF = new SettingsForm();
                SF.ShowDialog();
            }
            else
                MessageBox.Show("Please launch client first.", "Message");
        }

        private void Support_Click(object sender, EventArgs e)
        {
            if (onAttachProcess())
            {
                SupportForm SF = new SupportForm();
                SF.ShowDialog();
            }
            else
                MessageBox.Show("Please launch client first.", "Message");
           

        }

        private void Debugger_Click(object sender, EventArgs e)
        {
            DebuggerForm DF = new DebuggerForm();
            DF.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void Attacker_Click(object sender, EventArgs e)
        {
            AttackerForm AF = new AttackerForm();
            AF.ShowDialog();
        }

        private void Walker_Click(object sender, EventArgs e)
        {
            WalkerForm WF = new WalkerForm();
            WF.ShowDialog();
        }
    }
}
