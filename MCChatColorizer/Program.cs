using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
                output.WriteLine("<style>");
                output.WriteLine("body {\nfont-family: \"Comic Sans MS\", Arial, sans-serif;\n}");
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
                output.WriteLine("</head>");
                output.WriteLine("<body>");
                

                string line;
                string outLine;
                while ((line = sr.ReadLine()) != null)
                {
                    outLine = "";
                    foreach (string word in line.Split(' ')) {
                        bool magic = false;
                        if (word.StartsWith("&") || word.StartsWith("§"))
                        {
                            switch (word.ToLower().ToCharArray()[1])
                            {
                                case '0':
                                    openTags.Add("black");
                                    outLine += "<black>";
                                    break;
                                case '1':
                                    openTags.Add("darkblue");
                                    outLine += "<darkblue>";
                                    break;
                                case '2':
                                    openTags.Add("darkgreen");
                                    outLine += "<darkgreen>";
                                    break;
                                case '3':
                                    openTags.Add("darkaqua");
                                    outLine += "<darkaqua>";
                                    break;
                                case '4':
                                    openTags.Add("darkred");
                                    outLine += "<darkred>";
                                    break;
                                case '5':
                                    openTags.Add("darkpurple");
                                    outLine += "<darkpurple>";
                                    break;
                                case '6':
                                    openTags.Add("gold");
                                    outLine += "<gold>";
                                    break;
                                case '7':
                                    openTags.Add("gray");
                                    outLine += "<gray>";
                                    break;
                                case '8':
                                    openTags.Add("darkgray");
                                    outLine += "<darkgray>";
                                    break;
                                case '9':
                                    openTags.Add("blue");
                                    outLine += "<blue>";
                                    break;
                                case 'a':
                                    openTags.Add("green");
                                    outLine += "<green>";
                                    break;
                                case 'b':
                                    openTags.Add("aqua");
                                    outLine += "<aqua>";
                                    break;
                                case 'c':
                                    openTags.Add("red");
                                    outLine += "<red>";
                                    break;
                                case 'd':
                                    openTags.Add("lightpurple");
                                    outLine += "<lightpurple>";
                                    break;
                                case 'e':
                                    openTags.Add("yellow");
                                    outLine += "<yellow>";
                                    break;
                                case 'f':
                                    openTags.Add("white");
                                    outLine += "<white>";
                                    break;
                                case 'k':
                                    magic = true;
                                    break;
                                case 'l':
                                    openTags.Add("b");
                                    outLine += "<b>";
                                    break;
                                case 'm':
                                    openTags.Add("del");
                                    outLine += "<del>";
                                    break;
                                case 'n':
                                    openTags.Add("u");
                                    outLine += "<u>";
                                    break;
                                case 'o':
                                    openTags.Add("i");
                                    outLine += "<i>";
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
                            if (magic)
                                outLine += word.Substring(3);
                            else
                                outLine += word.Substring(2);
                        }
                        else
                            outLine += word;

                        outLine += " ";
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
