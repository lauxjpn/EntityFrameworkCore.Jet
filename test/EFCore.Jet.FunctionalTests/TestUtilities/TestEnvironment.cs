// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Data.Common;
using System.IO;
using System.Data.Jet;
using Microsoft.Extensions.Configuration;

namespace EntityFrameworkCore.Jet.FunctionalTests.TestUtilities
{
    public static class TestEnvironment
    {
        public static IConfiguration Config { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json", optional: true)
            .AddJsonFile("config.test.json", optional: true)
            .AddEnvironmentVariables()
            .Build()
            .GetSection("Test:Jet");

        public static string DefaultConnection { get; } = Config["DefaultConnection"]
            ?? JetConnection.GetConnectionString("Jet.accdb", TestEnvironment.DataAccessProviderFactory);

        public static bool IsConfigured
        {
            get
            {
                var dataAccessType = JetConnection.GetDataAccessType(DefaultConnection);
                var dataAccessProviderFactory = JetFactory.Instance.GetDataAccessProviderFactory(dataAccessType);
                var connectionStringBuilder = dataAccessProviderFactory.CreateConnectionStringBuilder();
                connectionStringBuilder.ConnectionString = DefaultConnection;
                
                return !string.IsNullOrEmpty(connectionStringBuilder.GetDataSource());
            }
        }

        public static DataAccessProvider DataAccessProvider { get; } = JetConnection.GetDataAccessType(DefaultConnection);
        public static DbProviderFactory DataAccessProviderFactory { get; } = JetFactory.Instance.GetDataAccessProviderFactory(JetConnection.GetDataAccessType(DefaultConnection));
        
        public static bool IsLocalDb { get; } = true;

        public static bool IsCI { get; } = Environment.GetEnvironmentVariable("PIPELINE_WORKSPACE") != null
            || Environment.GetEnvironmentVariable("TEAMCITY_VERSION") != null;

        public static bool? GetFlag(string key)
            => bool.TryParse(Config[key], out var flag) ? flag : (bool?)null;

        public static int? GetInt(string key)
            => int.TryParse(Config[key], out var value) ? value : (int?)null;
    }
}
