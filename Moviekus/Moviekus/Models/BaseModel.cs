using SQLite;
using System;

namespace Moviekus.Models
{
    public class BaseModel
    {
        [PrimaryKey]
        public string Id { get; set; }

        [Ignore]
        public bool IsNew { get; set; }

        public BaseModel()
        {
            Id = Guid.NewGuid().ToString();
            IsNew = false;
        }

        public static T CreateNewModel<T>() where T : BaseModel, new()
        {
            var newModel = new T
            {
                IsNew = true
            };
            return newModel;
        }
    }
}
