using System.Collections.Generic;

namespace Chicken.Web.Models.Admin
{
    public class AdminPostViewModel
    {
        public string Text { get; set; }

        public IEnumerable<string> Photos { get; set; } 
    }
}