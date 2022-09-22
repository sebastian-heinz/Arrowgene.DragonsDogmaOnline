using System;

namespace Arrowgene.Ddon.Database.Model
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalName { get; set; }
        public string Hash { get; set; }
        public string Mail { get; set; }
        public string MailToken { get; set; }
        public string PasswordToken { get; set; }
        public string LoginToken { get; set; }
        public DateTime? LoginTokenCreated { get; set; }
        public bool MailVerified { get; set; }
        public DateTime? MailVerifiedAt { get; set; }
        public AccountStateType State { get; set; }
        public DateTime? LastAuthentication { get; set; }
        public DateTime Created { get; set; }

        public Account()
        {
            Id = -1;
        }
    }
}
