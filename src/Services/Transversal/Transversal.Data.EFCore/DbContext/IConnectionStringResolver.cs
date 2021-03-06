﻿namespace Transversal.Data.EFCore.DbContext
{
    // <summary>
    /// Used to get connection string when a database connection is needed.
    /// </summary>
    public interface IConnectionStringResolver
    {
        /// <summary>
        /// Gets a connection string name (in config file) or a valid connection string
        /// </summary>
        string GetNameOrConnectionString<TDbContext>();
    }
}
