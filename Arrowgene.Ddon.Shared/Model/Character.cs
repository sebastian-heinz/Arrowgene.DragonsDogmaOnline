using System;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Character
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Created { get; set; }
    }
}
