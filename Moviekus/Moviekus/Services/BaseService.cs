using Moviekus.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public class BaseService<T> where T : BaseModel
    {
        public event EventHandler<T> OnModelInserted;
        public event EventHandler<T> OnModelUpdated;
        public event EventHandler<T> OnModelDeleted;

        private SQLiteAsyncConnection connection;

        private async Task CreateConnection()
        {
            if (connection != null)
                return;

            //string documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string documentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string databasePath = Path.Combine(documentPath, "Moviekus.db3");

            connection = new SQLiteAsyncConnection(databasePath);

            await connection.CreateTableAsync(typeof(T));
        }

        protected virtual async Task InsertAsync(T model)
        {
            await CreateConnection();
            await connection.InsertAsync(model);
            OnModelInserted?.Invoke(this, model);
        }

        protected virtual async Task UpdateAsync(T model)
        {
            await CreateConnection();
            await connection.UpdateAsync(model);
            OnModelUpdated?.Invoke(this, model);
        }

        protected virtual async Task DeleteAsync(T model)
        {
            await CreateConnection();
            await connection.DeleteAsync(model);
            OnModelDeleted?.Invoke(this, model);
        }
    }
}
