using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviekus.Models
{
    public class BaseModel : INotifyPropertyChanged
    {
        [Key]
        public string Id { get; set; }

        [NotMapped]
        public bool IsNew { get; set; }

        [NotMapped]
        public bool IsModified { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; }


        public BaseModel()
        {
            Id = Guid.NewGuid().ToString();
            // Es ist effizienter, IsNew mit false zu belegen, denn dies gilt für alle geladenen Objekte
            // Für neu erzeugte Models muss es explizit auf true gesetzt werden => CreateNew aufrufen
            IsNew = IsModified = IsDeleted = false;
        }

        // Es genügt hier, das Event zu definieren, es muss nicht explizit gefeuert werden, denn das macht FodyWeavers
        public event PropertyChangedEventHandler PropertyChanged;

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
                   IsNew == model.IsNew &&
                   IsModified == model.IsModified &&
                   IsDeleted == model.IsDeleted;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, IsNew, IsModified, IsDeleted);
        }
    }
}
