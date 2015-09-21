using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI
{
   public class Helper
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public static IList<T> GetEnumNames<T>()
        {
            IList<T> result = new List<T>();
            Type enumType = typeof(T);
            foreach (int enumValue in Enum.GetValues(enumType).Cast<int>())
            {
                T data = (T)Enum.Parse(typeof(T), Enum.GetName(enumType, enumValue));
                result.Add(data);
            }
            return result;
        }
    }
}
