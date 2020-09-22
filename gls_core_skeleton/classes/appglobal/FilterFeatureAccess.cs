using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using appglobal;
using System.Security.Claims;
using appglobal.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace appglobal
{
    public class FilterFeatureAccess : ActionFilterAttribute
    { //final m_feature user access checking

        public override void OnActionExecuting(ActionExecutingContext filter_context)
        {
            try
            {
                var User = filter_context.HttpContext.User;
                Claim features = User.FindFirst("features");
                List<feature_map> feature_map_list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<feature_map>>(features.Value);
                int[] feature_array = feature_map_list.Select(e => e.m_feature_id).ToArray();

                string feature_url = filter_context.ActionDescriptor.RouteValues["controller"];
                string action = filter_context.ActionDescriptor.RouteValues["action"];

                bool has_access = AppGlobal.check_access_feature(feature_url, feature_array); //false;
                if (!has_access)
                {
                    filter_context.Result = new UnauthorizedResult();
                }
            }
            catch (Exception e)
            {
                filter_context.Result = new UnauthorizedResult();
            }
        }
    }

    public class LogFeatureAccess : ActionFilterAttribute
    { //trying to do automatic log to standardize logging

        public override void OnActionExecuting(ActionExecutingContext filter_context)
        {
            try
            {
                var User = filter_context.HttpContext.User;

                //get control and action that filtered
                string feature_url = filter_context.ActionDescriptor.RouteValues["controller"];
                string action = filter_context.ActionDescriptor.RouteValues["action"];

                //get POST form data
                var form_data = filter_context.HttpContext.Request.Form.ToList();

                //remove unused/critical key
                form_data.Remove(form_data.First(item => item.Key.Equals("__RequestVerificationToken")));

                //serialize to JSON format
                string serialized_form_data = Newtonsoft.Json.JsonConvert.SerializeObject(form_data);

                //write to console
                //Console.WriteLine(serialized_form_data);

                AppGlobal.writeActivityLog(filter_context.HttpContext, User.Identity.Name, AppGlobal.get_feature_id(feature_url) + "", action, serialized_form_data);
            }
            catch (Exception e)
            {
                filter_context.Result = new UnauthorizedResult();
            }
        }
    }
}