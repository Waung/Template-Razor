using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace appglobal
{
  public class LogItem
  {
    public string username { set; get; }
    public string fitur { set; get; }
    public string aksi { set; get; }
    public string time { get; set; }
    public string message { get; set; }
    public string browser_name { get; set; }
    public string browser_version { get; set; }
    public string os_name { get; set; }
    public string os_version { get; set; }
    public string ip_address { get; set; }
  }
}