using System.ComponentModel.DataAnnotations;

namespace Number_to_Words.web.Models
{
    public class NumberToWordsViewModel
    {
        [Required(ErrorMessage = "Please enter a number.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a valid number up to two decimal places.")]
        [Range(typeof(decimal), "0", "9999999999", ErrorMessage = "Please enter a number between 0 and 9,999,999,999.")]
        public string NumberInput { get; set; }

        public string Result { get; set; }
    }
}