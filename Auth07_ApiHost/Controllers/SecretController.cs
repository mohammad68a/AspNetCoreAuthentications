using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth07_ApiHost.Controllers
{
    public class SecretController : Controller
    {
        [Authorize]
        public string Index()
        {
            return "Secret message";
        }
    }
}
