using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace botUI
{
    public partial class DebuggerForm : Form
    {
        Process _proc;
        
        public DebuggerForm()
        {
            InitializeComponent();
            //richTextBox1.ReadOnly = true;
            _proc = Addresses.p;
            // printDLLS();

            
        }


        void printDLLS()
        {
            foreach (ProcessModule m in _proc.Modules)
            {
                richTextBox1.Text += m.FileName + "\n";
                if (m.FileName.Contains("CGXerazx")) break;
               }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Addresses.IssueRelog();
        }
    }
}
