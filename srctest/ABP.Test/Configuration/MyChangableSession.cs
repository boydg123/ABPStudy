using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.MultiTenancy;

namespace Abp.Test.Configuration
{
    public class MyChangableSession : IAbpSession
    {
        public long? UserId { get; set; }

        public int? TenantId { get; set; }

        public MultiTenancySides MultiTenancySide
        {
            get { return !TenantId.HasValue ? MultiTenancySides.Host : MultiTenancySides.Tenant; }
        }

        public long? ImpersonatorUserId { get; set; }

        public int? ImpersonatorTenantId { get; set; }
    }
}
