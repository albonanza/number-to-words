using Moq;
using Number_to_Words.web.Services;

namespace Number_to_Words.tests.Services
{
    public class EnglishNumberToWordsConverterTests
    {
        private readonly EnglishNumberToWordsConverter _converter;
        private readonly Mock<ILanguageConfiguration> _languageConfigMock;

        public EnglishNumberToWordsConverterTests()
        {
            _languageConfigMock = new Mock<ILanguageConfiguration>();

            // language configurations
            _languageConfigMock.SetupGet(l => l.UnitsMap).Returns(new Dictionary<int, string>
            {
                {0, "ZERO"},
                {1, "ONE"},
                {2, "TWO"},
                {3, "THREE"},
                {4, "FOUR"},
                {5, "FIVE"},
                {6, "SIX"},
                {7, "SEVEN"},
                {8, "EIGHT"},
                {9, "NINE"},
                {10, "TEN"},
                {11, "ELEVEN"},
                {12, "TWELVE"},
                {13, "THIRTEEN"},
                {14, "FOURTEEN"},
                {15, "FIFTEEN"},
                {16, "SIXTEEN"},
                {17, "SEVENTEEN"},
                {18, "EIGHTEEN"},
                {19, "NINETEEN"}
            });

            _languageConfigMock.SetupGet(l => l.TensMap).Returns(new Dictionary<int, string>
            {
                {20, "TWENTY"},
                {30, "THIRTY"},
                {40, "FORTY"},
                {50, "FIFTY"},
                {60, "SIXTY"},
                {70, "SEVENTY"},
                {80, "EIGHTY"},
                {90, "NINETY"}
            });

            _languageConfigMock.SetupGet(l => l.ScaleMap).Returns(new Dictionary<long, string>
            {
                {1000000000, "BILLION"},
                {1000000, "MILLION"},
                {1000, "THOUSAND"},
                {100, "HUNDRED"}
            });

            _languageConfigMock.SetupGet(l => l.AndWord).Returns("AND");

            _converter = new EnglishNumberToWordsConverter(_languageConfigMock.Object);
        }

        [Theory]
        [InlineData(0, "ZERO")]
        [InlineData(1, "ONE")]
        [InlineData(15, "FIFTEEN")]
        [InlineData(23, "TWENTY-THREE")]
        [InlineData(85, "EIGHTY-FIVE")]
        [InlineData(100, "ONE HUNDRED AND ZERO")]
        [InlineData(123, "ONE HUNDRED AND TWENTY-THREE")]
        [InlineData(1000, "ONE THOUSAND AND ZERO")]
        [InlineData(1234, "ONE THOUSAND TWO HUNDRED AND THIRTY-FOUR")]
        [InlineData(1000000, "ONE MILLION AND ZERO")]
        [InlineData(1000000000, "ONE BILLION AND ZERO")]

        public void ConvertIntegerToWords_ShouldReturnCorrectWords(long input, string expected)
        {
            // Act
            var result = _converter.ConvertIntegerToWords(input);

            // Assert
            Assert.Equal(expected, result);
        }

    }
}