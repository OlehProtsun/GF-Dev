using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using GF.Scheduling;
using GF.UI;
using System.Windows.Forms;

namespace GF.Scheduling.IO
{
    public static class ScheduleFileService
    {
        private const string SaveDir = "Schedules";
        private const string Ext = ".json";

        static ScheduleFileService()
        {
            Directory.CreateDirectory(SaveDir);
        }

        /*────────── ЗБЕРЕГТИ ──────────*/
        public static void Save(SavedSchedule dto)
        {
            string fileName = $"{Sanitize(dto.Name)}_{DateTime.Now:MM_dd}{Ext}";
            string path = Path.Combine(SaveDir, fileName);

            string json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        /*────────── СПИСОК ФАЙЛІВ ──────────*/
        public static IReadOnlyList<(string display, string path)> GetAll()
        {
            return Directory.EnumerateFiles(SaveDir, $"*{Ext}")
                            .Select(p => (Path.GetFileNameWithoutExtension(p), p))
                            .OrderByDescending(t => File.GetCreationTime(t.Item2))   //  t.Item2, не t.path
                            .ToList();
        }

        /*────────── ВІДКРИТИ ──────────*/
        public static SavedSchedule Load(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<SavedSchedule>(json)
                   ?? throw new InvalidDataException("Формат файлу невірний");
        }

        /*────────── helper ──────────*/
        private static string Sanitize(string raw)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                raw = raw.Replace(c.ToString(), string.Empty);
            return raw.Trim();
        }

        public static void Delete(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        public static class ContainerStorage
        {
            private static readonly string FilePath =
                Path.Combine(System.Windows.Forms.Application.StartupPath, "containers.json");

            public static IList<MonthContainer> GetAll()
            {
                if (!File.Exists(FilePath)) return new List<MonthContainer>();
                var json = File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<IList<MonthContainer>>(json) ?? new List<MonthContainer>();
            }

            public static void Save(IEnumerable<MonthContainer> containers)
            {
                var json = JsonConvert.SerializeObject(containers, Formatting.Indented);
                File.WriteAllText(FilePath, json);
            }

            /// <summary>Додає або замінює один контейнер і зберігає увесь список.</summary>
            public static void SaveSingle(MonthContainer mc)
            {
                var list = GetAll();

                // ⚠️  критерієм тотожності нехай буде «такий самий рік-місяць та ім’я»
                var idx = list.ToList().FindIndex(c =>
                       c.Year == mc.Year &&
                       c.Month == mc.Month &&
                       c.Name == mc.Name);

                if (idx >= 0) list[idx] = mc;  // overwrite
                else list.Add(mc);    // append

                Save(list);
            }
        }


    }
}
