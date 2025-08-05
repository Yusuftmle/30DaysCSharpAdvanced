using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Day01_UnitTesting.sln
{
    public class XUnitAttributeExamples
    {
        // 🔹 [Fact] - Basit testler için
        [Fact]
        public void BasicTest()
        {
            // Parametresiz, basit test
            Assert.True(true);
        }

        // 🔹 [Theory] - Parametreli testler için
        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(5, 5, 10)]
        [InlineData(-1, 1, 0)]
        public void ParametricTest(int a, int b, int expected)
        {
            Assert.Equal(expected, a + b);
        }

        // 🔹 [Skip] - Testi atla
        [Fact(Skip = "Bu test henüz hazır değil")]
        public void SkippedTest()
        {
            // Bu test çalışmayacak
        }

        // 🔹 [Trait] - Test kategorilendirme
        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Priority", "High")]
        public void CategorizedTest()
        {
            // dotnet test --filter "Category=Unit" ile çalıştırabilirsin
            Assert.True(true);
        }
    }

    // ===== 2️⃣ THEORY DATA KAYNAKLARI =====

    public class TheoryDataExamples
    {
        // 🔸 [InlineData] - Direkt veri
        [Theory]
        [InlineData("hello", 5)]
        [InlineData("world", 5)]
        [InlineData("", 0)]
        public void StringLengthTest(string input, int expected)
        {
            Assert.Equal(expected, input.Length);
        }

        // 🔸 [MemberData] - Metoddan veri al
        [Theory]
        [MemberData(nameof(GetTestData))]
        public void MemberDataTest(int a, int b, int expected)
        {
            Assert.Equal(expected, a * b);
        }

        public static IEnumerable<object[]> GetTestData()
        {
            yield return new object[] { 2, 3, 6 };
            yield return new object[] { 4, 5, 20 };
            yield return new object[] { 0, 10, 0 };
        }

        // 🔸 [ClassData] - Sınıftan veri al
        [Theory]
        [ClassData(typeof(CalculatorTestData))]
        public void ClassDataTest(int a, int b, int expected)
        {
            Assert.Equal(expected, a + b);
        }
    }

    public class CalculatorTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1, 2, 3 };
            yield return new object[] { 5, 7, 12 };
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => GetEnumerator();
    }

    // ===== 3️⃣ ASSERT METODLARI =====

    public class AssertExamples
    {
        // 🟢 EŞİTLİK ASSERT'LERİ
        [Fact]
        public void EqualityAsserts()
        {
            // Temel eşitlik
            Assert.Equal(5, 2 + 3);
            Assert.NotEqual(4, 2 + 3);

            // String eşitlik (büyük/küçük harf duyarlı)
            Assert.Equal("Hello", "Hello");
            Assert.NotEqual("Hello", "hello");

            // String eşitlik (büyük/küçük harf duyarsız)
            Assert.Equal("Hello", "HELLO", StringComparer.OrdinalIgnoreCase);

            // Ondalık sayılar için hassasiyet
            Assert.Equal(3.14159, 3.14, precision: 2);

            // Object referans eşitliği
            string str1 = "test";
            string str2 = str1;
            Assert.Same(str1, str2); // Aynı referans
            Assert.NotSame("test", "test".ToUpper().ToLower()); // Farklı referans
        }

        // 🟡 BOOLEAN ASSERT'LERİ
        [Fact]
        public void BooleanAsserts()
        {
            Assert.True(5 > 3);
            Assert.False(2 > 5);
        }

        // 🔴 NULL ASSERT'LERİ
        [Fact]
        public void NullAsserts()
        {
            string nullString =  null;
            string notNullString = "test";

            Assert.Null(nullString);
            Assert.NotNull(notNullString);
        }

        // 🟠 TİP ASSERT'LERİ
        [Fact]
        public void TypeAsserts()
        {
            object obj = "Bu bir string";

            // Tip kontrolü
            Assert.IsType<string>(obj);
            Assert.IsNotType<int>(obj);

            // Alt sınıf kontrolü
            Assert.IsAssignableFrom<object>(obj);

            // As ile cast
            string castedString = Assert.IsType<string>(obj);
            Assert.Equal("Bu bir string", castedString);
        }

        // 🔵 RANGE ASSERT'LERİ
        [Fact]
        public void RangeAsserts()
        {
            int number = 15;

            // Aralık kontrolü
            Assert.InRange(number, 10, 20); // 10 <= number <= 20
            Assert.NotInRange(number, 1, 5);  // number < 1 veya number > 5
        }

        // 🟣 COLLECTİON ASSERT'LERİ
        [Fact]
        public void CollectionAsserts()
        {
            var numbers = new List<int> { 1, 2, 3, 4, 5 };
            var emptyList = new List<int>();

            // Boş koleksiyon
            Assert.Empty(emptyList);
            Assert.NotEmpty(numbers);

            // Eleman sayısı
            Assert.Single(new[] { "tek eleman" }); // Sadece 1 eleman

            // Eleman içeriyor mu
            Assert.Contains(3, numbers);
            Assert.DoesNotContain(10, numbers);

            // String içinde substring
            Assert.Contains("ell", "Hello World");
            Assert.DoesNotContain("xyz", "Hello World");

            // Koleksiyon eşitliği
            Assert.Equal(new[] { 1, 2, 3 }, new[] { 1, 2, 3 });

            // Sıralı eşitlik (sequence)
            Assert.Equal(numbers, new[] { 1, 2, 3, 4, 5 });
        }

        // 🔶 EXCEPTION ASSERT'LERİ
        [Fact]

        public async Task ExceptionAsserts()
        {
            // Exception fırlatır mı
            Assert.Throws<DivideByZeroException>(() => {
                int x = 5;
                int y = 0;
                int result = x / y;
            });

            // async lambda ve await doğru şekilde kullanılmalı
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Task.Delay(10); // örnek async iş
                throw new ArgumentException("Geçersiz parametre");
            });

            Assert.Equal("Geçersiz parametre", exception.Message);

            // Alt sınıf exception
            await Assert.ThrowsAsync<Exception>(() => {
                throw new InvalidOperationException();
            });

            // Exception fırlatmaz
            var result = Record.Exception(() => {
                int safe = 5 + 3;
            });
            Assert.Null(result); // Exception yok
        }

        // 🔸 STRING ASSERT'LERİ
        [Fact]
        public void StringAsserts()
        {
            string text = "Hello World Programming";

            // Başlar/Biter
            Assert.StartsWith("Hello", text);
            Assert.EndsWith("Programming", text);

            // RegEx match
            Assert.Matches(@"\d+", "Test123"); // Rakam içeriyor mu
            Assert.DoesNotMatch(@"\d+", "NoNumbers"); // Rakam içermiyor

            // Boş string
            Assert.Empty("");
            Assert.NotEmpty("not empty");
        }

        // 🔺 CUSTOM ASSERT'LER
        [Fact]
        public void CustomAsserts()
        {
            var person = new Person { Name = "Ahmet", Age = 25 };

            // Property kontrolü
            Assert.Equal("Ahmet", person.Name);
            Assert.True(person.Age > 18);

            // Koşul kontrolü
            Assert.True(person.Age >= 18 && person.Age <= 65,
                       "Yaş çalışma yaşı aralığında olmalı");

            // All - Tüm elemanlar koşulu sağlar mı
            var ages = new[] { 20, 25, 30, 35 };
            Assert.All(ages, age => Assert.True(age >= 18));
        }
    }

    // Test için örnek sınıf
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    // ===== 4️⃣ TEST YAŞAMı (LIFECYCLE) =====

    public class TestLifecycleExample : IDisposable
    {
        private readonly Calculator _calculator;

        // Her test öncesi çalışır
        public TestLifecycleExample()
        {
            _calculator = new Calculator();
            // Setup kodu buraya
        }

        // Her test sonrası çalışır
        public void Dispose()
        {
            // Cleanup kodu buraya
            // Dosyaları kapat, bağlantıları temizle vs.
        }

        [Fact]
        public void Test1()
        {
            // Test kodu
            Assert.NotNull(_calculator);
        }

        [Fact]
        public void Test2()
        {
            // Her test için yeni instance oluşur
            Assert.NotNull(_calculator);
        }
    }

    // ===== 5️⃣ PAYLAŞILAN CONTEXT (FIXTURE) =====

    // Tüm testlerde aynı instance kullanmak için
    public class DatabaseFixture : IDisposable
    {
        public string ConnectionString { get; private set; }

        public DatabaseFixture()
        {
            // Veritabanı bağlantısı kur
            ConnectionString = "test connection";
        }

        public void Dispose()
        {
            // Veritabanı bağlantısını kapat
        }
    }

    public class DatabaseTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public DatabaseTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ConnectionTest()
        {
            Assert.NotNull(_fixture.ConnectionString);
        }
    }

    // ===== 6️⃣ PARALEL TEST ÇALIŞTIRMA =====

    // Paralel çalıştırma kapatma
    [Collection("Sequential")]
    public class SequentialTests
    {
        [Fact]
        public void Test1() { }

        [Fact]
        public void Test2() { }
    }

    // Collection tanımı
    [CollectionDefinition("Sequential", DisableParallelization = true)]
    public class SequentialCollection { }

    // ===== 7️⃣ KOMUT SATIRI FİLTRELEME ÖRNEKLERİ =====

    /*
    🖥️ KOMUT SATIRI KULLANIM ÖRNEKLERİ:

    # Tüm testleri çalıştır
    dotnet test

    # Sadece belirli kategorideki testleri çalıştır
    dotnet test --filter "Category=Unit"

    # Sadece belirli trait'li testleri çalıştır
    dotnet test --filter "Priority=High"

    # Test adına göre filtrele
    dotnet test --filter "FullyQualifiedName~Calculator"

    # Birden fazla koşul
    dotnet test --filter "Category=Unit&Priority=High"

    # Verbose çıktı
    dotnet test --verbosity detailed

    # Sadece başarısız testleri göster
    dotnet test --logger "console;verbosity=quiet"

    # Test sonuçlarını dosyaya kaydet
    dotnet test --logger "trx;LogFileName=test-results.trx"
    */

    // ===== 8️⃣ BEST PRACTICES (EN İYİ UYGULAMALAR) =====

    public class BestPracticesExamples
    {
        /*
        ✅ İYİ UYGULAMALAR:

        1. TEST ADLANDIRMA:
           - MethodName_Scenario_ExpectedResult
           - Örnek: Calculator_DivideByZero_ThrowsException

        2. AAA PATTERN:
           - Arrange: Test verilerini hazırla
           - Act: Test edilecek kodu çalıştır
           - Assert: Sonucu kontrol et

        3. BİR TEST = BİR DOĞRULAMA:
           - Her test sadece bir şeyi test etmeli
           - Birden fazla Assert kullanma

        4. TEST BAGIMSIZLIĞI:
           - Testler birbirinden bağımsız olmalı
           - Test sırası önemli olmamalı

        5. ANLAMLI ASSERT MESAJLARI:
           - Assert.True(result, "Hesaplama sonucu pozitif olmalı");
        */

        [Fact]
        public void Calculator_Add_PositiveNumbers_ReturnsSum()
        {
            // Arrange
            var calculator = new Calculator();
            double a = 5;
            double b = 3;
            double expected = 8;

            // Act
            double actual = calculator.Toplama(a, b);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 5, 5)]
        [InlineData(-3, 3, 0)]
        [InlineData(1.5, 2.5, 4.0)]
        public void Calculator_Add_VariousInputs_ReturnsCorrectSum(
            double a, double b, double expected)
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            double actual = calculator.Toplama(a, b);

            // Assert
            Assert.Equal(expected, actual, 2); // 2 ondalık hassasiyet
        }
    }

    // Calculator sınıfı (referans için)
    public class Calculator
    {
        public double Toplama(double a, double b) => a + b;
        public double Carpma(double a, double b) => a * b;
        public string Bolme(double a, double b)
        {
            if (b == 0) return "Hata: Sıfıra bölme yapılamaz!";
            return (a / b).ToString();
        }
    }

}
