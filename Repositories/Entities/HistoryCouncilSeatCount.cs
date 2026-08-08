using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Entities
{
    public class HistoryCouncilSeatCount
    {
        public int Id { get; set; }
        public int CouncilSeatCount { get; set; }


        //Navigation properties
        public int MunicipalityId { get; set; }
        public Municipality Municipality { get; set; } = null!;

        public int ElectionDistrictId { get; set; }
        public ElectionDistrict? ElectionDistrict { get; set; }

        public int ElectionId { get; set; }
        public Election Election { get; set; } = null!;
    }
}
