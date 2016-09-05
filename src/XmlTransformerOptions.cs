// ***********************************************************************
// Copyright(c) 2007-2016 Charlie Poole
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// ***********************************************************************

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace NUnit.Extras
{
    class XmlTransformerOptions
    {
        public XmlTransformerOptions(string[] args)
        {
            Input = new List<string>();

            var optionChars = Path.DirectorySeparatorChar == '\\'
                ? "-/"
                : "-";

            try
            {
                foreach (string arg in args)
                    if (optionChars.IndexOf(arg[0]) >= 0)
                        processOption(arg);
                    else if (IsWildCardPattern(arg))
                        processWildCardPattern(arg);
                    else
                        Input.Add(arg);

                if (Input.Count == 0)
                    Input.Add("TestResult.xml");
            }
            catch (XmlTransformerOptionsException xcp)
            {
                Console.Error.WriteLine(xcp.Message);
                Error = true;
                usage();
            }
        }

        private void processOption(string arg)
        {
            string option = arg.Substring(1);
            string[] opt = option.Split(new char[] { '=' });

            switch (opt[0])
            {
                case "xsl":
                    if (opt.Length != 2)
                    {
                        throw new XmlTransformerOptionsException("You must specify a file name for -xsl option!");
                    }
                    Transform = opt[1];
                    break;
                case "o":
                case "out":
                    if (opt.Length != 2)
                    {
                        throw new XmlTransformerOptionsException("You must specify a file name for -out option!");
                    }
                    Output = opt[1];
                    MultipleOutput = Path.GetFileNameWithoutExtension(Output) == "*";
                    string ext = Path.GetExtension(Output);
                    if (ext == ".html" || ext == ".htm")
                        Html = true;
                    break;
                case "help":
                    usage();
                    Help = true;
                    break;
                case "noheader":
                    NoHeader = true;
                    break;
                case "html":
                    Html = true;
                    break;
                case "brief":
                    Brief = true;
                    break;
                default:
                    throw new XmlTransformerOptionsException(String.Format("Invalid option: {0}", arg));
            }
        }

        private void processWildCardPattern(string arg)
        {

            string dir = Path.GetDirectoryName(arg);
            if (dir == "") dir = Environment.CurrentDirectory;
            string name = Path.GetFileName(arg);

            if (IsWildCardPattern(dir))
                processWildCardPattern(name, dir.Split(
                    new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }));
            else
                foreach (string file in Directory.GetFiles(dir, name))
                    Input.Add(file);
        }

        private void processWildCardPattern(string name, string[] parts)
        {
            int count = 0;
            string dir = "";
            foreach (string part in parts)
            {
                if (IsWildCardPattern(part))
                    break;

                if (count > 0)
                    dir += Path.DirectorySeparatorChar;
                dir += part;
                count++;
            }

            if (dir == "")
                dir = Environment.CurrentDirectory;

            if (count < parts.Length)
            {
                string pattern = parts[count];
                SearchOption searchOption = SearchOption.TopDirectoryOnly;
                if (parts[count] == "**")
                {
                    pattern = "*";
                    searchOption = SearchOption.AllDirectories;
                }

                foreach (string d in Directory.GetDirectories(dir, pattern, searchOption))
                {
                    string temp = d;
                    for (int index = count + 1; index < parts.Length; index++)
                        temp = Path.Combine(temp, parts[index]);
                    if (Directory.Exists(temp))
                        processWildCardPattern(Path.Combine(temp, name));
                }
            }
            else
                processWildCardPattern(Path.Combine(dir, name));
        }

        private bool IsWildCardPattern(string name)
        {
            return name.IndexOfAny(new char[] { '*', '?' }) >= 0;
        }

        public bool Error { get; private set; }

        public bool Help { get; private set; }

        public bool Html { get; private set; }

        public bool MultipleOutput { get; private set; }

        public bool NoHeader { get; private set; }

        public ICollection<string> Input { get; private set; }

        public string Output { get; private set; }

        public string Transform { get; private set; }

        public bool Brief { get; private set; }

        void usage()
        {
            Console.Error.WriteLine("Usage is: nunit-transform inputFile [options]");
            Console.Error.WriteLine();
            Console.Error.WriteLine("  inputFile           The absolute or relative path to an input file to be");
            Console.Error.WriteLine("                      to be processed. Multiple files may be specified and");
            Console.Error.WriteLine("                      wildcard specifications may be used in either the name");
            Console.Error.WriteLine("                      of the file or the directory path. A directory specified");
            Console.Error.WriteLine("                      as '**' represents all directories to any depth.");
            Console.Error.WriteLine();
            Console.Error.WriteLine("  -xsl=transform      Specify xsl transform to use. If not specified, an");
            Console.Error.WriteLine("                      internal transform will be used depending on the setting");
            Console.Error.WriteLine("                      of the -brief and -html options. The default transform");
            Console.Error.WriteLine("                      gives the same output as nunit-console.");
            Console.Error.WriteLine();
            Console.Error.WriteLine("  -out=outputFile     Define the output file. If missing, output is written to");
            Console.Error.WriteLine("  -o=outputFile       standard output. When multiple input files are processed,");
            Console.Error.WriteLine("                      the output basename may be specified as '*' to create");
            Console.Error.WriteLine("                      a separate output file for each input using the basename");
            Console.Error.WriteLine("                      of the corresponding input file.");
            Console.Error.WriteLine();
            Console.Error.WriteLine("                      If the extension of the output file is html or htm, the");
            Console.Error.WriteLine("                      -html option is assumed.");
            Console.Error.WriteLine();
            Console.Error.WriteLine("  -html               Indicates that html output is to be produced. The program");
            Console.Error.WriteLine("                      generates standard html headers (<html> and <body> tags)");
            Console.Error.WriteLine();
            Console.Error.WriteLine("  -noheader           Suppresses the html header and footer for each file.");
            Console.Error.WriteLine();
            Console.Error.WriteLine("  -brief              Produces summary output without listing the individual");
            Console.Error.WriteLine("                      faiing tests. Ignored if a user transform is specified.");
            Console.Error.WriteLine();
            Console.Error.WriteLine("  Note: The '/' character may be used in place of '-' on Windows systems.");
            Console.Error.WriteLine();
            Console.Error.WriteLine("Examples:");
            Console.Error.WriteLine("  nunit-summary TestResult.xml");
            Console.Error.WriteLine("  nunit-summary TestResult*.xml -brief -o=latest.txt");
            Console.Error.WriteLine("  nunit-summary TestResult*.xml -out=*.html -xsl=MySummary.xsl");
            Console.Error.WriteLine("  nunit-summary **/TestResult*.xml -out=allplatforms.txt");
        }
    }

    internal class XmlTransformerOptionsException : Exception
    {
        public XmlTransformerOptionsException(string message)
            : base(message)
        {
        }
    }
}
