using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCChatColorizer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static void ProcessLog(string FileName, string OutFolder)
        {
            if (FileName.EndsWith(".gz"))
            {
                using (FileStream originalFileStream = new FileInfo(FileName).OpenRead())
                {
                    string newFileName = FileName.Remove(FileName.Length - ".gz".Length);
                    using (FileStream decompressedFileStream = File.Create(newFileName))
                    {
                        using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                        {
                            decompressionStream.CopyTo(decompressedFileStream);
                            FileName = newFileName;
                        }
                    }
                }
            }


            List<string> openTags = new List<string>();
            try
            {
                var sr = new StreamReader(FileName);

                var output = new StreamWriter($"{OutFolder}\\colorized-chat.html");

                
                output.WriteLine("<!DOCTYPE html>");
                output.WriteLine("<html>");
                output.WriteLine("<head>");
                output.WriteLine("<title>Colorized ChatLog</title>");
                output.WriteLine("<link href=\"https://fonts.googleapis.com/css2?family=VT323&display=swap\" rel=\"stylesheet\">");
                output.WriteLine("<style>");
                output.WriteLine("body {\nfont-family: 'VT323', monospace;\n}");
                output.WriteLine("black {\ncolor: #000;\n}");
                output.WriteLine("white {\ncolor: #fff;\n}");
                output.WriteLine("darkred {\ncolor: #700;\n}");
                output.WriteLine("red {\ncolor: #d00;\n}");
                output.WriteLine("green {\ncolor: #0d0;\n}");
                output.WriteLine("darkgreen {\ncolor: #070;\n}");
                output.WriteLine("yellow {\ncolor: #ed0;\n}");
                output.WriteLine("gold {\ncolor: #a90;\n}");
                output.WriteLine("aqua {\ncolor: #0dd;\n}");
                output.WriteLine("darkaqua {\ncolor: #088;\n}");
                output.WriteLine("lightpurple {\ncolor: #e0e;\n}");
                output.WriteLine("darkpurple {\ncolor: #909;\n}");
                output.WriteLine("darkblue {\ncolor: #009;\n}");
                output.WriteLine("blueblue {\ncolor: #00d;\n}");
                output.WriteLine("gray {\ncolor: #aaa;\n}");
                output.WriteLine("darkgray {\ncolor: #555;\n}");
                output.WriteLine("</style>");
                output.WriteLine("<script> setInterval(function() { var elems = document.getElementsByClassName('magic'); for (var i = 0; i < elems.length; i++) { elems[i].innerHTML = String.fromCharCode(97+Math.floor(Math.random() * 26)) }}, 50); </script>");
                output.WriteLine("</head>");
                output.WriteLine("<body>");
                

                string line;
                string outLine;
                while ((line = sr.ReadLine()) != null)
                {
                    outLine = "";
                    foreach (string word in line.Split('§'))
                    {
                        Boolean magic = false;
                        String tag = null;
                        switch (word.ToCharArray()[0])
                        {
                            case '0':
                                tag = "black";
                                break;
                            case '1':
                                tag = "darkblue";
                                break;
                            case '2':
                                tag = "darkgreen";
                                break;
                            case '3':
                                tag = "darkaqua";
                                break;
                            case '4':
                                tag = "darkred";
                                break;
                            case '5':
                                tag = "darkpurple";
                                break;
                            case '6':
                                tag = "gold";
                                break;
                            case '7':
                                tag = "gray";
                                break;
                            case '8':
                                tag = "darkgray";
                                break;
                            case '9':
                                tag = "blue";
                                break;
                            case 'a':
                                tag = "green";
                                break;
                            case 'b':
                                tag = "aqua";
                                break;
                            case 'c':
                                tag = "red";
                                break;
                            case 'd':
                                tag = "lightpurple";
                                break;
                            case 'e':
                                tag = "yellow";
                                break;
                            case 'f':
                                tag = "white";
                                break;
                            case 'k':
                                magic = true; //TODO add span with class "magic" and use js to handle it
                                break;
                            case 'l':
                                tag = "b";
                                break;
                            case 'm':
                                tag = "del";
                                break;
                            case 'n':
                                tag = "u";
                                break;
                            case 'o':
                                tag = "i";
                                break;
                            case 'r':
                                for (int index = openTags.Count() - 1; index >= 0; index--)
                                {
                                    outLine += "</" + openTags.ElementAt(index) + ">";
                                }
                                openTags.Clear();
                                break;
                            default:
                                break;
                        }

                        if (tag == null)
                        {
                            if (!magic)
                            {
                                if (word.StartsWith("r"))
                                    outLine += word.Substring(1);
                                else
                                    outLine += word;
                            }
                            else
                            {
                                outLine += "<span class=\"magic\"></span>";
                            }
                        } 
                        else
                        {
                            openTags.Add(tag);
                            outLine += $"<{tag}>{word.Substring(1)}";
                        }
                    }
                    for (int index = openTags.Count() - 1; index >= 0; index--)
                    {
                        outLine += "</" + openTags.ElementAt(index) + ">";
                    }
                    openTags.Clear();
                    output.WriteLine(outLine);
                    output.WriteLine("<br />");
                }

                output.WriteLine("</body>");
                output.WriteLine("</html>");

                // Lastly, close up
                output.Flush();
                output.Close();

                sr.Close();

                System.Diagnostics.Process.Start($"{OutFolder}\\colorized-chat.html");

            } 
            catch (SecurityException ex)
            {
                MessageBox.Show($"An error occurred while processing your log file.\n\n" +
                    $"Error message: {ex.Message}\n\n " +
                    $"Details:\n\n{ex.StackTrace}");
            }
        }
    }
}
