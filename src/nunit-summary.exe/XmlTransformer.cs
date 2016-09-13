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
	public class XmlTransformer
	{
        private XmlTransformerOptions _options;
        private XslCompiledTransform _xform;

        public void Execute(string[] args)
        {
            try
            {
                _options = new XmlTransformerOptions(args);

                if (_options.Error || _options.Help)
                {
                    int errs = 0;

                    foreach (var msg in _options.Errors)
                    {
                        Console.Error.WriteLine(msg);
                        errs++;
                    }

                    if (errs > 0)
                        Console.Error.WriteLine();

                    _options.ShowHelp();
                }
                else
                {
                    var dir = Path.GetDirectoryName(_options.Output);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    if (_options.MultipleOutput)
                        TransformToMultipleOutputFiles();
                    else
                        TransformToSingleOutputFile();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: {0}", ex.Message);
            }
        }

        public XslCompiledTransform XForm
        {
            get
            {
                if (_xform == null)
                {
                    _xform = new XslCompiledTransform();

                    if (_options.Transform != null)
                        _xform.Load(_options.Transform);
                    else
                    {
                        string transform;

                        if (_options.Brief)
                            if (_options.Html)
                                transform = "HtmlSummary-v2.xslt";
                            else
                                transform = "BriefSummary-v2.xslt";
                        else
                            if (_options.Html)
                            transform = "HtmlTransform-v2.xslt";
                        else
                            transform = "DefaultTransform-v2.xslt";

                        Assembly assembly = Assembly.GetExecutingAssembly();
                        Stream stream = assembly.GetManifestResourceStream("NUnit.Extras.Transforms." + transform);
                        if (stream == null)
                            throw new Exception("Transform not found: " + transform);

                        _xform.Load(new XmlTextReader(stream));
                    }
                }

                return _xform;
            }
        }

        private void TransformToMultipleOutputFiles()
        {
            TextWriter output = Console.Out;

            foreach (string inputFile in _options.Input)
            {
                string outputFile = _options.Output.Replace("*", Path.GetFileNameWithoutExtension(inputFile));
                output = new StreamWriter(outputFile);

                if (_options.Html && !_options.NoHeader)
                    WriteHtmlHeader(output);

                XForm.Transform(inputFile, null, output);

                if (_options.Html && !_options.NoHeader)
                    WriteHtmlTrailer(output);

                Console.Error.WriteLine("Output saved as {0}", outputFile);
                output.Close();
            }
        }

        private void TransformToSingleOutputFile()
        {
            TextWriter output = Console.Out;

            if (_options.Output != null)
            {
                output = new StreamWriter(_options.Output);
                if (_options.Html && !_options.NoHeader)
                    WriteHtmlHeader(output);
            }

            foreach (string inputFile in _options.Input)
            {
                XForm.Transform(inputFile, null, output);
            }

            if (_options.Html && !_options.NoHeader)
                WriteHtmlTrailer(output);

            if (_options.Output != null)
            {
                Console.Error.WriteLine("Output saved as {0}", Path.GetFullPath(_options.Output));
                output.Close();
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