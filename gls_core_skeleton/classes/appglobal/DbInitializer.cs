using appglobal.models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace appglobal
{
  /// <summary>
  /// Main class for initiating database content on .netcore
  /// </summary>
  public static class DbInitializer
  {

    /// <summary>
    /// Main method to populate database content
    /// </summary>
    public static void Initialize()
    {
      using (var _context = new gls_model(AppGlobal.get_db_option()))
      {
        //run primary migration method
        _context.Database.Migrate();

        // Look for any user or feature group
        if (_context.m_user.Any() || _context.m_feature_group.Any())
        {
          return; // if DB has been seeded, exit the method;
        }

        //m_feature_group
        List<m_feature_group> m_feature_group = new List<m_feature_group> {
          new m_feature_group {
            feature_group_name = "Data"
          },
          new m_feature_group {
            feature_group_name = "Akses"
          },
          new m_feature_group {
            feature_group_name = "Master"
          },
        };
        foreach (m_feature_group m_feature_group_data in m_feature_group)
        {
          _context.m_feature_group.Add(m_feature_group_data);
        }
        _context.SaveChanges();

        //m_feature
        int m_feature_group_id = 1;
        List<m_feature> m_feature1 = new List<m_feature> {
          new m_feature {
            feature_name = "Dashboard", feature_sequence = 1, feature_url = "dashboard", feature_icon = "fa fa-dashboard", feature_private = true, m_feature_group_id = m_feature_group_id
          },
        };
        m_feature_group_id = 2;
        List<m_feature> m_feature2 = new List<m_feature> {
          new m_feature {
            feature_name = "Feature Group", feature_sequence = 1, feature_url = "m_feature_group", feature_icon = "fa fa-list-alt", feature_private = true, m_feature_group_id = m_feature_group_id
          },
          new m_feature {
            feature_name = "Feature", feature_sequence = 2, feature_url = "m_feature", feature_icon = "fa fa-list", feature_private = true, m_feature_group_id = m_feature_group_id
          },
          new m_feature {
            feature_name = "User Group", feature_sequence = 3, feature_url = "m_user_group", feature_icon = "fa fa-group", feature_private = false, m_feature_group_id = m_feature_group_id
          },
          new m_feature {
            feature_name = "User", feature_sequence = 4, feature_url = "m_user", feature_icon = "fa fa-user", feature_private = false, m_feature_group_id = m_feature_group_id
          },
        };
        m_feature_group_id = 3;
        List<m_feature> m_feature3 = new List<m_feature> {
          new m_feature {
            feature_name = "Dinas", feature_sequence = 1, feature_url = "m_dinas", feature_icon = "fa fa-building", feature_private = true, m_feature_group_id = m_feature_group_id
          },
          new m_feature {
            feature_name = "Sekolah", feature_sequence = 1, feature_url = "m_sekolah", feature_icon = "fa fa-university", feature_private = true, m_feature_group_id = m_feature_group_id
          },
        };

        List<m_feature> m_feature = m_feature1.Concat(m_feature2).Concat(m_feature3).ToList(); //add several list together
        foreach (m_feature m_feature_data in m_feature)
        {
          _context.m_feature.Add(m_feature_data);
        }
        _context.SaveChanges();

        //m_user_group
        List<m_user_group> m_user_group = new List<m_user_group> {
          new m_user_group {
            user_group_name = "Administrator", user_group_default = true
          },
        };
        foreach (m_user_group m_user_group_data in m_user_group)
        {
          _context.m_user_group.Add(m_user_group_data);
        }
        _context.SaveChanges();

        //m_user
        List<m_user> m_user = new List<m_user> {
          new m_user {
            user_name = "Admin", user_password = PasswordStorage.CreateHash("Admin"), user_email = "admin@email.com", user_active = true, m_user_group_id = 1
          },
        };
        foreach (m_user m_user_data in m_user)
        {
          _context.m_user.Add(m_user_data);
        }
        _context.SaveChanges();

        //feature_map
        List<feature_map> feature_map = new List<feature_map>();
        for (int i = 1; i <= 7; i++)
        {
          feature_map.Add(new feature_map
          {
            m_user_group_id = 1,
            m_feature_id = i
          });
        }
        foreach (feature_map feature_map_data in feature_map)
        {
          _context.feature_map.Add(feature_map_data);
        }
        _context.SaveChanges();

        //m_parameter
        List<m_parameter> m_parameter = new List<m_parameter> {
          new m_parameter {
            parameter_group = "Base Setting", parameter_key = "Session Timeout", parameter_value = "30"
          },
          new m_parameter {
            parameter_group = "Base Setting", parameter_key = "MaxArchiveFiles", parameter_value = "730"
          },
        };
        foreach (m_parameter m_parameter_data in m_parameter)
        {
          _context.m_parameter.Add(m_parameter_data);
        }
        _context.SaveChanges();
      }
    }
  }
}