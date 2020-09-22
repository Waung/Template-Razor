using System;
using System.Web;
using System.IO;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Summary description for Access_Handler
/// </summary>
public class access_handler : PageModel
{
  public access_handler() {
  }

  public static void access_check(IHttpContextAccessor http) {
    if (my_session.Current.user_id == null && access_location(http) != "login") {
      http.HttpContext.Response.Redirect("login");
    }
    //trapping logged access accidentally hit the login page
    else if (my_session.Current.user_id != null) {
      if (my_session.Current.display_type == "admin" && access_location(http) != "") {
        http.HttpContext.Response.Redirect("");
      }
    }
  }

  public static string access_location(IHttpContextAccessor http) {
    return Path.GetFileName(http.HttpContext.Request.Path);
  }

  //public static int get_access_dt_count_admin() {
  //  int count = 0;
  //  DataTable dt = get_access_dt_admin();
  //  count = dt.Rows.Count;
  //  return count;
  //}

  //public static int get_access_dt_count_guru() {
  //  int count = 0;
  //  DataTable dt = get_access_dt_guru();
  //  count = dt.Rows.Count;
  //  return count;
  //}

  //public static DataTable get_access_dt_admin() {
  //  DataTable dt = new DataTable();
  //  DataHandler dh = new DataHandler();
  //  string query = @"
  //  SELECT MstFeaturesGroup.FeaturesGroupName, MstFeatures.FeatureName, Icon, URL
  //  FROM MstFeatures, MstFeaturesGroup, MstGroupUserPriviledge 
  //  WHERE MstFeatures.FeaturesGroupID=MstFeaturesGroup.FeaturesGroupID 
  //  AND MstGroupUserPriviledge.FeatureID=MstFeatures.FeatureID
  //  AND MstGroupUserPriviledge.GroupUserID='" + my_session.Current.user_category_id + @"'
  //  ORDER BY MstFeaturesGroup.FeaturesGroupOrder, MstFeatures.FeatureID";
  //  dt = dh.getDataTable(query, 4);
  //  return dt;
  //}
  //public static DataTable get_access_dt_guru() {
  //  DataTable dt = new DataTable();
  //  DataHandler dh = new DataHandler();
  //  string query = @"
  //  SELECT MstFeaturesGroup.FeaturesGroupName, MstFeatures.FeatureName, Icon, URL
  //  FROM MstFeatures, MstFeaturesGroup, MstGroupUserPriviledge 
  //  WHERE MstFeatures.FeaturesGroupID=MstFeaturesGroup.FeaturesGroupID 
  //  AND MstGroupUserPriviledge.FeatureID=MstFeatures.FeatureID
  //  AND MstFeatures.FeaturesGroupID>=6
  //  AND MstGroupUserPriviledge.GroupUserID='" + my_session.Current.user_category_id + @"'
  //  ORDER BY MstFeatures.FeaturesGroupID, MstFeatures.FeatureID";
  //  dt = dh.getDataTable(query, 4);
  //  return dt;
  //}

}