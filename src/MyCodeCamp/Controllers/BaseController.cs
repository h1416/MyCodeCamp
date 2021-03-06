﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyCodeCamp.Filter;

namespace MyCodeCamp.Controllers
{
    [ValidateModel]
    public abstract class BaseController : Controller
    {
        public const string UrlHelper = "UrlHelper";
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            context.HttpContext.Items[UrlHelper] = this.Url;
        }
    }
}
