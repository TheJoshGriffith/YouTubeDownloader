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
            UpdateThreadCount = new Thread(delegate()
            {
                while (true)
                { 
                    int count = 0;
                    foreach (Thread th in PoolManager.threadList)
                    {
                        if (th.IsAlive)
                        {
                            count++;
                        }
                    }
                    activeThreads.Text = Convert.ToString(count);
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
            
            /* ORIGINAL CODE
             * 
             * 
            string Directory = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            WebClient wc = new WebClient();
            string Content = wc.DownloadString(textBox1.Text);

            List<string> strl = Downloader.ExtractUrls(Content);

            string fileToDownload = Downloader.GetFLV(strl);

            byte[] dat = wc.DownloadData(fileToDownload);

            string tempFile = Directory + "\\" + textBox2.Text + ".flv";

            FileStream flvfw = new FileStream(tempFile, FileMode.OpenOrCreate);

            flvfw.Write(dat, 0, dat.Length);
            flvfw.Flush();
            flvfw.Close();

            FileStream fs = new FileStream(tempFile, FileMode.Open);
            FileStream fw = new FileStream(Directory + "\\" + textBox2.Text + ".wav", FileMode.OpenOrCreate);
            byte[] data = FLVMP3.ExtractAudio(fs);
            fw.Write(data, 0, data.Length);
            fw.Flush();
            fw.Close();
             * */
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
    }
}
