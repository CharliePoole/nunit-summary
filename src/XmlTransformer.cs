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
using System.Collections;
using System.Resources;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;

namespace NUnit.Extras
{
	class XmlTransformer
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
            Console.Error.WriteLine("nunit-summary version 0.4 - Copyright (c) 2007-2016, Charlie Poole");
            Console.Error.WriteLine();

            XmlTransformerOptions options = new XmlTransformerOptions(args);
            if (options.Error || options.Help) return;

            TextWriter output = Console.Out;

            if (!options.MultipleOutput && options.Output != null)
            {
                output = new StreamWriter(options.Output);
                if (options.Html && !options.NoHeader)
                    WriteHtmlHeader(output);
            }

			try
			{
                XslCompiledTransform xform = new XslCompiledTransform();

                if (options.Transform != null)
                    xform.Load(options.Transform);
                else
                {
                    string transform;

                    if (options.Brief)
                        if (options.Html)
                            transform = "HtmlSummary-v2.xslt";
                        else
                            transform = "BriefSummary-v2.xslt";
                    else
                        if (options.Html)
                            transform = "HtmlTransform-v2.xslt";
                        else
                            transform = "DefaultTransform-v2.xslt";

                    Assembly assembly = Assembly.GetExecutingAssembly();
                    Stream stream = assembly.GetManifestResourceStream("NUnit.Extras.Transforms." + transform);
                    if (stream == null)
                        throw new Exception("Transform not found: " + transform);

                    xform.Load(new XmlTextReader(stream));
                }

                int fileCount = 0;
                foreach (string inputFile in options.Input)
                {
                    fileCount++;

                    string outputFile = null;
                    if (options.MultipleOutput)
                    {
                        outputFile = options.Output.Replace("*", Path.GetFileNameWithoutExtension(inputFile));
                        output = new StreamWriter(outputFile);

                        if (options.Html && !options.NoHeader)
                            WriteHtmlHeader(output);
                    }

                    xform.Transform(inputFile, null, output);

                    if (options.MultipleOutput)
                    {
                        Console.Error.WriteLine("Output saved as {0}", outputFile);
                        output.Close();
                    }
                    else if (options.Html && !options.NoHeader)
                        WriteHtmlTrailer(output);
                }

                if (!options.MultipleOutput)
                {
                    if ( options.Html && !options.NoHeader )
                        WriteHtmlTrailer(output);

                    if (options.Output != null)
                    {
                        Console.Error.WriteLine("Output saved as {0}", Path.GetFullPath( options.Output ) );
                        output.Close();
                    }
                }
            }
			catch( Exception ex )
			{
				Console.Error.WriteLine( "Error: {0}", ex.Message );
			}
		}

        private static void WriteHtmlHeader(TextWriter output)
        {
            output.WriteLine("<html>");
            output.WriteLine("<body>");
        }

        private static void WriteHtmlTrailer(TextWriter output)
        {
            output.WriteLine("</body>");
            output.WriteLine("</html>");
        }
    }
}