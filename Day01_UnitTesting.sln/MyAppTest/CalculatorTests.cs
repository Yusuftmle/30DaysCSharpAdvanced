using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Day01_UnitTesting.sln.MyAPP.MyApp;

namespace Day01_UnitTesting.sln.MyAppTest
{
    public class CalculatorTests
    {
        private readonly Calculator _calculator;

        public CalculatorTests()
        {
            _calculator = new Calculator(); // Initialize the Calculator instance here.  
        }

        [Fact]
        public void Toplama()
        {
            double a = 15;
            double b = 10;
            Assert.Equal(25, _calculator.Toplama(a, b));
        }
        [Fact]
        public void Carpma()
        {
            double a = 15;
            double b = 10;
            Assert.Equal(150, _calculator.Carpma(a, b));
        }
    }
}
// ===== XUNIT GENEL KURALLARI VE İPUÇLARI =====

/*
1. TEST ADLANDIRMA KURALLARI:
   - MethodName_Scenario_ExpectedBehavior formatı kullan
   - Örnek: Toplama_PositiveNumbers_ReturnsCorrectSum

2. AAA PATTERN (Arrange-Act-Assert):
   - Arrange: Test verilerini hazırla
   - Act: Test edilecek metodu çalıştır
   - Assert: Sonucu doğrula

3. ATTRIBUTE'LAR:
   - [Fact]: Parametresiz basit testler
   - [Theory]: Parametreli testler
   - [InlineData]: Theory testleri için veri
   - [Skip("reason")]: Testi atla
   - [Trait("Category", "Unit")]: Test kategorilendirme

4. ASSERT METODLARI:
   - Assert.Equal(expected, actual): Eşitlik
   - Assert.True(condition): Koşul doğru mu
   - Assert.False(condition): Koşul yanlış mı
   - Assert.Null(object): Null mu
   - Assert.NotNull(object): Null değil mi
   - Assert.Throws<Exception>(() => method()): Exception fırlatır mı
   - Assert.InRange(actual, low, high): Aralıkta mı

5. TEST ORGANIZASYONU:
   - Her sınıf için ayrı test sınıfı
   - Test sınıfı adı: ClassName + Tests
   - Constructor kullanarak setup yap
   - IDisposable implement ederek cleanup yap

6. İYİ TESTLERIN ÖZELLİKLERİ:
   - Fast (Hızlı)
   - Independent (Bağımsız)
   - Repeatable (Tekrarlanabilir)
   - Self-Validating (Kendi kendini doğrulayan)
   - Timely (Zamanında yazılmış)

7. KOMUT SATIRI KOMUTLARI:
   - dotnet test: Tüm testleri çalıştır
   - dotnet test --filter "FullyQualifiedName~Calculator": Sadece Calculator testleri
   - dotnet test --logger trx: Test sonuçlarını TRX formatında kaydet

8. ÖĞRENME KAYNAKLARI:
   - XUnit resmi dokümantasyonu: https://xunit.net/
   - Microsoft Unit Testing dokümantasyonu
   - Test-Driven Development (TDD) konularını araştır
*/