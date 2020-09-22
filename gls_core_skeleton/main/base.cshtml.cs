using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using appglobal.models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Web;

namespace appglobal
{
  public class baseModel : PageModel
  {
    static gls_model _context = new gls_model(AppGlobal.get_db_option()); //simplifying context initializer by override

    public void OnGet()
    {
    }

    /// <summary>
    /// Create primary menu structure based on user authorization
    /// </summary>
    /// <returns></returns>
    public static string create_menu_structure()
    {

      //rfc : edit model & add hidden attribute
      //don't display hidden feature, but grant access to user when mapped
      string string_menu = "";

      Claim user_group_id = System.Web.HttpContext.Current.User.FindFirst("user_category_id");
      int m_user_group_id = Convert.ToInt32(user_group_id.Value);
      var authorized_feature_list = _context.feature_map
        .Where(e => e.m_user_group_id == m_user_group_id)
        .Select(e => e.m_feature_id)
        .ToList();
      var menu_list = _context.m_feature
        .Include(e => e.m_feature_group)
        .Where(e => authorized_feature_list.Contains(e.m_feature_id))
        .Select(e => new { e.m_feature_id, e.feature_name, e.feature_icon, e.feature_url, e.m_feature_group_id, e.m_feature_group.feature_group_name, e.feature_sequence })
        .OrderBy(e => e.m_feature_group_id.ToString("D2") + "-" + e.feature_sequence.ToString("D2"))
        .ToList();

      //AppGlobal.console_log("menu list", JsonConvert.SerializeObject(menu_list));

      if (menu_list.Count() > 0)
      {
        string header = "";
        int i = 0;
        foreach (var menu_list_data in menu_list)
        {
          string printed_header = ""; //make header blank for same feature group
          if (header != menu_list_data.feature_group_name)
          {
            header = menu_list_data.feature_group_name; //save new header for new feature group
            printed_header = i > 0 ? "<hr/>" + header : header; //add line separator for 2nd header below
          }
          printed_header = printed_header != "" ? @"<h4 style='text-align: center'>" + printed_header + "</h4>" : "";

          string feature_group_url = menu_list_data.feature_group_name.ToLower().Replace(" ", "_");
          string feature_name = menu_list_data.feature_name;
          string feature_icon = menu_list_data.feature_icon;
          string feature_url = menu_list_data.feature_url;

          string_menu = string_menu + printed_header +
            @"<li class='nav-item' data-feature_group_url='" + feature_group_url + @"' data-feature_url='" + feature_url + @"'>
              <a href='#'><i class='" + feature_icon + " fa-fw'></i> <span class='menu-title'>" + feature_name + @"</span></a>
            </li>";
          i++;
        }
      }

      return string_menu;
    }
  }
}