using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OneTimePassword.Core;
using OneTimePassword.Resources;

namespace OneTimePassword.Web.Controllers {
    public class OneTimePasswordController : Controller {
        private readonly IOneTimePasswordGenerator _oneTimePasswordGenerator;

        public OneTimePasswordController(IOneTimePasswordGenerator oneTimePasswordGenerator) {
            _oneTimePasswordGenerator = oneTimePasswordGenerator;
        }

        public JsonResult Generate(string userId) {
            if (String.IsNullOrWhiteSpace(userId)) {
                throw new HttpException((int) HttpStatusCode.BadRequest, Messages.UserIdIsRequired);
            }
            var token = _oneTimePasswordGenerator.Generate(userId);
            return Json(token, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Validate(string userId, string token) {
            if (String.IsNullOrWhiteSpace(userId)) {
                throw new HttpException((int) HttpStatusCode.BadRequest, Messages.UserIdIsRequired);
            }
            if (String.IsNullOrWhiteSpace(token)) {
                throw new HttpException((int) HttpStatusCode.BadRequest, Messages.TokenIsRequired);
            }
            var checkToken = _oneTimePasswordGenerator.Generate(userId);
            var result = token.Equals(checkToken);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
