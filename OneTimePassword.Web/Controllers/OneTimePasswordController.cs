﻿using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OneTimePassword.Core;

namespace OneTimePassword.Web.Controllers {
    public class OneTimePasswordController : Controller {
        private readonly IOneTimePasswordGenerator _oneTimePasswordGenerator;

        public OneTimePasswordController(IOneTimePasswordGenerator oneTimePasswordGenerator) {
            _oneTimePasswordGenerator = oneTimePasswordGenerator;
        }

        public JsonResult Generate(string userId) {
            if (String.IsNullOrWhiteSpace(userId)) {
                throw new HttpException((int) HttpStatusCode.BadRequest, "\"User ID\" is required.");
            }
            var token = _oneTimePasswordGenerator.Generate(userId);
            return Json(token, JsonRequestBehavior.AllowGet);
        }
    }
}
