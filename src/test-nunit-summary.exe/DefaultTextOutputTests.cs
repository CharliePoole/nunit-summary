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
    public class DefaultTextOutputTests : XmlTransformerTests
    {
        protected override string Input
        {
            get { return "MockAssemblyTestResult-2.6.4.xml"; }
        }

        protected override string Output
        {
            get { return "MockAssemblyDefaultOutput-2.6.4.txt"; }
        }

        protected override string Options
        {
            get { return ""; }
        }

        static TestCaseData[] ExpectedLines = new TestCaseData[]
        {
            new TestCaseData(0, @"***** C:\Program Files\NUnit 2.6.4\bin\tests\mock-assembly.dll"),
            new TestCaseData(1, ""),
            new TestCaseData(2, "NUnit Version 2.6.4.14350  2016-09-10  11:13:55"),
            new TestCaseData(3, ""),
            new TestCaseData(4, "Runtime Environment -"),
            new TestCaseData(5, "   OS Version: Microsoft Windows NT 6.2.9200.0"),
            new TestCaseData(6, "  CLR Version: " + Environment.Version),
            new TestCaseData(7, ""),
            new TestCaseData(8, "Tests run: 21, Errors: 1, Failures: 1, Inconclusive: 1, Time: 0.088 seconds"),
            new TestCaseData(9, "  Not run: 7, Invalid: 3, Ignored: 4, Skipped: 0"),
            new TestCaseData(10, ""),
            new TestCaseData(11, "Failures:"),
            new TestCaseData(12, "1) NUnit.Tests.Assemblies.MockTestFixture.FailingTest : Intentional failure"),
            new TestCaseData(13, "at NUnit.Tests.Assemblies.MockTestFixture.FailingTest()"),
            new TestCaseData(14, ""),
            new TestCaseData(15, "2) NUnit.Tests.Assemblies.MockTestFixture.TestWithException : System.ApplicationException : Intentional Exception"),
            new TestCaseData(16, "at NUnit.Tests.Assemblies.MockTestFixture.MethodThrowsException()"),
            new TestCaseData(17, "at NUnit.Tests.Assemblies.MockTestFixture.TestWithException()"),
            new TestCaseData(18, ""),
            new TestCaseData(19, "Tests not run:"),
            new TestCaseData(20, "1) NUnit.Tests.Assemblies.MockTestFixture.MockTest4 : ignoring this test method for now"),
            new TestCaseData(21, "2) NUnit.Tests.Assemblies.MockTestFixture.MockTest5 : Method is not public"),
            new TestCaseData(22, "3) NUnit.Tests.Assemblies.MockTestFixture.NotRunnableTest : No arguments were provided"),
            new TestCaseData(23, "4) NUnit.Tests.BadFixture.SomeTest : No suitable constructor was found"),
            new TestCaseData(24, "5) NUnit.Tests.IgnoredFixture.Test1 : "),
            new TestCaseData(25, "6) NUnit.Tests.IgnoredFixture.Test2 : "),
            new TestCaseData(26, "7) NUnit.Tests.IgnoredFixture.Test3 : ")
        };

        [TestCaseSource("ExpectedLines")]
        public void CheckReport(int line, string text)
        {
            Assert.That(ReportLines[line], Is.EqualTo(text));
        }
    }
}
