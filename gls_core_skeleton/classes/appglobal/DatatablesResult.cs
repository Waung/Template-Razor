using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace appglobal
{
  public class DatatablesResult
  {
    public int draw { get; set; }
    public int recordsTotal { get; set; }
    public int recordsFiltered { get; set; }
    public Object data { get; set; }
  }
}
