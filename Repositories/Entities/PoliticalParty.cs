using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Entities
{
    public class PoliticalParty
    {
        public int Id { get; set; }
        public string PartyName { get; set; } = null!;
        public bool IsLocal { get; set; } = false;
    }
}
