using Microsoft.AspNetCore.Mvc;
using TransportTicketApp.API.Services;

namespace TransportTicketApp.API.Controllers
{
    [Route("api/chatbot")]
    [ApiController]
    public class ChatbotController : Controller
    {
        private readonly ILuisService _luisService;

        public ChatbotController(ILuisService luisService)
        {
            _luisService = luisService;
        }

        [HttpPost("query")]
        public async Task<IActionResult> Query([FromBody] ChatbotQuery query)
        {
            var prediction = await _luisService.PredictAsync(query.Text);

            var topIntent = prediction.Prediction.TopIntent;
            var entities = prediction.Prediction.Entities;

            // Intent ve entity'lere göre işlemleri burada yapabilirsiniz.
            // Örnek olarak, intent'e göre basit bir switch-case yapısı:
            switch (topIntent)
            {
                case "GetTicketStatus":
                    // Bilet durumunu kontrol et ve yanıt ver
                    return View(new { message = "Checking ticket status..." });
                case "CancelTicket":
                    // Bilet iptal işlemini gerçekleştir ve yanıt ver
                    return View(new { message = "Cancelling ticket..." });
                case "BookTicket":
                    // Bilet rezervasyonu yap ve yanıt ver
                    return View(new { message = "Booking ticket..." });
                default:
                    return View(new { message = "Unknown intent" });
            }
        }
    }

    public class ChatbotQuery
    {
        public string Text { get; set; }
    }

}
