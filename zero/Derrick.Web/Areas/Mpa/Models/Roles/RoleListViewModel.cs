﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Derrick.Web.Areas.Mpa.Models.Roles
{
    public class RoleListViewModel
    {
        public List<ComboboxItemDto> Permissions { get; set; }
    }
}