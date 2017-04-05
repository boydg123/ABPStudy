using System.Collections.Generic;
using Derrick.Caching.Dto;

namespace Derrick.Web.Areas.Mpa.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}