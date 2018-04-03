using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EFConnect.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Specialty { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        public string Interests { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public ICollection<Photo> Photos { get; set; }
        public ICollection<Follow> Follower { get; set; }

        public ICollection<Follow> Followee { get; set; }

        public User()
        {
            Photos = new Collection<Photo>();
        }
    }
}