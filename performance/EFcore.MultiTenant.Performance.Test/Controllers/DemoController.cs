using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using EFcore.MultiTenant.Performance.Test.DbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFcore.MultiTenant.Performance.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class DemoController : ControllerBase
    {
        DemoContext demoContext;

        static ConcurrentDictionary<string, int> dic = new ConcurrentDictionary<string, int>(4, 20000);

        public DemoController(DemoContext demoContext)
        {
            this.demoContext = demoContext;
            this.demoContext.Database.EnsureCreated();
        }

        [HttpGet("test1")]
        public string test1([FromQuery] string p1, [FromQuery] string p2)
        {
            string tenantName = "";
            if (this.HttpContext.Request.Headers.ContainsKey("TenantName"))
            {
                tenantName = this.HttpContext.Request.Headers["TenantName"];
                if (dic.TryGetValue(tenantName, out int number))
                {
                    dic.TryUpdate(tenantName, number + 1, number);
                }
                else
                {
                    dic.TryAdd(tenantName, 1);
                }
            }

            // var t1 = this.demoContext.Tabel1.Find(1);

            return $"{p1}-{p2}";
        }

        [HttpGet("report")]
        public string getReport()
        {
            var onModelCreatingCount = demoContext.CountDown;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"onModelCreating runed times: {onModelCreatingCount}\n");
            sb.AppendLine($"tenant count: {dic.Count}");
            sb.AppendLine($"calling count: {dic.Sum(r => r.Value)}");

            foreach (var item in dic)
            {
                sb.AppendLine($"{item.Key}");
            }

            return sb.ToString();
        }
    }
}
