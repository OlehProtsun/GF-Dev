using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using GF.Domain.Scheduling;
using GF.Infrastructure.Interfaces;
using GF.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GF.Infrastructure
{
    public sealed class FileScheduleRepository : IScheduleRepository
    {
        private readonly ILogger<FileScheduleRepository> _log;
        private readonly string _folder;

        private static readonly JsonSerializerOptions _jsonOpt = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        };

        public FileScheduleRepository(
            ILogger<FileScheduleRepository> log,
            IOptions<StorageOptions> opt)
        {
            _log = log;
            _folder = Path.GetFullPath(opt.Value.SchedulesFolder ?? "Schedules");
            Directory.CreateDirectory(_folder);
            _log.LogInformation("Schedule storage folder: {Folder}", _folder);
        }

        /*───────── реалізація старого інтерфейсу ─────────*/
        public void Save(SavedSchedule schedule) =>
            SaveAsync(schedule).GetAwaiter().GetResult();

        public IReadOnlyList<SavedSchedule> LoadAll() =>
            LoadAllAsync().GetAwaiter().GetResult();

        /*───────── НОВІ async-методи ─────────*/

        public async Task SaveAsync(SavedSchedule dto,
                                    CancellationToken ct = default)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var path = GetPath(dto.Name);

            using var fs = new FileStream(path,
                FileMode.Create, FileAccess.Write, FileShare.None,
                32 * 1024, FileOptions.Asynchronous);

            await JsonSerializer.SerializeAsync(fs, dto, _jsonOpt, ct)
                                .ConfigureAwait(false);

            await fs.FlushAsync(ct).ConfigureAwait(false);

            _log.LogInformation("Saved schedule {Name}", dto.Name);
        }

        public async Task<SavedSchedule?> LoadAsync(string name,
                                                    CancellationToken ct = default)
        {
            var path = GetPath(name);
            if (!File.Exists(path))
            {
                _log.LogWarning("Schedule {Name} not found", name);
                return null;
            }

            using var fs = new FileStream(path,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                32 * 1024, FileOptions.Asynchronous);

            var dto = await JsonSerializer
                .DeserializeAsync<SavedSchedule>(fs, _jsonOpt, ct)
                .ConfigureAwait(false);

            return dto;
        }

        public async Task<IReadOnlyList<SavedSchedule>> LoadAllAsync(
            CancellationToken ct = default)
        {
            var list = new List<SavedSchedule>();

            foreach (var file in Directory.EnumerateFiles(_folder, "*.json"))
            {
                ct.ThrowIfCancellationRequested();

                using var fs = File.OpenRead(file);
                var dto = await JsonSerializer
                    .DeserializeAsync<SavedSchedule>(fs, _jsonOpt, ct)
                    .ConfigureAwait(false);

                if (dto != null)
                    list.Add(dto);
            }
            return list;
        }

        public Task DeleteAsync(string name, CancellationToken ct = default)
        {
            var path = GetPath(name);
            if (File.Exists(path))
            {
                File.Delete(path);
                _log.LogInformation("Deleted schedule {Name}", name);
            }
            return Task.CompletedTask;
        }

        /*───────── helpers ─────────*/

        private string GetPath(string name)
        {
            var safe = string.Concat(name.Split(Path.GetInvalidFileNameChars()));
            return Path.Combine(_folder, $"{safe}.json");
        }
    }
}
