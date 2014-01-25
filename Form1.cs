using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FLVtoMP3
{
    public partial class Form1 : Form
    {
        public static Thread UpdateThreadCount;
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            UpdateThreadCount = new Thread(delegate()
            {
                while (true)
                { 
                    activeThreads.Text = Convert.ToString(PoolManager.Threads);
                }
            });
            UpdateThreadCount.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains("http://"))
            {
                PoolManager.AddThread(textBox1.Text, textBox2.Text);
            }
            else
            {
                MessageBox.Show("Your link did not have the correct format, please use this format for all links:\n\nhttp://www.youtube.com/ABCDEFGHIJ\n\nIf you entered the link correctly please contact support. Note that everything is crucial from http to the slashes and dots.");
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "[Enter Song Name]")
            {
                textBox2.Clear();
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "[Enter YouTube URL]")
            {
                textBox1.Clear();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateThreadCount.Abort();
            foreach (Thread th in PoolManager.threadList)
            {
                if (th.IsAlive)
                {
                    MessageBox.Show("You have at least one download pending, program will exit upon completition.");
                    th.Join();
                }
            }
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            MessageBox.Show("This software was written for educational purposes by XtrmJosh.\n\nFor more information about me and my projects, visit my website at www.joshgriffith.co.uk.");
        }

        private void selectOutputDirectory_ButtonClick(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            fbd.Description = "Select a folder to save your audio tracks to.";
            DialogResult dr = fbd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK || dr == System.Windows.Forms.DialogResult.Yes)
            {
                PoolManager.Directory = fbd.SelectedPath;
            }
            
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }
    }
}
