using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Tim_s_IRC_bot
{
    public partial class BotControlPanel : Form
    {
        public BotControlPanel()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText("/TimsBot/data/access/commands/users/" + textBox1.Text, textBox1.Text);
            MessageBox.Show("Added user for full bot access");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        
            textBox2.Text = string.Join(Environment.NewLine, System.IO.Directory.GetFiles("/TimsBot/data/access/commands/users/")); 
            textBox2.Text = textBox2.Text.Replace("/TimsBot/data/access/commands/users/", "");
            textBox4.Text = string.Join(Environment.NewLine, System.IO.Directory.GetDirectories("/TimsBot/data/systems/warnsystem/warning/"));
            textBox4.Text = textBox4.Text.Replace("/TimsBot/data/systems/warnsystem/warning/", "");
            textBox10.Text = string.Join(Environment.NewLine, System.IO.Directory.GetDirectories("/TimsBot/data/systems/warnsystem/offtopic/"));
            textBox10.Text = textBox10.Text.Replace("/TimsBot/data/systems/warnsystem/offtopic/", "");

        }

        private void BotControlPanel_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText(System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemDrive"), "/TimsBot/settings.ini"), "Server=" + textBox5.Text + Environment.NewLine + "Channel=" + textBox6.Text + Environment.NewLine + "Botname=" + textBox7.Text + Environment.NewLine + "username=" + textBox8.Text + Environment.NewLine + "password=" + textBox9.Text);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/tim241/TimsIRCBot");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("/TimsBot/data/access/commands/users/" + textBox1.Text))
            {
                System.IO.File.Delete("/TimsBot/data/access/commands/users/" + textBox1.Text);
                MessageBox.Show("Removed user from full bot access");
            }
            else
            {
                MessageBox.Show("Username not found.");
            }
        }
    }
}
