using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Infrastructure.Options
{
    public class StorageOptions
    {
        /// <summary>Папка, де зберігаємо .json-графіки.</summary>
        public string SchedulesFolder { get; set; } = "Schedules";
    }
}
