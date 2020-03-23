using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviekus.Models
{
    public class BaseModel
    {
        [Key]
        public string Id { get; set; }

        [NotMapped]
        public bool IsNew { get; set; }

        public BaseModel()
        {
            Id = Guid.NewGuid().ToString();
            // Es ist effizienter, IsNew mit false zu belegen, denn dies gilt für alle geladenen Objekte
            // Für neu erzeugte Models muss es explizit auf true gesetzt werden => CreateNew aufrufen
            IsNew = false;
        }

        public static T CreateNew<T>() where T : BaseModel, new()
        {
            T model = new T
            {
                IsNew = true
            };
            return model;
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
