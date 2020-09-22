using System.Web;

/// <summary>
/// Summary description for MySession
/// </summary>
public class my_session
{
  // private constructor
  private my_session()
  {
  }

  // Gets the current session.
  public static my_session Current
  {
    get
    {
      my_session session =
        (my_session)HttpContext.Current.Session["__MySession__"];
      if (session == null)
      {
        session = new my_session();
        HttpContext.Current.Session["__MySession__"] = session;
      }
      return session;
    }
  }

  // **** add your session properties here, e.g like this:
  public string user_id { get; set; }
  public string user_category_id { get; set; }
  public string user_name { get; set; }
  public string display_type { get; set; } //admin atau guru
  public string kelas_ajar { get; set; } //1-6 atau 0 untuk all
}
