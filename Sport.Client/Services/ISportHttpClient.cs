using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sport.Client.Services
{
    public interface ISportHttpClient
    {
        Task<HttpClient> GetClient();
    }
}
