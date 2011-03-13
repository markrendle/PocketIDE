using System;

namespace PocketIDE.Web.Code
{
    public class ProgramAuditor
    {
        private readonly IBlobHelper _blobHelper;

        public ProgramAuditor(IBlobHelper blobHelper)
        {
            _blobHelper = blobHelper;
        }

        public void Audit(Program program)
        {
            _blobHelper.SaveObjectAsync("audit",  DateTime.UtcNow.ToString("yyyyMMddHHmmssfffffff"), program);
        }
    }
}