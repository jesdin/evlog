using Evlog.Domain.EventAggregate.Commands;
using Evlog.Domain.EventAggregate.Queries;
using Evlog.Domain.Events.Handlers;
using Evlog.Domain.UserAggregate.Commands;
using Evlog.Domain.UserAggregate.Queries;
using Evlog.Infrastructure;
using Evlog.Infrastructure.Commands;
using Evlog.Infrastructure.DataModels;
using Evlog.Infrastructure.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Evlog.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEvlogQueries(this IServiceCollection services)
        {
            // Add EventPost queries
            services.AddTransient<IAllEventsQuery, AllEventsQuery>();
            services.AddTransient<IPastEventsQuery, PastEventsQuery>();
            services.AddTransient<IUpcomingEventsQuery, UpcomingEventsQuery>();
            services.AddTransient<IEventQuery, EventQuery>();

            // Add User queries
            services.AddTransient<IUserQuery, UserQuery>();
            services.AddTransient<IUserExistsQuery, UserExistsQuery>();
        }

        public static void AddEvlogCommands(this IServiceCollection services)
        {
            services.AddTransient<IRegisterUserCommand, RegisterUserCommand>();
            services.AddTransient<ICreateUserCommand, CreateUserCommand>();
        }

        public static void AddEvlogEventHandlers(this IServiceCollection services)
        {
            services.AddTransient<IRegistrationCompletedHandler, RegistrationCompletedHandler>();
        }


        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration config)
        {
            var appsettings = config.GetSection("AppSettings").Get<AppSettings>();
            if(appsettings.UseMongo)
            {
                var configSection = config.GetSection("Mongo");
                var mongoConfig = configSection.Get<MongoConfig>();
                var mongoClient = new MongoClient(connectionString: mongoConfig.ConnectionString) as IMongoClient;
                var database = mongoClient.GetDatabase(mongoConfig.Database);
                var eventsCollection = database.GetCollection<EventPostDM>("Events");
                var usersCollection = database.GetCollection<UserDM>("Users");

                services.Configure<MongoConfig>(configSection);
                services.AddSingleton(mongoClient);
                services.AddSingleton(database);
                services.AddSingleton(eventsCollection);
                services.AddSingleton(usersCollection);
                services.AddTransient<MongoDbContext>();
            }
            return services;
        }

        public static void AddEvlogMvc(this IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true)
                    .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

    }
}
