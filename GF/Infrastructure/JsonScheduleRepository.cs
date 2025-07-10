using GF.Infrastructure.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Infrastructure
{
    public class JsonScheduleRepository : IScheduleRepository
    {
        private readonly string _folder;

        public JsonScheduleRepository(string folder) => _folder = folder;

        public void Save(SavedSchedule s)
        {
            var path = Path.Combine(_folder, $"{s.Name}.gfs");
            var json = JsonConvert.SerializeObject(s, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public IReadOnlyList<SavedSchedule> LoadAll()
        {
            var list = new List<SavedSchedule>();
            foreach (var file in Directory.EnumerateFiles(_folder, "*.gfs"))
            {
                var json = File.ReadAllText(file);
                var dto = JsonConvert.DeserializeObject<SavedSchedule>(json);
                if (dto != null) list.Add(dto);
            }
            return list;
        }
    }
}
