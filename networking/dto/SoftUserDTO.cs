using System;

namespace networking.dto
{
    [Serializable]
    public class SoftUserDTO
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public SoftUserDTO() { }

        public SoftUserDTO(long id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }

        public override string ToString()
        {
            return $"{Username}";
        }
    }
}