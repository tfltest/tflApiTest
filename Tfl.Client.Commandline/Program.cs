using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Tfl.Client.Commandline.Contracts;
using Tfl.Client.Commandline.Services;

namespace Tfl.Client.Commandline
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = InitServices();

            using (var scope = container.BeginLifetimeScope())
            {
                var roadService = scope.Resolve<IRoadService>();

                Start:

                Console.ResetColor();
                Console.Write("Please enter road id: ");

                string id = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(id))
                {
                    Console.WriteLine();
                    goto Start;
                }

                var response = roadService.GetRoadStatus(id).Result;

                if (response.IsSuccess)
                {
                    var roadStatus = response.Response.First();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(roadStatus);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{id} is not a valid road.");
                    Console.WriteLine(response.ApiError);
                }

                goto Start;
            }
        }

        private static IContainer InitServices()
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance<IDictionary<string, string>>(new Dictionary<string, string>()
            {
                { "app_id", ConfigurationManager.AppSettings["app_id"] },
                { "developer_key", ConfigurationManager.AppSettings["developer_key"] },
                { "base_api_url", ConfigurationManager.AppSettings["base_api_url"] }
            });

            builder.RegisterType<ApplicationSettingsService>().As<IApplicationSettingsService>();

            builder.RegisterType<RoadService>().As<IRoadService>();

            return builder.Build();
        }
    }
}