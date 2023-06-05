using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspMvcUdemyPractice.Utility
{
	public class StripeSettings
	{
        // for payment note:make sure it is the same name on what you written in appsettings.json
        public string SecretKey { get; set; }
        public string PublishKey { get; set; }
    }
}
