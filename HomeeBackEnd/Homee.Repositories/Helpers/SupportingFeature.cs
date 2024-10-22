using Homee.DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.Helpers
{
    public static class ReflectionExtensions
    {
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            PropertyInfo propInfo = obj.GetType().GetProperty(propertyName);
            if (propInfo == null) throw new ArgumentException($"Property '{propertyName}' not found on '{obj.GetType().Name}'");

            return propInfo.GetValue(obj);
        }
        public static IQueryable<TEntity> IncludeAll<TEntity>(this IQueryable<TEntity> query, DbContext context) where TEntity : class
        {
            var entityType = context.Model.FindEntityType(typeof(TEntity));

            if (entityType == null)
            {
                throw new InvalidOperationException($"The entity type '{typeof(TEntity).Name}' was not found in the model.");
            }

            // Get all the navigation properties of the entity
            var navigations = entityType.GetNavigations();

            foreach (var navigation in navigations)
            {
                // Dynamically call Include for each navigation property
                query = query.Include(navigation.Name);
            }

            return query;
        }

    }
    public class SupportingFeature
    {
        private static SupportingFeature instance = null;
        private static readonly object instanceLock = new object();
        public static SupportingFeature Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SupportingFeature();
                    }
                    return instance;
                }
            }
        }
        public long GenerateUniqueCode()
        {
            byte[] buffer = new byte[8]; // 8 bytes để tạo ra một giá trị long
            RandomNumberGenerator.Fill(buffer); // Tạo giá trị ngẫu nhiên an toàn

            long uniqueCode = BitConverter.ToInt64(buffer, 0);

            return Math.Abs(uniqueCode); // Trả về giá trị dương
        }

        public long GetOrderCode(int subId)
        {
            long uniqueCode = GenerateUniqueCode();

            // Dịch subId sang hàng tỷ (10^12) để tránh trùng lặp với mã unique
            long combinedCode = (subId * 1000000000000L) + (uniqueCode % 1000000000000L);

            return combinedCode;
        }

        public int ExtractSubIdFromCombinedCode(long combinedCode)
        {
            return (int)(combinedCode / 1000000000000L);
        }

        public long ExtractUniqueCodeFromCombinedCode(long combinedCode)
        {
            return combinedCode % 1000000000000L;
        }

        public string GenerateOTP()
        {
            // Create a random number generator
            Random random = new Random();

            // Generate a random number with the specified length
            string otp = string.Empty;

            for (int i = 0; i < 6; i++)
            {
                otp += random.Next(0, 10); // Append a digit between 0 and 9
            }

            return otp;
        }
        public void CopyValues<T>(T target, T source)
        {
            if (target == null || source == null)
            {
                throw new ArgumentNullException("Target or Source cannot be null");
            }

            foreach (PropertyInfo property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.CanWrite)
                {
                    var value = property.GetValue(source);
                    property.SetValue(target, value);
                }
            }
        }
        public static bool GetValueFromSession<T>(string key, out T value, HttpContext context)
        {
            value = JsonConvert.DeserializeObject<T>("");
            context.Session.TryGetValue(key, out byte[] o);
            if (o == null)
            {
                return false;
            }
            value = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(o));
            return true;
        }

        public static void SetValueToSession(string key, object value, HttpContext context)
        {
            context.Session.Set(key, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
        }

        //public object GetValueFromSession(string key, HttpContext context)
        //{
        //    context.Session.TryGetValue(key, out byte[] o);
        //    return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(o));
        //}

        //public void SetValueToSession(string key, object value, HttpContext context)
        //{
        //    context.Session.Set(key, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
        //}

        public bool TryParseJsonArrayGrades(string jsonString, out List<double> values)
        {
            try
            {
                values = JsonConvert.DeserializeObject<List<double>>(jsonString);
                return values != null;
            }
            catch
            {
                values = null;
                return false;
            }
        }

        public List<T> FilterModel<T>(List<T> list, Dictionary<string, object> categories)
        {
            if (categories.Count == 0) return list;
            foreach (var category in categories)
            {
                if (!list.Any()) return list;
                var accessory = list.FirstOrDefault();
                var type = accessory.GetPropertyValue(category.Key).GetType();
                switch (type.Name)
                {
                    case "Int":
                        if (int.TryParse(category.Value.ToString(), out int grade))
                            list = list.Where(d => int.TryParse(d.GetPropertyValue(category.Key).ToString(), out var value) &&
                                                           value == grade).ToList();
                        break;
                    case "Double":
                        if (TryParseJsonArrayGrades(category.Value.ToString(), out List<double> range))
                            list = list.Where(d => double.TryParse(d.GetPropertyValue(category.Key).ToString(), out var value) && range[0] <= value && value <= range[1]).ToList();
                        break;
                    case "Datetime":
                        if (TryParseJsonArrayDatetimes(category.Value.ToString(), out List<DateTime> datetimes))
                        {
                            list = list.Where(d => DateTime.TryParse(d.GetPropertyValue(category.Key).ToString(), out var value) && value.CompareTo(datetimes[0]) >= 0 && value.CompareTo(datetimes[1]) <= 0).ToList();
                        }
                        break;
                    default:
                        list = list.Where(d => d.GetPropertyValue(category.Key).ToString().Trim().ToUpper()
                                                            .Contains(category.Value.ToString().Trim().ToUpper())).ToList();
                        break;
                }
            }
            return list;
        }

        public bool TryParseJsonArrayDatetimes(string jsonString, out List<DateTime> values)
        {
            try
            {
                //values = JsonConvert.DeserializeObject<List<DateTime>>(jsonString);
                values = new List<DateTime>();
                return values != null;
            }
            catch
            {
                values = null;
                return false;
            }
        }
        public Dictionary<int, string> GetEnumName<TEnum>()
        {
            Dictionary<int, string> enumValues = new Dictionary<int, string>();

            foreach (int e in Enum.GetValues(typeof(TEnum)))
            {
                enumValues.Add(e, Enum.GetName(typeof(TEnum), e));
            }

            return enumValues;
        }
    }
}
