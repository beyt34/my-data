using Newtonsoft.Json;

namespace MyData.Core.Extensions {
    public static class ObjectExtensions {
        public static string SerializeToJson<T>(this T serialize) {
            try {
                return JsonConvert.SerializeObject(serialize);
            } catch {
                return string.Empty;
            }
        }
    }
}
