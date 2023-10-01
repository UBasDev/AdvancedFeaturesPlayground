using AuthorService.Application.Interfaces.Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorService.Persistence.Hangfire
{
    public class SyncAuthors : ISyncAuthors
    {
        public async Task Test1()
        {
            Console.WriteLine($"******************************Authors have been synchronized: {DateTime.Now}******************************");
        }
    }
}
