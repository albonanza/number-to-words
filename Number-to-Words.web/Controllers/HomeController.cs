using Microsoft.AspNetCore.Mvc;
using Number_to_Words.web.Models;
using Number_to_Words.web.Services;

namespace Number_to_Words.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly INumberToWordsService _numberToWordsService;

        public HomeController(INumberToWordsService numberToWordsService)
        {
            _numberToWordsService = numberToWordsService;
        }

        // GET: /Home/Index
        [HttpGet]
        public IActionResult Index()
        {
            var model = new NumberToWordsViewModel();
            return View(model);
        }

        // POST: /Home/Index
        [HttpPost]
        public IActionResult Index(NumberToWordsViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (decimal.TryParse(model.NumberInput, out decimal number))
                {
                    if (number < 0)
                    {
                        ModelState.AddModelError(nameof(model.NumberInput), "Negative numbers are not allowed. Please enter a number between 0 and 9,999,999,999.");
                    }
                    else if (number > 9999999999M)
                    {
                        ModelState.AddModelError(nameof(model.NumberInput), "Number is too large. Please enter a number between 0 and 9,999,999,999.");
                    }
                    else
                    {
                        model.Result = _numberToWordsService.ConvertNumberToWords(number);
                        return Json(new { success = true, result = model.Result });
                    }
                }
                else
                {
                    ModelState.AddModelError(nameof(model.NumberInput), "Please enter a valid number.");
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();

            return Json(new { success = false, errors });
        }
    }
}