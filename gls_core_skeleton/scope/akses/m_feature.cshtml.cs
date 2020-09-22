using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using appglobal.models;
using Microsoft.EntityFrameworkCore;

namespace appglobal
{
  public class m_featureModel : ScopePageModel
  {

    /// <summary>
    /// Post method bisa ditulis tanpa parameter catch
    /// parameter dibaca manual lewat Request.Query untuk QUERY STRING
    /// dan Request.Form untuk FORM BODY POST, termasuk file
    /// </summary>
    /// <returns></returns>
    public JsonResult OnPost()
    {
      gls_model _context = new gls_model(AppGlobal.get_db_option()); //simplifying context initializer by override
      AppResponseMessage arm = new AppResponseMessage(); //inisialisasi ARM sebagai standarisasi respon balik

      //handle kiriman parameter sesuai f >> function, dihandle filternya di ScopePageModel
      if (Request.Query["f"] == "insert_handler")
      {
        string feature_name = Request.Form["feature_name"];
        int count_existed = _context.m_feature.Where(e => e.feature_name == feature_name).Count(); //gunakan e >> entity untuk select ef
        if (count_existed > 0)
        { //cek data duplikat
          arm.fail();
          arm.message = "Data sudah ada (duplikat)!";
        }
        else
        {
          //construct object m_feature
          m_feature m_feature_data = new m_feature {
            m_feature_group_id = Convert.ToInt32(Request.Form["m_feature_group_id"]),
            feature_name = feature_name,
            feature_sequence = Convert.ToInt32(Request.Form["feature_sequence"]),
            feature_url = Request.Form["feature_url"],
            feature_icon = Request.Form["feature_icon"],
            feature_private = Convert.ToBoolean(Request.Form["feature_private"]),
          };
          _context.m_feature.Add(m_feature_data); //insert m_feature yg diconstruct
          arm.success(); //set success status
          arm.message = "Data berhasil ditambahkan"; //set success message
        }
      }
      else if (Request.Query["f"] == "edit_handler")
      {
        int m_feature_id = Convert.ToInt32(Request.Form["m_feature_id"]);
        string feature_name = Request.Form["feature_name"];
        int count_existed = _context.m_feature.Where(e => e.feature_name == feature_name && e.m_feature_id != m_feature_id).Count();
        if (count_existed > 0)
        { //cek data duplikat
          arm.fail();
          arm.message = "Data sudah ada (duplikat)!";
        }
        else
        {
          //construct object m_feature
          m_feature m_feature_data = new m_feature
          {
            m_feature_id = Convert.ToInt32(Request.Form["m_feature_id"]),
            m_feature_group_id = Convert.ToInt32(Request.Form["m_feature_group_id"]),
            feature_name = feature_name,
            feature_sequence = Convert.ToInt32(Request.Form["feature_sequence"]),
            feature_url = Request.Form["feature_url"],
            feature_icon = Request.Form["feature_icon"],
            feature_private = Convert.ToBoolean(Request.Form["feature_private"]),
          };
          _context.m_feature.Update(m_feature_data); //update m_feature yg diconstruct
          arm.success(); //set success status
          arm.message = "Data berhasil diperbaharui"; //set success message
        }
      }
      else if (Request.Query["f"] == "delete_handler")
      {
        int m_feature_id = Convert.ToInt32(Request.Query["id"]);
        var db_row = _context.m_feature.AsNoTracking().SingleOrDefault(e => e.m_feature_id == m_feature_id);
        if (db_row == null)
        {
          arm.fail();
          arm.message = "Data tidak ditemukan!";
        }
        else
        {
          _context.m_feature.Remove(db_row);
          arm.success(); //set success status
          arm.message = "Data berhasil dihapus"; //set success message
        }
      }

      try
      {
        _context.SaveChanges(); //save changes to database
      }
      catch(Exception ex)
      {
        arm.fail();
        arm.message = ex.Message;
        AppGlobal.console_log("Error Save: ", ex.ToString());
      }
      return new JsonResult(arm); //return ARM dg method JsonResult untuk auto serialize ke format JSON
    }
  }
}