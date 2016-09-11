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
    public class DefaultHtmlOutputTests : XmlTransformerTests
    {
        protected override string Input
        {
            get { return "MockAssemblyTestResult-2.6.4.xml"; }
        }

        protected override string Output
        {
            get { return "MockAssemblyDefaultOutput-2.6.4.html"; }
        }

        protected override string Options
        {
            get { return "-html"; }
        }

        static TestCaseData[] ExpectedLines = new TestCaseData[]
        {
            new TestCaseData(0, "<html>"),
            new TestCaseData(1, "<body>"),
            new TestCaseData(2, @"<b>C:\Program Files\NUnit 2.6.4\bin\tests\mock-assembly.dll</b><br><br>"),
            new TestCaseData(3, ""),
            new TestCaseData(4, "<b>NUnit Version:</b> 2.6.4.14350&nbsp;&nbsp;&nbsp;<b>Date:</b> 2016-09-10&nbsp;&nbsp;&nbsp;<b>Time:</b> 11:13:55<br><br>"),
            new TestCaseData(5, ""),
            new TestCaseData(6, "<b>Runtime Environment -</b><br>"),
            new TestCaseData(7, "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>OS Version:</b> Microsoft Windows NT 6.2.9200.0<br>"),
            new TestCaseData(8, "&nbsp;&nbsp;&nbsp;<b>CLR Version:</b> " + Environment.Version + "<br><br>"),
            new TestCaseData(9, ""),
            new TestCaseData(10, "<b>Tests run: 21, Errors: 1, Failures: 1, Inconclusive: 1, Time: 0.088 seconds<br>&nbsp;&nbsp;&nbsp;Not run: 7, Invalid: 3, Ignored: 4, Skipped: 0</b><br><br>"),
            new TestCaseData(11, ""),
            new TestCaseData(12, "<h4>Failures:</h4>"),
            new TestCaseData(13, "<ol>"),
            new TestCaseData(14, "<pre>"),
            new TestCaseData(15, "<li>NUnit.Tests.Assemblies.MockTestFixture.FailingTest : Intentional failure"),
            new TestCaseData(16, "at NUnit.Tests.Assemblies.MockTestFixture.FailingTest()"),
            new TestCaseData(17, "</pre>"),
            new TestCaseData(18, "<pre>"),
            new TestCaseData(19, "<li>NUnit.Tests.Assemblies.MockTestFixture.TestWithException : System.ApplicationException : Intentional Exception"),
            new TestCaseData(20, "at NUnit.Tests.Assemblies.MockTestFixture.MethodThrowsException()"),
            new TestCaseData(21, "at NUnit.Tests.Assemblies.MockTestFixture.TestWithException()"),
            new TestCaseData(22, "</pre>"),
            new TestCaseData(23, "</ol>"),
            new TestCaseData(24, "<h4>Tests not run:</h4>"),
            new TestCaseData(25, "<ol>"),
            new TestCaseData(26, "<pre>"),
            new TestCaseData(27, "<li>NUnit.Tests.Assemblies.MockTestFixture.MockTest4 : ignoring this test method for now"),
            new TestCaseData(28, "</pre>"),
            new TestCaseData(29, "<pre>"),
            new TestCaseData(30, "<li>NUnit.Tests.Assemblies.MockTestFixture.MockTest5 : Method is not public"),
            new TestCaseData(31, "</pre>"),
            new TestCaseData(32, "<pre>"),
            new TestCaseData(33, "<li>NUnit.Tests.Assemblies.MockTestFixture.NotRunnableTest : No arguments were provided"),
            new TestCaseData(34, "</pre>"),
            new TestCaseData(35, "<pre>"),
            new TestCaseData(36, "<li>NUnit.Tests.BadFixture.SomeTest : No suitable constructor was found"),
            new TestCaseData(37, "</pre>"),
            new TestCaseData(38, "<pre>"),
            new TestCaseData(39, "<li>NUnit.Tests.IgnoredFixture.Test1 : "),
            new TestCaseData(40, "</pre>"),
            new TestCaseData(41, "<pre>"),
            new TestCaseData(42, "<li>NUnit.Tests.IgnoredFixture.Test2 : "),
            new TestCaseData(43, "</pre>"),
            new TestCaseData(44, "<pre>"),
            new TestCaseData(45, "<li>NUnit.Tests.IgnoredFixture.Test3 : "),
            new TestCaseData(46, "</pre>"),
            new TestCaseData(47, "</ol>"),
            new TestCaseData(48, ""),
            new TestCaseData(49, "<hr>"),
            new TestCaseData(50, "</body>"),
            new TestCaseData(51, "</html>")
        };

        [TestCaseSource("ExpectedLines")]
        public void CheckReport(int line, string text)
        {
            Assert.That(ReportLines[line], Is.EqualTo(text));
        }
    }
}
