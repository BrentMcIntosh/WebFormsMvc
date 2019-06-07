using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Util
{
	public class EnumContainer
	{
		public virtual String Description { get; set; }
		public virtual String Value { get; set; }
	}

    public class OptionsAttribute : Attribute
    {
        public readonly IEnumerable<string> Options;
        public OptionsAttribute(params string[] options)
        {
            Options = options;
        }
    }

    public class ShortDescriptionAttribute : Attribute
    {
        public readonly string Description;

        public ShortDescriptionAttribute(string description)
        {
            Description = description;
        }
    }

    public static class EnumUtils
	{
        public static IEnumerable<EnumContainer> GetEnumDescriptions<T>()
        {
            return GetEnumDescriptions<T>(x => x.Name); 
        }
        
        public static IEnumerable<EnumContainer> GetEnumDescriptions<T>(System.Linq.Expressions.Expression<Func<FieldInfo, string>> y)
		{
			FieldInfo[] members = typeof (T).GetFields(BindingFlags.Public | BindingFlags.Static);
            //System.Linq.Expressions.Expression<Func<FieldInfo, string>> y = x => x.Name;
            foreach (var member in members.OrderBy(y.Compile()))
			{
				var name = member.Name;
				var descriptionAttribute = member.GetCustomAttributes<DescriptionAttribute>(false).FirstOrDefault();
				var description = descriptionAttribute != null ? descriptionAttribute.Description : name;
				yield return new EnumContainer {Description = description, Value = name};
			}
		}

		public static string GetEnumShortDescription<T>(T value)
		{
			var attrib = GetEnumAttribute<T, ShortDescriptionAttribute>(value);
			return attrib != null ? attrib.Description : Enum.GetName(typeof(T), value);
		}

		public static string GetEnumDescription<T>(T value)
		{
			var attrib = GetEnumAttribute<T, DescriptionAttribute>(value);
			return attrib != null ? attrib.Description : Enum.GetName(typeof (T), value);
		}

		public static string GetEnumCategory<T>(T value)
		{
			var attrib = GetEnumAttribute<T, CategoryAttribute>(value);
			return attrib != null ? attrib.Category : Enum.GetName(typeof(T), value);
		}

		private static V GetEnumAttribute<T,V>(T t) where V : Attribute
		{
			var name = Enum.GetName(typeof (T), t);
			var field = typeof (T)
							.GetFields(BindingFlags.Public | BindingFlags.Static)
							.Where(m => m.Name == name)
							.FirstOrDefault();
			if (field != null)
				return field.GetCustomAttributes<V>().FirstOrDefault();
			return null;
		}

		public static T Parse<T>(string enumName)
		{
			if (!string.IsNullOrEmpty(enumName))
				return (T)Enum.Parse(typeof(T), enumName, true);
			return default(T);
		}

        public static T? ParseNullable<T>(string enumName) where T : struct
        {
            T parsed;
            if (TryParse(enumName, out parsed))
                return parsed;
            return null;
        }

		public static bool TryParse<T>(string enumName, out T t)
		{
			try
			{
				t= (T)Enum.Parse(typeof(T), enumName, true);
				return true;
			}
			catch { }
			t = default(T);
			return false;
		}

        public static List<T> GetEnumToList<T>()
        {
            Type enumType = typeof(T);
            Array enumValArray = Enum.GetValues(enumType);
            List<T> enumValList = new List<T>(enumValArray.Length);
            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }
            return enumValList;
        } 
	}
}
