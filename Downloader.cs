using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FLVtoMP3
{
    class Downloader
    {
        public static List<string> ExtractUrls(string html)
        {
            string title = GetTitle(html);

            List<string> urls = new List<string>();
            string DataBlockStart = "\"url_encoded_fmt_stream_map\":\\s+\"(.+?)&";  // Marks start of Javascript Data Block

            html = Uri.UnescapeDataString(Regex.Match(html, DataBlockStart, RegexOptions.Singleline).Groups[1].ToString());

            string firstPatren = html.Substring(0, html.IndexOf('=') + 1);
            var matchs = Regex.Split(html, firstPatren);
            for (int i = 0; i < matchs.Length; i++)
                matchs[i] = firstPatren + matchs[i];
            foreach (var match in matchs)
            {
                if (!match.Contains("url=")) continue;

                string url = GetTxtBtwn(match, "url=", "\\u0026", 0, false);
                if (url == "") url = GetTxtBtwn(match, "url=", ",url", 0, false);
                if (url == "") url = GetTxtBtwn(match, "url=", "\",", 0, false);

                string sig = GetTxtBtwn(match, "sig=", "\\u0026", 0, false);
                if (sig == "") sig = GetTxtBtwn(match, "sig=", ",sig", 0, false);
                if (sig == "") sig = GetTxtBtwn(match, "sig=", "\",", 0, false);

                while ((url.EndsWith(",")) || (url.EndsWith(".")) || (url.EndsWith("\"")))
                    url = url.Remove(url.Length - 1, 1);

                while ((sig.EndsWith(",")) || (sig.EndsWith(".")) || (sig.EndsWith("\"")))
                    sig = sig.Remove(sig.Length - 1, 1);

                if (string.IsNullOrEmpty(url)) continue;
                if (!string.IsNullOrEmpty(sig))
                    url += "&signature=" + sig;
                urls.Add(url);
            }

            for (int i = 0; i < urls.Count; i++)
            {
                urls[i] += "&title=";
                urls[i] += title;
            }
            
            return urls;
        }

        public static string GetFLV(List<string> urls)
        {
            // Acquire a list of links which match the criteria for being FLV files
            List<string> flvurls = new List<string>();
            foreach (string url in urls)
            {
                string itag = Regex.Match(url, @"itag=([1-9]?[0-9]?[0-9])", RegexOptions.Singleline).Groups[1].ToString();
                int itagint;
                int.TryParse(itag, out itagint);
                
                if (itagint == 5 || itagint == 6 || itagint == 34 || itagint == 35)
                {
                    flvurls.Add(url);
                }
            }

            // If we didn't find any FLVs, we return a fatal error and cause a bug later on
            if (flvurls.Count == 0)
            {
                MessageBox.Show("Fatal error | iTag could not be found for FLV filetype. Please contact software vendor for assistance.");
                return "";
            }
            // If we did find some FLVs, we need to find the highest quality FLV
            else
            {
                #region findBestFLV
                foreach (string url in flvurls)
                {
                    string itag = Regex.Match(url, @"itag=([1-9]?[0-9]?[0-9])", RegexOptions.Singleline).Groups[1].ToString();
                    int itagint;
                    int.TryParse(itag, out itagint);
                    if (itagint == 35)
                    {
                        return url;
                    }
                }
                foreach (string url in flvurls)
                {
                    string itag = Regex.Match(url, @"itag=([1-9]?[0-9]?[0-9])", RegexOptions.Singleline).Groups[1].ToString();
                    int itagint;
                    int.TryParse(itag, out itagint);
                    if (itagint == 34)
                    {
                        return url;
                    }
                }
                foreach (string url in flvurls)
                {
                    string itag = Regex.Match(url, @"itag=([1-9]?[0-9]?[0-9])", RegexOptions.Singleline).Groups[1].ToString();
                    int itagint;
                    int.TryParse(itag, out itagint);
                    if (itagint == 6)
                    {
                        return url;
                    }
                }
                foreach (string url in flvurls)
                {
                    string itag = Regex.Match(url, @"itag=([1-9]?[0-9]?[0-9])", RegexOptions.Singleline).Groups[1].ToString();
                    int itagint;
                    int.TryParse(itag, out itagint);
                    if (itagint == 5)
                    {
                        return url;
                    }
                }
                #endregion
            }
            MessageBox.Show("Fatal error | Something has gone horrible wrong whilst finding the best FLV to use. Run, brave warrior, for the end is near.");
            return "";
        }

        public static string GetTitle(string RssDoc)
        {
            string str14 = GetTxtBtwn(RssDoc, "'VIDEO_TITLE': '", "'", 0, false);
            if (str14 == "") str14 = GetTxtBtwn(RssDoc, "\"title\" content=\"", "\"", 0, false);
            if (str14 == "") str14 = GetTxtBtwn(RssDoc, "&title=", "&", 0, false);
            str14 = str14.Replace(@"\", "").Replace("'", "&#39;").Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("+", " ");
            return str14;
        }

        public static string GetTxtBtwn(string input, string start, string end, int startIndex, bool UseLastIndexOf)
        {
            int index1 = UseLastIndexOf ? input.LastIndexOf(start, startIndex) :
                                          input.IndexOf(start, startIndex);
            if (index1 == -1) return "";
            index1 += start.Length;
            int index2 = input.IndexOf(end, index1);
            if (index2 == -1) return input.Substring(index1);
            return input.Substring(index1, index2 - index1);
        }
    }
}
