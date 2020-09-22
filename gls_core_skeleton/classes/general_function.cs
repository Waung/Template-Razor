using System;
using System.Web;
using System.Data;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for GeneralFunction
/// </summary>
public static class general_function
{

  public static string get_file_icon(string ext)
  {
    string icon = "mif-file-text";

    if (ext == "docx" || ext == "doc")
    {
      icon = "mif-file-word";
    }
    else if (ext == "xlsx" || ext == "xls")
    {
      icon = "mif-file-excel";
    }
    else if (ext == "ppt" || ext == "pptx")
    {
      icon = "mif-file-powerpoint";
    }
    else if (ext == "pdf")
    {
      icon = "mif-file-pdf";
    }
    else if (ext == "jpg" || ext == "png" || ext == "bmp" || ext == "gif" || ext == "jpeg")
    {
      icon = "mif-file-image";
    }

    return icon;
  }
  public static string get_file_type(string ext)
  {
    string type = "Umum";

    if (ext == "docx" || ext == "doc")
    {
      type = "MS-Word";
    }
    else if (ext == "xlsx" || ext == "xls")
    {
      type = "MS-Excel";
    }
    else if (ext == "ppt" || ext == "pptx")
    {
      type = "MS-Powerpoint";
    }
    else if (ext == "pdf")
    {
      type = "PDF";
    }
    else if (ext == "jpg" || ext == "png" || ext == "bmp" || ext == "gif" || ext == "jpeg")
    {
      type = "Image";
    }

    return type;
  }

  //public static string get_tahun_akademik_aktif()
  //{
  //  string Str = "0000/0000";
  //  DataHandler dh = new DataHandler();
  //  Str = dh.ExecuteScalar("SELECT TahunAkademikID FROM BukuTahunAkademik WHERE StatusAktif='True'");
  //  return Str;
  //}
  //public static string get_semester_akademik_aktif()
  //{
  //  string Str = "0";
  //  DataHandler dh = new DataHandler();
  //  Str = dh.ExecuteScalar("SELECT SemesterAkademik FROM BukuSemesterAkademik WHERE StatusAktif='True' AND TahunAkademikID='" + get_tahun_akademik_aktif() + "'");
  //  return Str;
  //}
  //public static string get_pegawai_id_aktif()
  //{
  //  string Str = "0";
  //  DataHandler dh = new DataHandler();
  //  Str = dh.ExecuteScalar("SELECT ISNULL(PegawaiID,'Non-Pegawai') FROM MstUser WHERE UserID='" + my_session.Current.user_id + "'","Non-Pegawai");
  //  return Str;
  //}
  //public static string get_kelas_ajar_aktif()
  //{
  //  string Str = "0";
  //  DataHandler dh = new DataHandler();
  //  Str = dh.ExecuteScalar("SELECT NamaKelas FROM KelasAktif WHERE TahunAkademikID='" + get_tahun_akademik_aktif() + "' AND WaliKelasID='"+ get_pegawai_id_aktif() + "'","Non-Kelas");
  //  //return Str;
  //  return "Non-Kelas";
  //}
  //public static string get_kurikulum_aktif()
  //{
  //  string Str = "0";
  //  DataHandler dh = new DataHandler();
  //  Str = dh.ExecuteScalar("SELECT NamaKurikulum FROM MstKurikulumNasional WHERE Aktif='True'", "Non-Kurikulum");
  //  return Str;
  //}

