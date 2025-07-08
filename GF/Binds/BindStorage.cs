using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace GF.UI
{
    // =====================================================================
    //  СПІЛЬНИЙ СХОВИЩЕ БІНДІВ
    // =====================================================================
    public static class BindStorage
    {
        public static readonly string FilePath = Path.Combine(Application.StartupPath, "binds.json");

        public static List<BindModel> Load()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string json = File.ReadAllText(FilePath);
                    var list = JsonSerializer.Deserialize<List<BindModel>>(json);
                    return list ?? new List<BindModel>();
                }
            }
            catch { }
            return new List<BindModel>();
        }

        public static void Save(List<BindModel> binds)
        {
            try
            {
                string json = JsonSerializer.Serialize(binds, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
            catch { }
        }
    }

    // =====================================================================
    //  МОДЕЛЬ БІНДА
    // =====================================================================
    public class BindModel
    {
        public string Key { get; set; }
        public string Text { get; set; }
        public bool IsActive { get; set; }
    }
}