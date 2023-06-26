using System.Security.Cryptography.X509Certificates;
using PixBB.Core;
using PixBB.Core.Enumerators;
using PixBB.Core.Models;

namespace PixBB.Test
{
    public abstract class Config
    {
        internal static PixClient GetPixClient()
        {
            return new PixClient(new PixOptions
            {
                Certificate = new X509Certificate2(@"cert-chain.pem"),
                ApiVersion = ApiVersion.V1,
                DeveloperApplicationKey = "32abaa32effa0a0b1ee9197fe0c23111",
                ClientId = "eyJpZCI6ImJiOTY4NjItMWMyMC00MTc4LTliMjctMjY0IiwiY29kaWdvUHVibGljYWRvciI6MCwiY29kaWdvU29mdHdhcmUiOjY0OTY0LCJzZXF1ZW5jaWFsSW5zdGFsYWNhbyI6MX0",
                ClientSecret = "eyJpZCI6IjgyYmFlMmEtNDAyNC00ZjM4LWFkZGItMDY2ZjY1YjIyZDMiLCJjb2RpZ29QdWJsaWNhZG9yIjowLCJjb2RpZ29Tb2Z0d2FyZSI6NjQ5NjQsInNlcXVlbmNpYWxJbnN0YWxhY2FvIjoxLCJzZXF1ZW5jaWFsQ3JlZGVuY2lhbCI6MSwiYW1iaWVudGUiOiJob21vbG9nYWNhbyIsImlhdCI6MTY4NjkzOTU1NTU5OH0"
            });
        }
    }
}