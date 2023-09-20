using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace ChatApplication.Controllers
{
    public class ChatController : Controller
    {
        public class Class1
        {
            public string? Username { get; set; } = String.Empty;
            public string? City { get; set; } = String.Empty;
            public byte? Age { get; set; } = 0;
        }
        public IActionResult ChatLogin()
        {
            return View("~/Views/Chat/ChatLoginPage.cshtml");
        }
        public IActionResult ChatHomepage()
        {
            try
            {
                var formObject = Request.Form;
                var x1 = String.IsNullOrEmpty(formObject["Username"]) ? StringValues.Empty : formObject["Username"];
                var x2 = String.IsNullOrEmpty(formObject["City"]) ? StringValues.Empty : formObject["City"];
                var x3 = String.IsNullOrEmpty(formObject["Age"]) ? Byte.MinValue : Convert.ToByte(formObject["Age"]);
                var formValues = new Class1()
                {
                    Username = String.IsNullOrEmpty(formObject["Username"]) ? StringValues.Empty : formObject["Username"],
                    City = String.IsNullOrEmpty(formObject["City"]) ? StringValues.Empty : formObject["City"],
                    Age = String.IsNullOrEmpty(formObject["Age"]) ? Byte.MinValue : Convert.ToByte(formObject["Age"]),
                };
                return View("~/Views/Chat/ChatHomePage.cshtml", new
                {
                    Username = formValues.Username,
                    City = formValues.City,
                    Age = formValues.Age,
                });
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction(nameof(this.ChatLogin));
            }
        }
    }
}
