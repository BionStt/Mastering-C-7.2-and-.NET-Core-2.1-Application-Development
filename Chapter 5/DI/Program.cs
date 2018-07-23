﻿using System;
using Autofac;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace AirportDI
{
    class Program
    {
        private static IContainer Container { get; set; }
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            // Register all engines in the currrent library
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());
            // Construct the container
            Container = builder.Build();
            //get both engines from our container
            using (var scope = Container.BeginLifetimeScope())
            {
                var jet = scope.Resolve<JetEngine>();
                jet.Start();
                var prop = scope.Resolve<PropellerEngine>();
                prop.Start();
                var solar = scope.Resolve<SolarEngine>();
                solar.Start();
            }
        }
    }
}