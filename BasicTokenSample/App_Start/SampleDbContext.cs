using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicTokenSample.App_Start
{
    public class SampleDbContext : IdentityDbContext
    {
        public SampleDbContext()
        : base("SampleDbContext")
        {
        }
    }
}