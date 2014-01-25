using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FLVtoMP3
{
    class PoolManager
    {
        public static List<Thread> threadList = new List<Thread>();
        public static void AddThread(string URL, string FILENAME)
        {
            if (!(URL == "") && !(FILENAME == ""))
            {
                string DATA = URL + "|" + FILENAME;
                threadList.Add(new Thread(() => doSomething(DATA)));
                threadList[threadList.Count - 1].Start();
            }
        }

        public static void doSomething(string myString)
        {
            string[] arr = myString.Split('|');
            string URL, FILENAME;
            URL = arr[0];
            FILENAME = arr[1];
            
            string Directory = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            WebClient wc = new WebClient();
            string Content = wc.DownloadString(URL);

            List<string> strl = Downloader.ExtractUrls(Content);

            string fileToDownload = Downloader.GetFLV(strl);

            byte[] dat = wc.DownloadData(fileToDownload);

            string tempFile = Directory + "\\" + FILENAME + ".flv";

            FileStream flvfw = new FileStream(tempFile, FileMode.OpenOrCreate);

            flvfw.Write(dat, 0, dat.Length);
            flvfw.Flush();
            flvfw.Close();

            FileStream fs = new FileStream(tempFile, FileMode.Open);
            FileStream fw = new FileStream(Directory + "\\" + FILENAME + ".mp3", FileMode.OpenOrCreate);
            byte[] data = FLVMP3.ExtractAudio(fs);
            fw.Write(data, 0, data.Length);
            fw.Flush();
            fw.Close();
            fs.Close();

            File.Delete(tempFile);
        }
    }
}
