using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Entities
{
    public class ElectionDistrict
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int CouncilSeatCount { get; set; }


        // Navigation properties
        public int MunicipalityId { get; set; }
        public Municipality Municipality { get; set; } = null!;

        public int ElectionId { get; set; }
        public Election Election { get; set; } = null!;

    }
}
