using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportTicketApp.Data.Mongo.Configurations
{
    public class MongoDbOptions : IConnectionStringOption
    {
        public  const   string      SectionName         = "MongoDb";
        public          string?     ConnectionString    { get; set; }
        public          string?     DatabaseName        { get; set; }
        public          string?     ServiceName         { get; set; }

        string IConnectionStringOption.Protocol { get; } = "mongodb";
        string IConnectionStringOption.Username { get; } = "";
        string IConnectionStringOption.Password { get; } = "";
    }

    public interface IConnectionStringOption
    {
        string? ConnectionString { get; }
        string? ServiceName { get; }
        string Protocol { get; }
        string Username { get; }
        string Password { get; }
    }
    //public static class UriOptionsExtensions
    //{
    //    public static string GetConnectionString(this IConnectionStringOption option, IServiceDiscoverProvider serviceDiscoverProvider)
    //    {
    //        if (option == null)
    //            return string.Empty;

    //        if (string.IsNullOrWhiteSpace(option.ConnectionString))
    //            if (string.IsNullOrWhiteSpace(option.ServiceName))
    //                return string.Empty;
    //            else
    //                return serviceDiscoverProvider.GetServiceUri(option.ServiceName, option.Protocol).Result!;
    //        else
    //            return option.ConnectionString;
    //    }
    //}
}
