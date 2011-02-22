using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using PocketIDE.Web.Code;
using Xunit;

namespace PocketIDE.Web.Test
{
    public class CodeResourceTest
    {
        [Fact]
        public void IncorrectHashShouldFail()
        {
            var program = new Program {AuthorId = "Foo", Code = "Bar", Hash = "FooBar"};
            var responseMessage = new HttpResponseMessage();
            var actual = new CodeResource().Run(program, responseMessage);
            Assert.Null(actual);
            Assert.Equal(HttpStatusCode.Forbidden, responseMessage.StatusCode);
        }

        [Fact]
        public void CorrectShouldRun()
        {
            var program = new Program { AuthorId = "Foo", Code = Convert.ToBase64String(Encoding.UTF8.GetBytes(Properties.Resources.HelloWorld)) };
            program.Hash = Program.CreateHash(program);
            var responseMessage = new HttpResponseMessage();
            var actual = new CodeResource().Run(program, responseMessage);
            Assert.NotNull(actual);
            Assert.Equal("Hello World!", actual.Output);
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        }
    }
}
