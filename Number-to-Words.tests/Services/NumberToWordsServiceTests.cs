using Moq;
using Number_to_Words.web.Services;

namespace Number_to_Words.tests.Services
{
    public class NumberToWordsServiceTests
    {
        private readonly NumberToWordsService _service;
        private readonly Mock<ICurrencySettings> _currencySettingsMock;
        private readonly Mock<INumberToWordsConverter> _numberToWordsConverterMock;
        private readonly Mock<ILanguageConfiguration> _languageConfigMock;

        public NumberToWordsServiceTests()
        {
            _currencySettingsMock = new Mock<ICurrencySettings>();
            _numberToWordsConverterMock = new Mock<INumberToWordsConverter>();
            _languageConfigMock = new Mock<ILanguageConfiguration>();

            // Default configurations
            _currencySettingsMock.SetupGet(c => c.CurrencyName).Returns("DOLLAR");
            _currencySettingsMock.SetupGet(c => c.SubunitName).Returns("CENT");
            _currencySettingsMock.SetupGet(c => c.DecimalPlaces).Returns(2);

            _languageConfigMock.SetupGet(l => l.AndWord).Returns("AND");

            _service = new NumberToWordsService(
                _currencySettingsMock.Object,
                _numberToWordsConverterMock.Object,
                _languageConfigMock.Object
            );
        }

        [Fact]
        public void ConvertNumberToWords_ShouldReturnZero_WhenInputIsZero()
        {
            // Arrange
            decimal input = 0M;
            _numberToWordsConverterMock.Setup(n => n.ConvertIntegerToWords(0)).Returns("ZERO");

            // Act
            var result = _service.ConvertNumberToWords(input);

            // Assert
            Assert.Equal("ZERO DOLLARS AND ZERO CENTS", result);
        }

        [Fact]
        public void ConvertNumberToWords_ShouldConvertDollarsAndCents()
        {
            // Arrange
            decimal input = 123.45M;
            _numberToWordsConverterMock.Setup(n => n.ConvertIntegerToWords(123)).Returns("ONE HUNDRED AND TWENTY-THREE");
            _numberToWordsConverterMock.Setup(n => n.ConvertIntegerToWords(45)).Returns("FORTY-FIVE");

            // Act
            var result = _service.ConvertNumberToWords(input);

            // Assert
            Assert.Equal("ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS", result);
        }

        [Fact]
        public void ConvertNumberToWords_ShouldHandleZeroCents()
        {
            // Arrange
            decimal input = 5.00M;
            _numberToWordsConverterMock.Setup(n => n.ConvertIntegerToWords(5)).Returns("FIVE");
            _numberToWordsConverterMock.Setup(n => n.ConvertIntegerToWords(0)).Returns("ZERO");

            // Act
            var result = _service.ConvertNumberToWords(input);

            // Assert
            Assert.Equal("FIVE DOLLARS AND ZERO CENTS", result);
        }

        [Fact]
        public void ConvertNumberToWords_ShouldHandleZeroDollars()
        {
            // Arrange
            decimal input = 0.75M;
            _numberToWordsConverterMock.Setup(n => n.ConvertIntegerToWords(0)).Returns("ZERO");
            _numberToWordsConverterMock.Setup(n => n.ConvertIntegerToWords(75)).Returns("SEVENTY-FIVE");

            // Act
            var result = _service.ConvertNumberToWords(input);

            // Assert
            Assert.Equal("ZERO DOLLARS AND SEVENTY-FIVE CENTS", result);
        }

        [Theory]
        [InlineData(1.01, "ONE", "ONE", "ONE DOLLAR AND ONE CENT")]
        [InlineData(2.02, "TWO", "TWO", "TWO DOLLARS AND TWO CENTS")]
        public void ConvertNumberToWords_ShouldHandleSingularAndPlural(decimal input, string dollarWords, string centWords, string expected)
        {
            // Arrange
            long dollars = (long)input;
            long cents = (long)((input - dollars) * 100);

            _numberToWordsConverterMock.Setup(n => n.ConvertIntegerToWords(dollars)).Returns(dollarWords);
            _numberToWordsConverterMock.Setup(n => n.ConvertIntegerToWords(cents)).Returns(centWords);

            // Act
            var result = _service.ConvertNumberToWords(input);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}