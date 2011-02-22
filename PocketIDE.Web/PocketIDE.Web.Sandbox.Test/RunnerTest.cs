using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PocketIDE.Web.Sandbox.Test
{
    public class RunnerTest
    {
        [Fact]
        public void ShouldRunCode()
        {
            var runner = new Runner();
            string output;
            var success = runner.CompileAndRun(Properties.Resources.HelloWorld, out output);
            Assert.Equal("Hello World!" + Environment.NewLine, output);
            Assert.True(success);
        }
    }
}
