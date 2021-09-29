using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp
{
    public partial class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int? Permission { get; set; }
    }
}
