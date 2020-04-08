using HikesOfAmerica.Core;
using HikesOfAmerica.Core.Interfaces;
using HikesOfAmerica.Core.Publishing;
using HikesOfAmerica.Data.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace HikesOfAmerica.Web.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mongoConnection = Configuration["ConnectionStrings:MongoDB"];
            var s3KeyId = Configuration["Keys:S3KeyId"];
            var s3Key = Configuration["Keys:S3Key"];

            var repo = new MongoDbRepository(mongoConnection);

            services.AddCors(x => x.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader())
            );

            var host = Configuration["RabbitMQ:Host"];
            var user = Configuration["RabbitMQ:Username"];
            var pwd = Configuration["RabbitMQ:Password"];

            ConnectionFactory connectionFactory = new ConnectionFactory
            {
                UserName = user,
                Password = pwd,
                HostName = host
            };

            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "Locations", type: ExchangeType.Fanout);
            channel.QueueBind("HikesOfAmerica.Messaging.Services.Consumers.LocationsConsumerQueue", "Locations", "");

            var publisher = new RabbitMQPublisher(channel);

            services.AddSingleton<IPublisher>(publisher);
            services.AddSingleton<IRepository>(repo);
            services.AddSingleton<ILocationsManager>(new LocationsManager(repo));
            services.AddSingleton<ISubmissionsManager>(new SubmissionsManager(repo, publisher));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
