using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Arrowgene.Services.Logging;
using Ddo.Server.Setting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions.Internal;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ILogger = Arrowgene.Services.Logging.ILogger;

namespace Ddo.Server.Web.Server.Kestrel
{
    /// <summary>
    /// Implementation of Kestrel server as backend
    /// </summary>
    public class KestrelWebServer : IWebServer
    {
        private CancellationTokenSource _cancellationTokenSource;
        private ApplicationLifetime _applicationLifetime;
        private IServer _server;
        private int _shutdownTimeout = 10000;
        private IWebServerHandler _handler;
        private DdoSetting _setting;
        private ILogger _logger;

        public KestrelWebServer(DdoSetting setting)
        {
            _logger = LogProvider.Logger(this);
            _setting = setting;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void SetHandler(IWebServerHandler handler)
        {
            _handler = handler;
        }

        public async Task Start()
        {
            IHttpApplication<HostingApplication.Context> app;
            try
            {
                if (_handler == null)
                {
                    throw new Exception("Missing Handler - Call SetHandler()");
                }

                ILoggerFactory loggerFactory = new LoggerFactory();
                loggerFactory.AddProvider(new KestrelLoggerProvider());
                ServiceCollection services = new ServiceCollection();
                services.AddSingleton(loggerFactory);
                services.AddLogging();

                IServiceProvider serviceProvider = GetProviderFromFactory(services);
                IOptions<KestrelServerOptions> kestrelServerOptions = Options.Create(new KestrelServerOptions());
                kestrelServerOptions.Value.ApplicationServices = serviceProvider;
                kestrelServerOptions.Value.ListenAnyIP(_setting.WebSetting.HttpPort);
                if (_setting.WebSetting.HttpsEnabled)
                {
                    kestrelServerOptions.Value.ListenAnyIP(_setting.WebSetting.HttpsPort,
                        listenOptions =>
                        {
                            X509Certificate2 cert = new X509Certificate2(_setting.WebSetting.HttpsCertPath);
                            listenOptions.UseHttps(cert);
                        });
                }

                kestrelServerOptions.Value.AddServerHeader = false;

                IOptions<SocketTransportOptions> socketTransportOptions = Options.Create(new SocketTransportOptions());
                _applicationLifetime = new ApplicationLifetime(
                    loggerFactory.CreateLogger<ApplicationLifetime>()
                );
                ITransportFactory transportFactory = new SocketTransportFactory(
                    socketTransportOptions, _applicationLifetime, loggerFactory
                );


                _server = new KestrelServer(kestrelServerOptions, transportFactory, loggerFactory);
                DiagnosticListener diagnosticListener = new DiagnosticListener("a");
                IOptions<FormOptions> formOptions = Options.Create(new FormOptions());
                IHttpContextFactory httpContextFactory = new HttpContextFactory(formOptions);
                app = new HostingApplication(
                    RequestDelegate,
                    loggerFactory.CreateLogger<KestrelWebServer>(),
                    diagnosticListener,
                    httpContextFactory
                );
            }
            catch (Exception ex)
            {
                _logger.Exception(ex);
                return;
            }

            Task kestrelStartup = _server.StartAsync(app, _cancellationTokenSource.Token);
            await kestrelStartup;
            _cancellationTokenSource.Token.Register(
                state => ((IApplicationLifetime) state).StopApplication(),
                _applicationLifetime
            );
            TaskCompletionSource<object> completionSource = new TaskCompletionSource<object>(
                TaskCreationOptions.RunContinuationsAsynchronously
            );
            _applicationLifetime.ApplicationStopping.Register(
                obj => ((TaskCompletionSource<object>) obj).TrySetResult(null),
                completionSource
            );
            Task<object> kestrelCompleted = completionSource.Task;
            object kestrelCompletedResult = await kestrelCompleted;
            Task kestrelShutdown = _server.StopAsync(new CancellationToken());
            await kestrelShutdown;
        }

        public async Task Stop()
        {
            CancellationToken token = new CancellationTokenSource(_shutdownTimeout).Token;
            _applicationLifetime?.StopApplication();
            if (_server != null)
            {
                await _server.StopAsync(token).ConfigureAwait(false);
            }

            _applicationLifetime?.NotifyStopped();
            HostingEventSource.Log.HostStop();
        }

        /// <summary>
        /// Called whenever a web request arrives.
        /// - Maps Kestrel HttpRequest/HttpResponse to WebRequest/WebResponse
        /// - Calls router to handle the request
        /// </summary>
        private async Task RequestDelegate(HttpContext context)
        {
            WebRequest request = new WebRequest();
            request.Host = context.Request.Host.Host;
            request.Port = context.Request.Host.Port;
            request.Method = WebRequest.ParseMethod(context.Request.Method);
            request.Path = context.Request.Path;
            request.Scheme = context.Request.Scheme;
            request.ContentType = context.Request.ContentType;
            request.QueryString = context.Request.QueryString.Value;
            request.ContentLength = context.Request.ContentLength;
            foreach (string key in context.Request.Headers.Keys)
            {
                request.Header.Add(key, context.Request.Headers[key]);
            }

            foreach (string key in context.Request.Query.Keys)
            {
                request.QueryParameter.Add(key, context.Request.Query[key]);
            }

            foreach (string key in context.Request.Cookies.Keys)
            {
                request.Cookies.Add(key, context.Request.Cookies[key]);
            }

            await context.Request.Body.CopyToAsync(request.Body);
            request.Body.Position = 0;
            WebResponse response = await _handler.Handle(request);
            context.Response.StatusCode = response.StatusCode;
            foreach (string key in response.Header.Keys)
            {
                context.Response.Headers.Add(key, response.Header[key]);
            }
            
            response.Body.Position = 0;
            await response.Body.CopyToAsync(context.Response.Body);
        }

        private IServiceProvider GetProviderFromFactory(IServiceCollection collection)
        {
            ServiceProvider provider = collection.BuildServiceProvider();
            IServiceProviderFactory<IServiceCollection> service =
                provider.GetService<IServiceProviderFactory<IServiceCollection>>();
            if (service == null || service is DefaultServiceProviderFactory)
            {
                return provider;
            }

            using (provider)
            {
                return service.CreateServiceProvider(service.CreateBuilder(collection));
            }
        }
    }
}
