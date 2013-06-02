using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhpVH.ScanPlugins
{
    //public class ResponseSplittingScanPlugin : ScanPlugin
    //{
    //    const string _badCharsIn = "test%0A%0Dtest", _badCharsOut = "test\x0A";

    //    private string _server;

    //    public override string Server
    //    {
    //        get { return _server; }
    //        set { _server = value; }
    //    }

    //    public override string BuildRequest(string TargetFile, FileTrace SourceTrace)
    //    {
    //        return _requestBuilder.CreateRequest(TargetFile, Server, _badCharsIn);
    //    }

    //    public override ScanAlert ScanTrace(FileTrace TargetTrace)
    //    {
    //        int headerEnd = TargetTrace.Response.IndexOf("\r\n\r\n");

    //        var s = headerEnd != -1 ?
    //            TargetTrace.Response.Remove(headerEnd) :
    //            TargetTrace.Response;

    //        TraceHelper.WriteLineFormat("\r\n\r\n" + s);

    //        if (s.Contains(_badCharsOut))
    //            TraceHelper.WriteLineFormat("\r\n\r\n" + TargetTrace.Response + "\r\n\r\n");

    //        return null;
    //    }
    //}
}
