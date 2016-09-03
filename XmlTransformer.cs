// *****************************************************
// Copyright 2007-2016, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

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
                            transform = "HtmlSummary.xslt";
                        else
                            transform = "BriefSummary.xslt";
                    else
                        if (options.Html)
                            transform = "HtmlTransform.xslt";
                        else
                            transform = "DefaultTransform.xslt";

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