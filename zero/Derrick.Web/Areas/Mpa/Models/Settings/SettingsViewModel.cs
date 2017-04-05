using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Derrick.Configuration.Tenants.Dto;

namespace Derrick.Web.Areas.Mpa.Models.Settings
{
    public class SettingsViewModel
    {
        public TenantSettingsEditDto Settings { get; set; }
        
        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}