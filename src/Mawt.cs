using Mono.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MakeAvsWithTemplate
{
    class Mawt
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            bool showHelp = false;

            string input = null;
            string output = null;
            string video = null;
            string audio = null;
            
            OptionSet os = new OptionSet()
                .Add("i|input=", "input avs", v => input = v)
                .Add("o|output=", "output avs", v => output = v)
                .Add("v|video=", "video file", v => video = v)
                .Add("a|audio=", "audio file", v => audio = v)
                .Add("h|help", "show this message and exit", h => showHelp = h != null);

            List<string> extra;
            try
            {
                extra = os.Parse(args);
                extra.ForEach(t => Debug.WriteLine(t));
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `CommandLineOption --help' for more information.");
                return;
            }

            if (
                showHelp
                || video == null
                || (video == null && audio == null)
                )
            {
                Console.Error.WriteLine("Usage: MakeAvsWithTemplate.exe [option]");
                Console.Error.WriteLine();
                os.WriteOptionDescriptions(Console.Error);
                return;
            }
            
            if (input == null)
                input = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\template.avs";

            if (output == null)
                output = Environment.CurrentDirectory + "\\output.avs";

            File.Copy(input, output, true);

            StreamReader sr = new StreamReader(output, Encoding.GetEncoding("Shift_JIS"));
            string stream = sr.ReadToEnd();
            sr.Close();

            stream = stream.Replace("___video___", video);
            if (audio != null)
                stream = stream.Replace("___audio___", audio);

            StreamWriter sw = new StreamWriter(output, false, Encoding.GetEncoding("Shift_JIS"));
            sw.Write(stream);
            sw.Close();

            Console.WriteLine("Created a avs file.");
            Console.WriteLine(output);
        }
    }
}
