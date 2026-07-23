using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Entities
{
    public class Municipality
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int CouncilSeatCount { get; set; }
    }
}
