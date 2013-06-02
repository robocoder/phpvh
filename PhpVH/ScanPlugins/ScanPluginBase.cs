using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhpVH.CodeAnalysis;

namespace PhpVH.ScanPlugins
{
    public abstract class ScanPluginBase
    {
        private RequestBuilder _requestBuilder = new RequestBuilder();

        public RequestBuilder RequestBuilder
        {
            get { return _requestBuilder; }
        }

        public abstract int ModeCount { get; }

        public abstract string Server { get; set; }

        protected abstract string BuildRequestCore(int Mode, string TargetFile, FileTrace SourceTrace);

        public string BuildRequest(int Mode, string TargetFile, FileTrace SourceTrace)
        {
            _requestBuilder.Calls = SourceTrace.Calls;

            return BuildRequestCore(Mode, TargetFile, SourceTrace);
        }

        protected abstract ScanAlert ScanTraceCore(FileTrace TargetTrace);

        public ScanAlert ScanTrace(FileTrace targetTrace)
        {
            return ScanTraceCore(targetTrace);
        }

        public virtual void Initialize() { }

        public virtual void Uninitialize() { }        
    }    
}
