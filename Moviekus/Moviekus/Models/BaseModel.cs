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
            IsNew = true;
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
