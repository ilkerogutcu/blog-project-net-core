using System.Diagnostics;
using Blog.Core.CrossCuttingConcerns.Caching;
using Blog.Core.CrossCuttingConcerns.Caching.Microsoft;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Utilities.IoC;
using Blog.Core.Utilities.Mail;
using Blog.Core.Utilities.Uri;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMemoryCache();
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();
            serviceCollection.AddSingleton<IMailService, MailService>();
            serviceCollection.AddSingleton<Stopwatch>();
            serviceCollection.AddTransient<FileLogger>();
            serviceCollection.AddTransient<MongoDbLogger>();
            serviceCollection.AddHttpContextAccessor();
            serviceCollection.AddTransient<ElasticsearchLogger>();
            serviceCollection.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext?.Request;
                var uri = string.Concat(request?.Scheme, "://", request?.Host.ToUriComponent(), request?.PathBase);
                return new UriManager(uri);
            });
        }
    }
}