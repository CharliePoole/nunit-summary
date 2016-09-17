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
using NUnit.Framework;

namespace NUnit.Extras.Tests
{
    public class MultipleInputSingleOutputTest : ReportCreationTests
    {
        protected override string Input
        {
            get { return "TestInput/TestResult-*.xml"; }
        }

        protected override string Output
        {
            get { return "MultipleTestResultReport.txt"; }
        }

        protected override string Options
        {
            get { return "-brief"; }
        }

        static TestCaseData[] ExpectedText = new TestCaseData[]
        {
            new TestCaseData(0, @"C:\Program Files\NUnit-Net-2.0 2.2.10\bin\NUnitTests.nunit"),
            new TestCaseData(2, "NUnit Version 2.2.10"),
            new TestCaseData(4, "Runtime Environment -"),
            new TestCaseData(5, "OS Version: Microsoft Windows NT 5.1.2600 Service Pack 2"),
            new TestCaseData(6, "CLR Version: 2.0.50727"),
            new TestCaseData(8, "Tests run: 774, Failures: 1, Not run: 2, Time: 37.063 seconds"),

            new TestCaseData(9, @"C:\Program Files\NUnit 2.4.8\bin\NUnitTests.nunit"),
            new TestCaseData(11, "NUnit Version 2.4.8"),
            new TestCaseData(13, "Runtime Environment -"),
            new TestCaseData(14, "OS Version: Microsoft Windows NT 5.1.2600 Service Pack 2"),
            new TestCaseData(15, "CLR Version: 2.0.50727"),
            new TestCaseData(17, "Tests run: 1264, Failures: 0, Not run: 2, Time: 79.859 seconds"),

            new TestCaseData(18, @"C:\Program Files\NUnit 2.5.10\bin\net-2.0\NUnitTests.nunit"),
            new TestCaseData(20, "NUnit Version 2.5.10"),
            new TestCaseData(22, "Runtime Environment -"),
            new TestCaseData(23, "OS Version: Microsoft Windows NT 6.1.7601 Service Pack 1"),
            new TestCaseData(24, "CLR Version: 2.0.50727"),
            new TestCaseData(26, "Tests run: 3001, Errors: 0, Failures: 0, Inconclusive: 13, Time: 71.132 seconds"),
            new TestCaseData(27, "Not run: 2, Invalid: 0, Ignored: 0, Skipped: 2"),

            new TestCaseData(28, @"C:\Program Files\NUnit 2.5.2\bin\net-2.0\NUnitTests.nunit"),
            new TestCaseData(30, "NUnit Version 2.5.2"),
            new TestCaseData(32, "Runtime Environment -"),
            new TestCaseData(33, "OS Version: Microsoft Windows NT 5.1.2600 Service Pack 2"),
            new TestCaseData(34, "CLR Version: 2.0.50727"),
            new TestCaseData(36, "Tests run: 2625, Errors: 0, Failures: 0, Inconclusive: 7, Time: 181.688 seconds"),
            new TestCaseData(37, "Not run: 2, Invalid: 0, Ignored: 0, Skipped: 2"),

            new TestCaseData(38, @"C:\Program Files\NUnit 2.6\bin\NUnitTests.nunit"),
            new TestCaseData(40, "NUnit Version 2.6.0"),
            new TestCaseData(42, "Runtime Environment -"),
            new TestCaseData(43, "OS Version: Microsoft Windows NT 6.1.7601 Service Pack 1"),
            new TestCaseData(44, "CLR Version: 2.0.50727"),
            new TestCaseData(46, "Tests run: 3242, Errors: 0, Failures: 0, Inconclusive: 17, Time: 58.148 seconds"),
            new TestCaseData(47, "Not run: 8, Invalid: 0, Ignored: 0, Skipped: 8"),

            new TestCaseData(48, @"C:\Program Files\NUnit 2.6.2\bin\NUnitTests.nunit"),
            new TestCaseData(50, "NUnit Version 2.6.2"),
            new TestCaseData(52, "Runtime Environment -"),
            new TestCaseData(53, "OS Version: Microsoft Windows NT 6.1.7601 Service Pack 1"),
            new TestCaseData(54, "CLR Version: 2.0.50727"),
            new TestCaseData(56, "Tests run: 3480, Errors: 0, Failures: 0, Inconclusive: 19, Time: 78.534 seconds"),
            new TestCaseData(57, "Not run: 8, Invalid: 0, Ignored: 0, Skipped: 8"),

            new TestCaseData(58, @"D:\Dev\NUnit\nunit-2.6\bin\Debug\tests\nunit.framework.tests.dll"),
            new TestCaseData(60, "NUnit Version 2.6.4"),
            new TestCaseData(62, "Runtime Environment -"),
            new TestCaseData(63, "OS Version: Microsoft Windows NT 6.1.7601 Service Pack 1"),
            new TestCaseData(64, "CLR Version: 2.0.50727"),
            new TestCaseData(66, "Tests run: 1580, Errors: 0, Failures: 0, Inconclusive: 0, Time: 18.407 seconds"),
            new TestCaseData(67, "Not run: 0, Invalid: 0, Ignored: 0, Skipped: 0"),
        };

        [TestCaseSource("ExpectedText")]
        public void CheckReportContent(int lineno, string text)
        {
            //int count = 0;
            //int index = 0;
            //for (;;)
            //{
            //    index = Report.IndexOf(text, index);
            //    if (index < 0) break;

            //    count++;

            //    index += text.Length;
            //    if (index >= Report.Length) break;
            //}

            //Assert.That(count, Is.EqualTo(expectedCount), "Error finding text \"" + text + "\"");
            Assert.That(ReportLines[lineno], Contains.Substring(text));
        }
    }
}
