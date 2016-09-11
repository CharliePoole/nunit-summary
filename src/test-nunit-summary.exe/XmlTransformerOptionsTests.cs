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
    public class XmlTransformerOptionsTests
    {
        [TestCase("Help", false)]
        [TestCase("NoHeader", false)]
        [TestCase("Brief", false)]
        [TestCase("Html", false)]
        [TestCase("Transform", null)]
        [TestCase("Output", null)]
        [TestCase("MultipleOutput", false)]
        [TestCase("Input", new string[] { "TestResult.xml" })]
        public void DefaultOptions(string propName, object expected)
        {
            var options = new XmlTransformerOptions(new string[0]);
            Assert.False(options.Error);
            Assert.That(options, Has.Property(propName).EqualTo(expected));
        }

        [TestCase("-help", "Help", true)]
        [TestCase("-noheader", "NoHeader", true)]
        [TestCase("-brief", "Brief", true)]
        [TestCase("-html", "Html", true)]
        [TestCase("-xsl=MyTransform.xslt", "Transform", "MyTransform.xslt")]
        [TestCase("-out=SOMEFILE", "Output", "SOMEFILE")]
        [TestCase("-o=SOMEFILE", "Output", "SOMEFILE")]
        [TestCase("-out=SOMEFILE.html", "Html", true)]
        [TestCase("-o=SOMEFILE.html", "Html", true)]
        [TestCase("-out=*.html", "MultipleOutput", true)]
        [TestCase("-o=*.html", "MultipleOutput", true)]
        [TestCase("RESULT.xml", "Input", new string[] { "RESULT.xml" })]
        public void ValidOptions(string option, string propName, object expected)
        {
            var options = new XmlTransformerOptions(new string[] { option });
            Assert.False(options.Error);
            Assert.That(options, Has.Property(propName).EqualTo(expected));
        }

        [TestCase("-junk")]
        [TestCase("-xsl")]
        [TestCase("-out")]
        [TestCase("-out=")]
        public void InvalidOptions(string option)
        {
            var options = new XmlTransformerOptions(new string[] { option });
            Assert.True(options.Error);
            Assert.That(options.Errors, Has.Some.Length.GreaterThan(0));
        }
    }
}
