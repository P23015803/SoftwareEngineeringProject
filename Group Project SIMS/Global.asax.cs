using System;
using System.Web;
using System.Web.UI;

namespace Group_Project_SIMS
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Register a ScriptResourceMapping named exactly "jquery" required by UnobtrusiveValidationMode
            ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
            {
                // Local paths (optional) - keep them if you add a local copy under ~/scripts/
                Path = "~/scripts/jquery-3.6.0.min.js",
                DebugPath = "~/scripts/jquery-3.6.0.js",
                // CDN fallback paths
                CdnPath = "https://code.jquery.com/jquery-3.6.0.min.js",
                CdnDebugPath = "https://code.jquery.com/jquery-3.6.0.js"
            });
        }
    }
}
