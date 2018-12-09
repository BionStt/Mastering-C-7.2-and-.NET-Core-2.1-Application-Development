﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TaskSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new HttpClient();
            var bag = new ConcurrentBag<string>();
            var task = Task.Factory.StartNew(() =>
            {
                Task.Factory.StartNew(() =>
                {
                    bag.Add(client.GetStringAsync("https://www.packtpub.com").Result);
                }, TaskCreationOptions.AttachedToParent);
                Task.Factory.StartNew(() =>
                {
                    bag.Add(client.GetStringAsync("https://www.amazon.com").Result);
                }, TaskCreationOptions.AttachedToParent);
                Task.Factory.StartNew(() =>
                {
                    bag.Add(client.GetStringAsync("https://www.microhsoft.com").Result);
                }, TaskCreationOptions.AttachedToParent);
            }
            );
            var success = task.ContinueWith(prior => Console.WriteLine($"Success! {bag.Count}"),
                TaskContinuationOptions.NotOnFaulted);
            var error = task.ContinueWith(prior =>
            {
                var ex = prior.Exception;
                Console.WriteLine("Error on fetch!");
            }, TaskContinuationOptions.OnlyOnFaulted);
            Console.Read();
        }
    }
}
