using Moviekus.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public class BaseService<T> : IService<T> where T : BaseModel, new()
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

            await connection.CreateTableAsync<T>();
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            await CreateConnection();
            return await connection.Table<T>().ToListAsync();
        }

        public async Task<T> GetAsync(string id)
        {
            await CreateConnection();
            return await connection.Table<T>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public virtual async Task InsertAsync(T model)
        {
            await CreateConnection();
            if (model.IsNew)
            {
                await connection.InsertAsync(model);
                model.IsNew = false;
                OnModelInserted?.Invoke(this, model);
            }
            else await UpdateAsync(model);
        }

        public virtual async Task UpdateAsync(T model)
        {
            await CreateConnection();
            if (!model.IsNew)
            {
                await connection.UpdateAsync(model);
                OnModelUpdated?.Invoke(this, model);
            }
            else await InsertAsync(model);
        }

        public virtual async Task DeleteAsync(T model)
        {
            await CreateConnection();
            await connection.DeleteAsync(model);
            OnModelDeleted?.Invoke(this, model);
        }
    }
}
