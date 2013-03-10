using FluentNHibernate.Conventions;
using System;

namespace MappingExample.Mappings
{

	public class TableNameConvention : IClassConvention
	{
		public void Apply(FluentNHibernate.Conventions.Instances.IClassInstance instance)
		{
			instance.Table(GetPluralOfText(instance.EntityType.Name));
		}

        public static string GetPluralOfText(string text)
        {
            string pluralString = text;
            string lastCharacter = pluralString.Substring(pluralString.Length - 1).ToLower();

            // y's become ies (such as Category to Categories)
            if (string.Equals(lastCharacter, "y", StringComparison.InvariantCultureIgnoreCase))
            {
                pluralString = pluralString.Remove(pluralString.Length - 1);
                pluralString += "ie";
            }

            // ch's become ches (such as Church to Churches)
            if (string.Equals(pluralString.Substring(pluralString.Length - 2), "ch", StringComparison.InvariantCultureIgnoreCase))
            { pluralString += "e"; }

            switch (lastCharacter)
            {
                case "s":
                    return pluralString + "es";
                default:
                    return pluralString + "s";
            }
        }
	}
}