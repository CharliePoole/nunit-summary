// ***********************************************************************
// Copyright(c) 2016 Charlie Poole
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
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace NUnit.Extras.Tests
{
    public abstract class ReportCreationTests
    {
        protected const string INPUT_DIR = "../../input";
        protected const string REPORT_DIR = "../../reports";

        protected abstract string Input { get; }
        protected abstract string Output { get; }
        protected abstract string Options { get; }

        private string _output;

        [OneTimeSetUp]
        public void CreateOutputReport()
        {
            var transformer = new XmlTransformer();

            var input = Input;
            if (!Path.IsPathRooted(input))
                input = Path.Combine(INPUT_DIR, input);

            _output = Output;
            if (!Path.IsPathRooted(_output))
                _output = Path.Combine(REPORT_DIR, _output);

            string[] options = !string.IsNullOrEmpty(Options)
                ? Options.Split(new char[] { ' ' })
                : new string[0];
            string[] args = new string[options.Length + 2];

            int index = 0;
            args[index++] = input;
            args[index++] = "-o=" + _output;
            foreach (string opt in options)
                args[index++] = opt;

            transformer.Execute(args);

            FileAssert.Exists(_output, "No output created - no tests can be run");
        }

        private string _report;
        /// <summary>
        /// Returns the full text of the created report as a string,
        /// For use in tests that examine the full report as a string.
        /// </summary>
        protected string Report
        {
            get
            {
                if (_report == null)
                    _report = File.ReadAllText(_output);

                return _report;
            }
        }

        private string[] _reportLines;
        /// <summary>
        /// Returns the full text of the created report as an array of lines.
        /// For use in tests that examine the full report, line by line.
        /// </summary>
        protected string[] ReportLines
        {
            get
            {
                if (_reportLines == null)
                    _reportLines = File.ReadAllLines(_output);

                return _reportLines;
            }
        }

        private static Regex _htmlStripper = new Regex("<.*?>", RegexOptions.Compiled);

        private string _strippedReport;
        /// <summary>
        /// Returns the full text of the created report stripped of all HTML.
        /// For use in tests that examine the full report, ignoring HTML.
        /// </summary>
        protected string StrippedReport
        {
            get
            {
                if (_strippedReport == null)
                    _strippedReport = _htmlStripper.Replace(Report, "");

                return _strippedReport;
            }
        }
    }
}
