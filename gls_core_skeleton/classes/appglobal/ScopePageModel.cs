using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace appglobal
{
  /// <summary>
  /// Class to override PageModel and modify its behaviour
  /// </summary>
  public class ScopePageModel : PageModel
  {

    /// <summary>
    /// Main logic to filter user access and override all ScopePageModel derived behaviour
    /// </summary>
    /// <param name="context"></param>
    public override void OnPageHandlerSelected(PageHandlerSelectedContext context)
    {
      //Check user validity & feature allowed to be accessed
      if (AppGlobal.get_user_validity() == false)
      {
        Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        Response.Redirect("/main/forbidden"); //short circuit & send forbidden page
      }

      //var context_temp = context.HttpContext.Request.Query;
      //string content = JsonConvert.SerializeObject(context_temp, Formatting.Indented,
      //new JsonSerializerSettings {
      //  ReferenceLoopHandling = ReferenceLoopHandling.Serialize
      //});
      //AppGlobal.console_log("content of context", content);

      //Created short circuit based on page request query, if f is null rejected
      if (string.IsNullOrEmpty(context.HttpContext.Request.Query["f"]))
      {
        Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        Response.Redirect("/main/forbidden"); //short circuit & send forbidden page
      }
      else
      {
        //RFC : 
        //1. add logging here, to save : feature accessed, params passed, etc.
        //2. add more thorough f filtering (if needed)
      }
    }
  }
}