  //public static string get_NPSN_aktif()
  //{
  //  string Str = "0";
  //  DataHandler dh = new DataHandler();
  //  Str = dh.ExecuteScalar("Select NPSN from  MstSekolah where NamaSekolah <> 'Garuda Learning Sistem'", "Non-Kurikulum");
  //  return Str;
  //}
  //public static DataTable get_dt_nama_kelas()
  //{
  //  DataHandler dh = new DataHandler();
  //  string query_nama_kelas = @"SELECT NamaKelas FROM MstKelas WHERE (NamaKelas NOT LIKE '%Default') AND (StatusAktif = 1)";
  //  DataTable dt_nama_kelas = dh.getDataTable(query_nama_kelas, 1);
  //  return dt_nama_kelas;
  //}
  //public static string get_rpp_default(string RPPID)
  //{
  //  DataHandler dh = new DataHandler();
  //  DataTable dt = dh.getDataTable(@"SELECT NamaKelas,TemaID,SubTemaID,PembelajaranID from RPP WHERE RPPID = '" + RPPID + @"'", 4);
  //  string kelas = dh.ExecuteScalar(@"Select Kelas from MstKelas Where NamaKelas = '" + dt.Rows[0][0].ToString() + @"'");
  //  string TahunAkademik = dh.ExecuteScalar("SELECT TahunAkademikID FROM BukuTahunAkademik WHERE StatusAktif='True'");
  //  string SemesterAkademik = dh.ExecuteScalar("SELECT SemesterAkademik FROM BukuSemesterAkademik WHERE StatusAktif='True'");
  //  string Kelas_Default = kelas + "Default";

  //  string RPPID_Guru = dh.ExecuteScalar(@"SELECT RPPID
  //      FROM RPP 
  //      WHERE NamaKelas = '" + Kelas_Default + @"'
  //      AND  TahunAkademikID='0000/0000'
  //      AND  SemesterAkademik='" + SemesterAkademik + @"' 
  //      and  TemaID = '" + dt.Rows[0][1].ToString() + @"' 
  //      and  SubTemaID = '" + dt.Rows[0][2].ToString() + @"' 
  //      AND  PembelajaranID='" + dt.Rows[0][3].ToString() + @"'  
  //      and  TipeRPP = '1'");
  //  return RPPID_Guru;
  //}
  //public static string cetak_select_pegawai()
  //{
  //  DataHandler dh = new DataHandler();
  //  string select = "";
  //  if (get_pegawai_id_aktif() == "Non-Pegawai")
  //  {
  //    string query_pegawai = @"select PegawaiID,NamaPegawai FROM MstPegawai WHERE StatusAktif='True'";
  //    select = @"<select id='PegawaiID' name='PegawaiID' class='form-control'>";
  //    DataTable dt1 = dh.getDataTable(query_pegawai, 2); int size1 = dt1.Rows.Count;
  //    for (int ki = 0; ki < size1; ki++)
  //    {
  //      string selected = "";
  //      select = select + @"
  //      <option value='" + dt1.Rows[ki][0].ToString() + @"' " + selected + @">
  //      " + dt1.Rows[ki][1].ToString() + @" 
  //      </option>";
  //    }
  //    select = select + @"</select>";
  //  }
  //  else
  //  {
  //    string query_pegawai = @"select PegawaiID,NamaPegawai FROM MstPegawai WHERE PegawaiID = '" + get_pegawai_id_aktif() + "'";
  //    select = @"
  //    <input type='hidden' name='PegawaiID' value='"+ get_pegawai_id_aktif() + @"'>
  //    <select id='PegawaiID' name='PegawaiID' disabled class='form-control'>";
  //    DataTable dt1 = dh.getDataTable(query_pegawai, 2); int size1 = dt1.Rows.Count;
  //    for (int ki = 0; ki < size1; ki++)
  //    {
  //      string selected = "";
  //      select = select + @"
  //      <option value='" + dt1.Rows[ki][0].ToString() + @"' " + selected + @">
  //      " + dt1.Rows[ki][1].ToString() + @" 
  //      </option>";
  //    }
  //    select = select + @"</select>";
  //  }


  //  return select;
  //}
  public static String string_encode(String input)
  {
    return Regex.Replace(input,@"[^A-Za-z\\d]",delegate (Match m) {
      Char c = Convert.ToChar(m.Value);
      return String.Format("&#{0};",(Int32)c);
    });
  }
	public static String lite_string_encode(String input)
	{
		return Regex.Replace(input, @"[^\u0020-\u007E]", delegate (Match m)
		{
			Char c = Convert.ToChar(m.Value);
			return String.Format("&#{0};", (Int32)c);
		});
	}
}