using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using appglobal.models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace appglobal
{
  public class loginModel : PageModel
  {
    gls_model _context = new gls_model(AppGlobal.get_db_option()); //simplifying context initializer by override

    public void OnGet()
    {
    }

    /// <summary>
    /// Handle onPost Login AJAX based dengan return value JsonResult
    /// </summary>
    /// <param name="request_parameter"></param>
    /// <param name="returnURL"></param>
    /// <returns></returns>
    public JsonResult OnPost(string request_parameter, string returnURL = null)
    {
      dynamic login_object = JsonConvert.DeserializeObject(request_parameter);
      string user_name = login_object["username"];
      string password = login_object["password"];
      //Console.WriteLine("user_name>> " + user_name);
      //Console.WriteLine("password >> " + password);
      AppResponseMessage arm = new AppResponseMessage();

      if (!string.IsNullOrWhiteSpace(user_name) && !string.IsNullOrWhiteSpace(password))
      {
        //jika masukan username & password valid
        if (IsValidLogin(user_name, password))
        {
          //jika username & password dikenali
          string user_id = _context.m_user.Where(f => f.user_name == user_name).FirstOrDefault().m_user_id + "";
          string Role = _context.m_user.Include(f => f.m_user_group).Where(f => f.user_name == user_name).FirstOrDefault().m_user_group.user_group_name;
          string user_category_id = _context.m_user.Where(f => f.user_name == user_name).FirstOrDefault().m_user_group_id + "";

          bool status_aktif = _context.m_user.Where(f => f.user_name == user_name).FirstOrDefault().user_active;
          if (status_aktif != true)
          {
            //jika user tidak aktif
            arm.fail();
            arm.message = "user tidak aktif";
          }
          else
          {
            //jika user valid & aktif
            var claims = new[] {
              new Claim(ClaimTypes.Name, user_name),
              new Claim(ClaimTypes.Role, Role),

              new Claim("user_id", user_id),
              new Claim("user_category_id", user_category_id),
              new Claim("user_name", user_name),
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            AuthenticationHttpContextExtensions.SignInAsync(HttpContext, CookieAuthenticationDefaults.AuthenticationScheme, principal);
            arm.success();
            arm.message = "login berhasil";
          }
        }
        else
        {
          arm.fail();
          arm.message = "login gagal";
        }
      }
      else
      {
        arm.fail();
        arm.message = "login gagal";
      }
      return new JsonResult(arm);
    }

    /// <summary>
    /// Handle pencocokan identity login di database
    /// </summary>
    /// <param name="user_name"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    internal bool IsValidLogin(string user_name, string password)
    {
      bool hasil = false;
      try
      {
        string passInDB = _context.m_user.Where(u => u.user_name == user_name).FirstOrDefault().user_password;
        hasil = PasswordStorage.VerifyPassword(password, passInDB) ? true : false;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return hasil;
    }

    /// <summary>
    /// Get all feature user has access to
    /// </summary>
    /// <param name="m_user_group_id"></param>
    /// <returns></returns>
    private string getFeatureMap(int m_user_group_id)
    {
      string result = "";
      List<feature_map> feature_map_list;
      feature_map_list = _context.feature_map.Where(a => a.m_user_group_id == m_user_group_id).ToList();
      result = Newtonsoft.Json.JsonConvert.SerializeObject(feature_map_list);
      Console.WriteLine(result);
      return result;
    }
  }
}