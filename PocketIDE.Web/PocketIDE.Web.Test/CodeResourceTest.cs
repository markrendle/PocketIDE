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
    }
}
