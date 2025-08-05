using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day01_UnitTesting.sln.MyAPP
{
    public class MyApp
    {
        public class Calculator
        {
            /// <summary>
            /// Üç temel fonksiyona sahip hesap makinesi sınıfı
            /// </summary>

            /// <summary>
            /// İki sayıyı toplar
            /// </summary>
            public double Toplama(double a, double b)
            {
                return a + b;
            }

            /// <summary>
            /// İki sayıyı çarpar
            /// </summary>
            public double Carpma(double a, double b)
            {
                return a * b;
            }

            /// <summary>
            /// İki sayıyı böler (sıfıra bölme kontrolü ile)
            /// </summary>
            public string Bolme(double a, double b)
            {
                if (b == 0)
                {
                    return "Hata: Sıfıra bölme yapılamaz!";
                }
                return (a / b).ToString();
            }
        }
        public class SlugGenerator()
        {
            public async Task< string> Generate(string input)
            {
                return await Task.Run(() =>
                {
                    if (string.IsNullOrWhiteSpace(input))
                        return string.Empty;

                    input = input.ToLowerInvariant().Trim();
                    input = ReplaceTurkishChars(input);
                    input = Regex.Replace(input, @"[^a-z0-9\s-]", "");
                    input = Regex.Replace(input, @"\s+", "-");
                    input = Regex.Replace(input, @"-+", "-");

                    return input.Trim('-');
                });
            }

            private string ReplaceTurkishChars(string text)
            {
                return text
                    .Replace("ç", "c")
                    .Replace("ğ", "g")
                    .Replace("ı", "i")
                    .Replace("ö", "o")
                    .Replace("ş", "s")
                    .Replace("ü", "u");
            }
        }


    }


}
