using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace appglobal
{
  public class crud_base
  {

    /// <summary>
    /// Generate edit button
    /// </summary>
    /// <param name="id">id that being used in generating controls</param>
    /// <returns></returns>
    public static string get_edit_button(dynamic id)
    {
      string edit_button = @"<button class='btn btn-primary btn-sm' onclick='edit_display(""" + id + @""")'><i class='fa fa-edit'></i> Ubah</button> ";
      return edit_button;
    }

    /// <summary>
    /// Generate delete button
    /// </summary>
    /// <param name="id">Id that being used to delete data</param>
    /// <returns></returns>
    public static string get_delete_button(dynamic id)
    {
      string edit_button = @"<button class='btn btn-primary btn-sm' onclick='send_delete_data(""" + id + @""")'><i class='fa fa-trash'></i> Hapus</button> ";
      return edit_button;
    }

    /// <summary>
    /// Generate standard input group with label & input tags
    /// </summary>
    /// <param name="name">Name & id for the input groups</param>
    /// <param name="label">Text to display in label</param>
    /// <param name="cannot_null">Set the input to be checked with validate_form</param>
    /// <param name="type">Type of input (text, password, number, etc)</param>
    /// <param name="value">Value to be passed in input group</param>
    /// <returns></returns>
    public static string get_input_group(string name, string label, bool cannot_null = false, string type = "text", string value = "")
    {
      string cannot_null_value = cannot_null == false ? "" : "cannot-null";
      string red_symbol = cannot_null == false ? "" : "<span style='color:red'>*</span>";
      string input_group = @"
      <div class='form-group row'>
        <label class='col-md-3 col-form-label text-right' for='" + name + @"'>" + label + @" " + red_symbol + @"</label>
        <div class='col-md-9'>
          <input type='" + type + @"' id='" + name + @"' class='form-control " + cannot_null_value + @"' placeholder='" + label + @"' name='" + name + @"' value='" + value + @"'>
        </div>
      </div>";
      return input_group;
    }

    /// <summary>
    /// Generate standard input
    /// </summary>
    /// <param name="name">Name & id for the input groups</param>
    /// <param name="type">Type of input (text, password, number, etc)</param>
    /// <param name="value">Value to be passed in input group</param>
    /// <returns></returns>
    public static string get_input(string name, string type = "text", string value = "")
    {
      string input = @"<input type='" + type + @"' id='" + name + @"' class='form-control' placeholder='" + value + @"' name='" + name + @"' value='" + value + @"'>";
      return input;
    }

    /// <summary>
    /// Generate select based on anonymous list sent from the view
    /// </summary>
    /// <param name="name"></param>
    /// <param name="label"></param>
    /// <param name="select_list">the anonymous list used to populate options</param>
    /// <param name="default_value">default selected index</param>
    /// <param name="disabled"></param>
    /// <returns></returns>
    public static string get_dropdown_group(string name, string label, dynamic select_list, string default_value = "-1", bool disabled = false)
    {
      bool cannot_null = true;
      string cannot_null_value = cannot_null == false ? "" : "cannot-null";
      string red_symbol = cannot_null == false ? "" : "<span style='color:red'>*</span>";

      string disabled_string = disabled == false ? "" : "disabled";
      string select = "<select  name='" + name + "' id='" + name + "' class='form-control' " + disabled_string + ">";
      foreach (var select_list_data in select_list)
      {
        //logic to parsing anonymous list with propertyInfo looping
        Type t = select_list_data.GetType();
        PropertyInfo[] pi = t.GetProperties();

        //define what property to be assigned
        string select_value = pi[0].GetValue(select_list_data) + "";
        string select_display = pi[1].GetValue(select_list_data);

        //create select options
        select = select + "<option value='" + select_value + "' ";
        AppGlobal.console_log("select_value", select_value);
        AppGlobal.console_log("default_value", default_value);
        if (select_value + "" == default_value)
        {
          select = select + "selected";
        }
        select = select + ">" + select_display + "</option>";
      }
      select = select + "</select>";

      string input_group = @"
      <div class='form-group row'>
        <label class='col-md-3 col-form-label text-right' for='" + name + @"'>" + label + @" " + red_symbol + @"</label>
        <div class='col-md-9'>
          " + select + @"
        </div>
      </div>";
      return input_group;
    }

    /// <summary>
    /// Generate switch group (checkbox modifier)
    /// Represented by hidden input to create consistency for backend post parameter
    /// </summary>
    /// <param name="name"></param>
    /// <param name="label"></param>
    /// <param name="_checked">the state of the checkbox</param>
    /// <param name="on_text">displayed text for on state</param>
    /// <param name="off_text">displayed text for off state</param>
    /// <returns></returns>
    public static string get_toggle_group(string name, string label, bool _checked = false, string on_text = "Ya", string off_text = "Tidak")
    {
      string checked_string = _checked == false ? "" : "checked";
      string input_group = @"
      <div class='form-group row'>
        <label class='col-md-3 col-form-label text-right' for='" + name + @"_area'>" + label + @"</label>
        <div class='col-md-9'>
          <input type='checkbox' name='" + name + @"_area' id='" + name + @"_area' class='form-control' " + checked_string + 
          @" data-on-text='" + on_text + @"' data-off-text='" + off_text + @"' onchange='toggle_update(""" + name + @""")'>
          <input type='hidden' id='" + name + @"' class='form-control' name='" + name + @"' value='" + _checked + @"'>
        </div>
      </div>
      <script>
        $('#" + name + @"_area').bootstrapSwitch();
      </script>";
      return input_group;
    }

    //RFC : Add
    //1. calendar control
    //2. text area control
    //3. rich text control >> modify send_insert_data & send_edit_data to include custom data getter
    //4. image upload (with preview)
    //5. file upload (non preview)
  }
}