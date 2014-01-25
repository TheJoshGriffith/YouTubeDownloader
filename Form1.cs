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
        public Form1()
        {
            InitializeComponent();
            Thread UpdateThreadCount = new Thread(delegate()
            {
                while (true)
                { 
                    activeThreads.Text = Convert.ToString(PoolManager.threadList.Count());
                }
            });
            UpdateThreadCount.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PoolManager.AddThread(textBox1.Text, textBox2.Text);
            
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
    }
}
