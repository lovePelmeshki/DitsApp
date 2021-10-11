using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp.Model
{
    public partial class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int? Permission { get; set; }
        public string Hash { get; set; }
    }
}
