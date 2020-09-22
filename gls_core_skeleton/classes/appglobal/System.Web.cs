using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Web
{
  namespace Hosting
  {
    public static class HostingEnvironment
    {
      public static bool m_IsHosted;

      static HostingEnvironment()
      {
        m_IsHosted = false;
      }

      public static bool IsHosted
      {
        get
        {
          return m_IsHosted;
        }
      }
    }
  }

  /// <summary>
  /// Used to make class's code context aware without passed parameter from view
  /// </summary>
  public static class HttpContext
  {
    public static IApplicationBuilder AppProvider;

    static HttpContext() { }

    public static Microsoft.AspNetCore.Http.HttpContext Current
    {
      get
      {
        object factory = AppProvider.ApplicationServices.GetService(typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor));

        Microsoft.AspNetCore.Http.HttpContext context = ((Microsoft.AspNetCore.Http.HttpContextAccessor)factory).HttpContext;

        return context;
      }
    }
  }
}