using ChatApplication.Models.Chat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace ChatApplication.Controllers
{
    public class ChatController : Controller
    {
        
        public IActionResult ChatLogin()
        {
            return View("~/Views/Chat/ChatLoginPage.cshtml");
        }
        public IActionResult ChatHomepage()
        {
            try
            {
                var formObject = Request.Form;
                const string usernameFormKey = "Username";
                const string countryFormKey = "Country";
                const string ageFormKey = "Age";

                return View("~/Views/Chat/ChatHomePage.cshtml", new ChatLoginModel
                {
                    Username = String.IsNullOrEmpty(formObject[usernameFormKey]) ? StringValues.Empty : formObject[usernameFormKey],
                    Country = String.IsNullOrEmpty(formObject[countryFormKey]) ? StringValues.Empty : formObject[countryFormKey],
                    Age = String.IsNullOrEmpty(formObject[ageFormKey]) ? Byte.MinValue : Convert.ToByte(formObject[ageFormKey]),
                });
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction(nameof(this.ChatLogin));
            }
        }
    }
}
