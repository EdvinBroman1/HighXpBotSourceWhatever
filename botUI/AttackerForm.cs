using System;
using System.Windows.Forms;

namespace botUI
{
    public partial class AttackerForm : Form
    {
        string Spell = "";
        string SpellLevel8 = "Mega Holy";
        public AttackerForm()
        {
            InitializeComponent();
            onLoad();
        }

        void onLoad()
        {
            if(PublicSettings.TargetingEnabled)
            {
                checkBox1.Checked = true;
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                timer1.Interval = int.Parse(numericUpDown1.Text);
                Spell = textBox1.Text;
                PublicSettings.TargetingEnabled = true;
                numericUpDown1.Enabled = false;
                timer1.Start();
            }
            else if(!checkBox1.Checked)
            {
                PublicSettings.TargetingEnabled = false;
                numericUpDown1.Enabled = true;
                timer1.Stop();
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Addresses.getPlayerLevel() > 8)
            {
                Addresses.SendIngameMessage(Spell);
            }
            else
            {
                Addresses.SendIngameMessage("                      ");
                Addresses.SendIngameMessage(SpellLevel8);
            }
        }
    }
}
