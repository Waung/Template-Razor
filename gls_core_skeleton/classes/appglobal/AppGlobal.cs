using appglobal.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace appglobal
{
  /// <summary>
  /// Class utama untuk mengeset parameter project .netcore
  /// - Author    : Kurniawan
  /// - Modified  : 2018-09-03
  /// </summary>
  public static class AppGlobal
  {
    internal static string BASE_URL = "";
    internal static string LOG_DIR = @"./LOGS";
    internal static string MYSQL_CS = "Server=127.0.0.1;Port=30000;Database=gls_model;Uid=root;Pwd=GL-System123";
    internal static string OVERRIDE_CS = "";
    internal static string OVERRIDE_TM = "";

    /// <summary>
    /// Get primary connection string for .netcore project
    /// </summary>
    /// <param name="db_server"></param>
    /// <returns></returns>
    static internal string get_connection_string(string db_server = "MySQL")
    {
      string connection = "";
      if (db_server == "MySQL")
      {
        string file_setting = OVERRIDE_CS;
        connection = file_setting == "" ? MYSQL_CS : file_setting;
      }
      return connection;
    }

    /// <summary>
    /// Get primary working directory for application path searching
    /// </summary>
    /// <returns></returns>
    public static string get_working_directory()
    {
      return BASE_URL; //Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
    }


    /// <summary>
    /// Used in defining which connection to be used in a db_context
    /// </summary>
    /// <returns></returns>
    public static dynamic get_db_option()
    {
      DbContextOptionsBuilder ob = new DbContextOptionsBuilder<gls_model>();
      ob.UseMySql(get_connection_string());
      return ob.Options;
    }

    /// <summary>
    /// Used in determining whether system is activated or not
    /// </summary>
    /// <returns></returns>
    public static bool get_system_active_status()
    {
      /*DataHandler dh = new DataHandler();
      string check_connection = dh.ExecuteScalar("SELECT GETDATE() AS CurrentDateTime", null, "unlogged");
      string NPSN = dh.ExecuteScalar("SELECT NPSN FROM MstSekolah WHERE NPSN<>'00000000'", null, "unlogged");

      bool activated = false;
      try {
        IDGenerator ig = new IDGenerator();
        string get_serial_number = ig.MD5SerialNumber();
        string get_serial_number_from_file = ig.GetSerialNumberFromFile();
        activated = get_serial_number == get_serial_number_from_file && NPSN.Length == 8;
      } catch (Exception e) {
        activated = true;
      }
      */
      return true;
    }

    /// <summary>
    /// Used in getting multiple trademark options
    /// </summary>
    /// <returns></returns>
    public static string get_system_trademark()
    {
      string trademark = OVERRIDE_TM;
      trademark = trademark == "" ? "gls" : trademark;
      return trademark;
    }

    /// <summary>
    /// Used in getting nama instansi from database
    /// </summary>
    /// <returns></returns>
    public static string get_nama_instansi()
    {
      return "[NAMA INSTANSI]";
    }

    /// <summary>
    /// Used in getting tahun aktif from database
    /// </summary>
    /// <returns></returns>
    public static string get_tahun_aktif()
    {
      return "Tahun Akademik: " + "[TAHUN AKTIF]";
    }

    /// <summary>
    /// Used in getting semester aktif from database
    /// </summary>
    /// <returns></returns>
    public static string get_semester_aktif()
    {
      return "Semester Aktif: " + "[SEMESTER AKTIF]";
    }

    /// <summary>
    /// Get session timout value from db
    /// </summary>
    /// <returns></returns>
    public static int get_session_timeout()
    {
      gls_model _context = new gls_model(AppGlobal.get_db_option()); //simplifying context initializer by override
      int session_timeout = 0;
      try
      {
        session_timeout = Convert.ToInt32(_context.m_parameter.Where(e => e.parameter_key == "Session Timeout").Single().parameter_value);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return session_timeout;
    }

    /// <summary>
    /// Standardize console logging
    /// </summary>
    public static void console_log(string name, string content)
    {
      Console.WriteLine("======================================================");
      Console.Write(name + " >> ");
      Console.WriteLine(content);
      Console.WriteLine("======================================================");
    }

    /// <summary>
    /// Get url componen from path
    /// </summary>
    /// <returns></returns>
    public static string get_url_from_path(string path, string part = "feature")
    {
      int part_index = part == "feature" ? 2 : 1;
      string result = "";
      string base_path = "/scope";
      string split_url = path.Substring(base_path.Length);
      string[] urls = split_url.Split("/");
      result = urls[part_index];
      return result;
    }

    /// <summary>
    /// Get id from current logged in user
    /// </summary>
    /// <returns></returns>
    public static int get_user_login_id()
    {
      return Convert.ToInt32(System.Web.HttpContext.Current.User.FindFirst("user_id").Value);
    }

    /// <summary>
    /// Get id from current logged in user
    /// </summary>
    /// <returns></returns>
    public static int get_user_group_login_id()
    {
      gls_model _context = new gls_model(AppGlobal.get_db_option()); //simplifying context initializer by override
      return _context.m_user.Include(e => e.m_user_group).Where(e => e.m_user_id == get_user_login_id()).Single().m_user_group_id;
    }

    /// <summary>
    /// Get user validity with defined user_id & feature_id derived from url
    /// </summary>
    /// <returns></returns>
    public static bool get_user_validity()
    {
      gls_model _context = new gls_model(AppGlobal.get_db_option()); //simplifying context initializer by override

      bool valid = false;
      string path = System.Web.HttpContext.Current.Request.Path;
      string url_feature = get_url_from_path(path, "feature");
      string url_feature_group = get_url_from_path(path, "feature_group");
      int feature_id = 0;

      try
      {
        feature_id = _context.m_feature
          .Include(e => e.m_feature_group)
          .Where(e => e.feature_url == url_feature && e.m_feature_group.feature_group_name.ToLower().Replace(" ", "_") == url_feature_group)
          .Single().m_feature_id;
      }
      catch (Exception e)
      {
        console_log("error feature_id", e.ToString());
      }

      int feature_count = _context.feature_map
        .Where(e => e.m_feature_id == feature_id && e.m_user_group_id == get_user_group_login_id()).Count();

      valid = feature_count == 1 ? true : false;
      return valid;
    }

    /// <summary>
    /// Encode string before saving to the database
    /// Increasing consistency
    /// </summary>
    /// <param name="input">string to be encoded</param>
    /// <param name="mode">determining wether encode all or only non-ASCII character</param>
    /// <returns></returns>
    public static string string_encode(string input, string mode = "lite")
    {
      string pattern = mode == "lite" ? @"[^\u0020-\u007E]" : @"[^A-Za-z\\d]";
      return Regex.Replace(input, pattern, delegate (Match m)
      {
        Char c = Convert.ToChar(m.Value);
        return string.Format("&#{0};", (Int32)c);
      });
    }
  }
}