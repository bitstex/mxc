using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace  webapi.tests.Infrastructure{

 public class ReusableHttpContent : HttpContent
    {
        private readonly HttpContent _innerContent;

        public ReusableHttpContent(HttpContent innerContent)
        {
            _innerContent = innerContent;
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            await _innerContent.CopyToAsync(stream);
        }

        protected override void SerializeToStream(Stream stream, TransportContext context, CancellationToken cancellationToken)
        {
             _innerContent.CopyTo(stream,context,cancellationToken);
        }
        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            // Don't call base dispose
            //base.Dispose(disposing);
        }
    }
 }