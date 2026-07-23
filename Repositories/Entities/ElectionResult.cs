using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Entities
{
    public class ElectionResult
    {
        public int Id { get; set; }
        public int NumberOfVotes { get; set; }
        public decimal VotePercentage { get; set; }


        #region Navigation properies
        public int MunicipalityId { get; set; }
        public Municipality Municipality { get; set; } = null!;

        public int PoliticalPartyId { get; set; }
        public PoliticalParty PoliticalParty { get; set; } = null!;

        public int ElectionId { get; set; }
        public Election Election { get; set; } = null!;
        #endregion
    }
}
