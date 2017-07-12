using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace MinimalWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .Configure(app => app.Map("/echo", EchoHandler))
                .Build();

            host.Run();
        }

        private static void EchoHandler(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(new
                    {
                        StatusCode = (string)context.Response.StatusCode.ToString(),
                        PathBase = (string)context.Request.PathBase.Value.Trim('/'),
                        Path = (string)context.Request.Path.Value.Trim('/'),
                        Method = (string)context.Request.Method,
                        Scheme = (string)context.Request.Scheme,
                        ContentType = (string)context.Request.ContentType,
                        ContentLength = (long?)context.Request.ContentLength,
                        QueryString = (string)context.Request.QueryString.ToString(),
                        Query = context.Request.Query
                            .ToDictionary(
                                _ => _.Key,
                                _ => _.Value,
                                StringComparer.OrdinalIgnoreCase)
                    })
                );
            });
        }
    }
}
