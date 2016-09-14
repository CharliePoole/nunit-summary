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
using System.Text.RegularExpressions;
using System.Xml;
using NUnit.Framework;

namespace NUnit.Extras.Tests
{
    public class BriefHtmlOutputTests_NUnit2 : ReportCreationTests
    {
        protected override string Input
        {
            get { return "MockAssemblyTestResult-2.6.4.xml"; }
        }

        protected override string Output
        {
            get { return "MockAssemblyBriefSummary-2.6.4.html"; }
        }

        protected override string Options
        {
            get { return "-brief -html"; }
        }

        static TestCaseData[] ExpectedText = new TestCaseData[]
        {
            new TestCaseData(@"C:\Program Files\NUnit 2.6.4\bin\tests\mock-assembly.dll"),
            new TestCaseData("NUnit Version: 2.6.4.14350"),
            new TestCaseData("Date: 2016-09-10"),
            new TestCaseData("Time: 11:13:55"),
            new TestCaseData("Runtime Environment -"),
            new TestCaseData("OS Version: Microsoft Windows NT 6.2.9200.0"),
            new TestCaseData("CLR Version: " + Environment.Version),
            new TestCaseData("Tests run: 21, Errors: 1, Failures: 1, Inconclusive: 1, Time: 0.088 seconds"),
            new TestCaseData("Not run: 7, Invalid: 3, Ignored: 4, Skipped: 0")
        };

        [TestCaseSource("ExpectedText")]
        public void CheckReportContent(string text)
        {
            Assert.That(StrippedReport, Contains.Substring(text));
        }
    }
}
