using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Moviekus.ServiceContracts
{
    public enum MovieSortOrder
    {
        [Display(Name = "Keine")]
        None,
        [Display(Name = "Titel")]
        Title,
        [Display(Name = "Laufzeit")]
        Runtime,
        [Display(Name = "Bewertung")]
        Rating,
        [Display(Name = "Zuletzt gesehen")]
        LastSeen,
        [Display(Name = "Veröffentlicht")]
        ReleaseDate,
        [Display(Name = "Episode")]
        EpisodeNumber
    }

    public static class MovieSortOrderHelper
    {
        public static string[] GetDisplayNames()
        {
            var displayFields = new List<string>();

            var enumType = typeof(MovieSortOrder);
            foreach(var value in Enum.GetValues(typeof(MovieSortOrder)))
            {
                var memberInfos = enumType.GetMember(value.ToString());
                var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);
                var valueAttributes = enumValueMemberInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
                if (valueAttributes.Any())
                    displayFields.Add(((DisplayAttribute)valueAttributes[0]).Name);
            }

            return displayFields.ToArray();
        }

        public static MovieSortOrder GetSortOrderFromDisplayName(string displayName)
        {
            // Bestimme alle Ausprägungen des enums
            var enumType = typeof(MovieSortOrder);
            var memberInfos = enumType.GetMembers().Where(d => d.DeclaringType == enumType);

            foreach (var info in memberInfos)
            {
                // Ermittle das DisplayAttribe für diese enum-Ausprägung
                var displayAttribute = info.CustomAttributes.FirstOrDefault(a => a.AttributeType == typeof(DisplayAttribute));

                if (displayAttribute != null)
                {
                    // Prüfe, ob der Name des DisplayAttribute mit dem gesuchten Namen übereinstimmt
                    var namedArgument = displayAttribute.NamedArguments.Where(n => n.TypedValue.Value.ToString() == displayName);
                    if (namedArgument.Any())
                        return (MovieSortOrder)Enum.Parse(typeof(MovieSortOrder), info.Name);
                }
            }

            return MovieSortOrder.None;
        }
    }
}
