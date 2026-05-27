using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace WebThucPhamSach.Helpers
{
    /// <summary>
    /// Các phương thức mở rộng cho ISession để lưu trữ và lấy đối tượng phức tạp dạng JSON.
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// Serialize một đối tượng sang JSON và lưu trữ vào Session.
        /// </summary>
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        /// <summary>
        /// Lấy chuỗi JSON từ Session và deserialize về kiểu đối tượng mong muốn.
        /// </summary>
        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
