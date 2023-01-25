using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using SkiaSharp;
using MatchEngineApi.DTO;

namespace MatchEngineApi.Controllers.Tools
{
    public static class ToolsExtentions
    {
        public static T GetService<T>  (this IServiceCollection services){
            var provider = services.BuildServiceProvider();
            return provider.GetService<T>();
        }
        public static bool IsBase64StringAnImage(this string base64String)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using (var stream = new MemoryStream(imageBytes))
                {
                    using (var bitmap = SKBitmap.Decode(stream))
                    {
                        return bitmap != null;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool IsBase64StringAnDoubleArray(this string base64String)
        {
            try
            {
                byte[] byteArray = Convert.FromBase64String(base64String);
                const int size = sizeof(double);
                return byteArray.Length % size == 0;
            }
            catch
            {
                return false;
            }
        }

        public static string BuildCacheKey(string memeberKey, string internalKey)
        {
           return memeberKey+"_"+internalKey;
        }
    }
}