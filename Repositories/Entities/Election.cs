using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Entities;

public class Election
{
    public int Id { get; set; }
    public int ElectionYear { get; set; }



    #region Navigation properties
    public ICollection<Municipality> Municipalities { get; set; } = [];
    public ICollection<ElectionConstituency> ElectionConstituencies { get; set; } = [];
    public ICollection<OriginalElectionResultSet> OriginalElectionResultSets { get; set; } = [];
    #endregion
}
