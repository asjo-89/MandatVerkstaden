using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Entities;

public class Election
{
    public int Id { get; set; }
    public int ElectionYear { get; set; }



    #region Navigation properties
    public ICollection<ElectionConstituency> ElectionConstituencies { get; set; } = new List<ElectionConstituency>();
    public ICollection<OriginalElectionResultSet> OriginalElectionResultSets { get; set; } = new List<OriginalElectionResultSet>();
    #endregion
}
