using SQLite;
using System;
using System.Collections.Generic;

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

        public override bool Equals(object obj)
        {
            return obj is BaseModel model &&
                   Id == model.Id &&
                   IsNew == model.IsNew;
        }

        public override int GetHashCode()
        {
            var hashCode = -1952321503;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + IsNew.GetHashCode();
            return hashCode;
        }
    }
}
