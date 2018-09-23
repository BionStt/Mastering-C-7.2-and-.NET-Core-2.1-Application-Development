﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var source = new CancellationTokenSource();
                source.CancelAfter(TimeSpan.FromSeconds(3));
                var result = await DownloadHomePages(source);
                foreach (var item in result)
                    Console.WriteLine(item);
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.Flatten().InnerException.Message);
            }
        }
        static Task<List<string>> DownloadHomePages(CancellationTokenSource source)
        {
            var client = new HttpClient();
            var bag = new List<string>();
            var token = source.Token;
            var sites = new string[] {"https://www.packtpub.com","https://www.amazon.com","https://www.google.com",
    "https://www.apple.com","https://www.salesforce.com","http://www.microsoft.com","http://www.oracle.com",
    "https://www.ibm.com","https://www.redhat.com","https://www.ubuntu.com","https://www.adobe.com",
    "https://www.autodesk.com","https://www.embarcadero.com"};
            var task = Task.Factory.StartNew(() =>
             {
                 foreach (var site in sites)
                 {
                     Console.WriteLine($"Processing {site}");
                     token.ThrowIfCancellationRequested();
                     bag.Add(client.GetStringAsync(site).Result);
                 }
             }, token);
            task.Wait();
            return Task.FromResult(bag);

        }
    }
}
