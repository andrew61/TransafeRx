using TransafeRx.Shared.Models;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;

namespace TransafeRx.API.Controllers
{
    public class ErrorController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Post(ErrorMessage errorMessage)
        {
            {
                message.Subject = errorMessage.Subject;
                message.Body = errorMessage.Body;
                message.IsBodyHtml = true;
                await client.SendMailAsync(message);
            }
            return Ok();
        }
    }
}
