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
                    if (!string.IsNullOrEmpty(_options.Output))
                    {
                        var dir = Path.GetDirectoryName(_options.Output);
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                    }

                    if (_options.MultipleOutput)
                        TransformToMultipleOutputFiles();
                    else
                        TransformToSingleOutputFile();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: {0}", ex.Message);
                throw;
            }
        }

        private XslCompiledTransform _userTransform;
        public XslCompiledTransform UserTransform
        {
            get
            {
                if (_userTransform == null && _options.Transform != null)
                {
                    _userTransform = new XslCompiledTransform();
                    _userTransform.Load(_options.Transform);
                }

                return _userTransform;
            }
        }

        private XslCompiledTransform _internalV2Transform;
        public XslCompiledTransform InternalV2Transform
        {
            get
            {
                if (_internalV2Transform == null)
                {
                    string transform = _options.Brief
                        ? _options.Html
                            ? "HtmlSummary-v2.xslt"
                            : "BriefSummary-v2.xslt"
                        : _options.Html
                            ? "HtmlTransform-v2.xslt"
                            : "DefaultTransform-v2.xslt";

                    _internalV2Transform = LoadInternalTransform(transform);
                }

                return _internalV2Transform;
            }
        }

        private XslCompiledTransform _internalV3Transform;
        public XslCompiledTransform InternalV3Transform
        {
            get
            {
                if (_internalV3Transform == null)
                {
                    string transform = _options.Brief
                        ? _options.Html
                            ? "HtmlSummary-v3.xslt"
                            : "BriefSummary-v3.xslt"
                        : _options.Html
                            ? "HtmlTransform-v3.xslt"
                            : "DefaultTransform-v3.xslt";

                    _internalV3Transform = LoadInternalTransform(transform);
                }

                return _internalV3Transform;
            }
        }

        private XslCompiledTransform LoadInternalTransform(string transform)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("NUnit.Extras.Transforms." + transform);

            if (stream == null)
                throw new Exception("Transform not found: " + transform);

            var xform = new XslCompiledTransform();
            xform.Load(new XmlTextReader(stream));

            return xform;
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

                TransformResult(inputFile, output);

                if (_options.Html && !_options.NoHeader)
                    WriteHtmlTrailer(output);

                Console.Error.WriteLine("Output saved as {0}", outputFile);
                output.Close();
            }
        }

        private void TransformToSingleOutputFile()
        {
            TextWriter output = Console.Out;

            try
            {
                if (_options.Output != null)
                {
                    output = new StreamWriter(_options.Output);
                    if (_options.Html && !_options.NoHeader)
                        WriteHtmlHeader(output);
                }

                foreach (string inputFile in _options.Input)
                {
                    TransformResult(inputFile, output);
                }

                if (_options.Html && !_options.NoHeader)
                    WriteHtmlTrailer(output);

                if (_options.Output != null)
                    Console.Error.WriteLine("Output saved as {0}", Path.GetFullPath(_options.Output));
            }
            finally
            {
                if (_options.Output != null)
                    output.Close();
            }
        }

        private void TransformResult(string inputFile, TextWriter output)
        {
            if (_options.Transform != null) // User-supplied transform
                UserTransform.Transform(inputFile, XmlWriter.Create(output));
            else // Select internal transform
            {
                var doc = new XmlDocument();
                doc.Load(inputFile);
                if (IsV2Result(doc))
                    InternalV2Transform.Transform(doc, null, output);
                else
                    InternalV3Transform.Transform(doc, null, output);
            }
        }

        private static bool IsV2Result(XmlDocument doc)
        {
            return doc.DocumentElement.Name == "test-results";
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