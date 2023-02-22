using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using SkiaSharp;
using MatchEngineApi.DTO;
using ItZnak.Infrastruction.Services;
using Newtonsoft.Json;

namespace MatchEngineApi.Controllers.Tools
{
    public static class ToolsExtentions
    {
        public static T GetService<T>(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            return provider.GetService<T>();
        }
        public static bool IsBase64StringAnImage(this string base64String)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using var stream = new MemoryStream(imageBytes);
                using var bitmap = SKBitmap.Decode(stream);
                return bitmap != null;
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
                return true;
                // Console.WriteLine(byteArray.Length);
                // const int size = sizeof(double);
                // return byteArray.Length % size == 0;
            }
            catch
            {
                return false;
            }
        }

      

        public static string ToBase64String(this double[] vector)
        {
            double[] values = vector;
            byte[] bytes = new byte[values.Length * sizeof(double)];
            Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);
            return Convert.ToBase64String(bytes);
        }

        public static string ToJson(this double[] vector)
        {
            return JsonConvert.SerializeObject(vector);
        }

        public static double[] JsonToDouble(this string vector)
        {
            return JsonConvert.DeserializeObject<double[]>(vector);
        }

        

        public static bool IsRootNode(this IConfigService config)
        {
            var rootNode = config.GetString("rootNode");
            return rootNode == System.Net.Dns.GetHostName();

        }
     //   public static double[] ToDouble()
    }
}