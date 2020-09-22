using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

using appglobal;
using NLog.Config;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using System.IO;
using appglobal.models;

namespace lego_world {
  public class Startup {
    public Startup(IHostingEnvironment env) {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();
      Configuration = builder.Build();

      AppGlobal.BASE_URL = env.ContentRootPath;
      AppGlobal.OVERRIDE_CS = SettingReader.read_setting_file("conn.dat");
      AppGlobal.OVERRIDE_TM = SettingReader.read_setting_file("tm.dat");
      DbInitializer.Initialize();
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.AddMvc(config => {
        var policy = new AuthorizationPolicyBuilder()
                         .RequireAuthenticatedUser()
                         .Build();
        config.Filters.Add(new AuthorizeFilter(policy));
      })
      .AddRazorPagesOptions(options => {
        options.Conventions.AddPageRoute("/main/base", "");
        options.Conventions.AddPageRoute("/main/login", "login");
        options.Conventions.AddPageRoute("/main/logout", "logout");
        options.Conventions.AddPageRoute("/main/reset", "reset");
        options.Conventions.AddPageRoute("/main/notfound", "{*url}");
        options.Conventions.AllowAnonymousToPage("/login");
        options.Conventions.AllowAnonymousToPage("/main/login");
        //options.Conventions.AuthorizeFolder(""); //for double layer scenario, not used in GLS porting project
      })
      .WithRazorPagesAtContentRoot(); //removing the needs of /Pages folder to contain .cshtml files, moving it to root folder
      ;

      services.AddDbContext<gls_model>(options => options.UseMySql(AppGlobal.get_connection_string())); //set DBContext - used in migration

      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //get HTTPCONTEXT

      //new core 2.0 session handling is different from v1.1, using below code
      services.AddAuthentication
      (o => {
        o.DefaultScheme = "appcore_cookie_instance";
        o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      })
        .AddCookie(o => {
          o.LoginPath = new PathString("/login");
          o.AccessDeniedPath = new PathString("/main/forbidden");
          o.ExpireTimeSpan = TimeSpan.FromMinutes(AppGlobal.get_session_timeout());
        }
      );
      services.AddAuthorization(opts => {
        //opts.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
      });

      services.Configure<IISOptions>(options => {
        options.ForwardClientCertificate = true; //IIS cannot access kestrel session claims by default, this code made it possible.
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svp) {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      System.Web.HttpContext.AppProvider = app;
      System.Web.Hosting.HostingEnvironment.m_IsHosted = true;

      if (env.IsDevelopment()) {
        app.UseBrowserLink();
        app.UseDeveloperExceptionPage();
      } else {
        app.UseExceptionHandler("/main/forbidden");
      }

      app.UseStaticFiles(new StaticFileOptions {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "")), //set the root physical address
        RequestPath = "" //set the root request address
      });

      app.UseAuthentication(); //core 2.0 change in auth

      // IMPORTANT: This session call MUST go before UseMvc()
      //app.UseSession();

      app.UseForwardedHeaders(
         new ForwardedHeadersOptions {
           ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
         });

      app.UseMvc();

    }
  }
}
