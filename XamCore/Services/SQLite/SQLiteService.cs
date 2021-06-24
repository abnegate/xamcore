using System;
using System.IO;
using SQLite;

namespace XamCore.Services
{
    public class SQLiteService : ISQLiteService
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var file = "XamCore.sqlite";
            var directory = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData);

            return new SQLiteAsyncConnection(Path.Combine(directory, file));
        }
    }
}
