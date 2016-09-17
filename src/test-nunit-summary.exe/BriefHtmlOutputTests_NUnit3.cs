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

using NUnit.Framework;

namespace NUnit.Extras.Tests
{
    public class BriefHtmlOutputTests_NUnit3 : ReportCreationTests
    {
        protected override string Input
        {
            get { return "TestInput/MockAssemblyTestResult-3.5.0.xml"; }
        }

        protected override string Output
        {
            get { return "MockAssemblyBriefSummary-3.5.0.html"; }
        }

        protected override string Options
        {
            get { return "-brief -html"; }
        }

        static string[] ExpectedText = new string[]
        {
            "OS Version:( *)Microsoft Windows NT 10.0.10586.0",
            "CLR Version:( *)4.0.30319.42000",
            "NUnit Version:( *)3.5.0.0",
            "Overall result:( *)Failed",
            "Test Count:( *)28, Passed: 15, Failed: 5, Inconclusive: 1, Skipped: 7",
            "Failed Tests:( *)Failures: 1, Errors: 1, Invalid: 3",
            "Skipped Tests:( *)Ignored: 4, Explicit: 3, Other: 0",
            "Start time:( *)2016-09-12 03:27:40Z",
            "End time:( *)2016-09-12 03:27:42Z",
            "Duration:( *)1.701 seconds"
        };

        [TestCaseSource("ExpectedText")]
        public void CheckReportContent(string text)
        {
            Assert.That(StrippedReport, Does.Match(text));
        }
    }
}
