using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Entities
{
    public class CouncilSeatAllocations
    {
        public int Id { get; set; } 
        public int SeatNumber { get; set; }
        public int PartySeatCount { get; set; }
        public decimal Quotient { get; set; }
        public decimal AllocationDivisor { get; set; }


        #region Navigation properties
        public int PoliticalPartyId { get; set; }
        public PoliticalParty PoliticalParty { get; set; } = new PoliticalParty();

        public int MunicipalityId { get; set; }
        public Municipality Municipality { get; set; } = new Municipality();

        public int ElectionId { get; set; }
        public Election Election { get; set; } = new Election();
        #endregion
    }
}
