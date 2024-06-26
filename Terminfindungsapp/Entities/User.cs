﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Terminfindungsapp
{
    public class User
    {
        private string id;
        private string username;
        private string password;
        private string firstname;
        private string lastname;

        private static User instance = null;
        private static readonly object padlock = new object();

        public User()
        {

        }

        public User(string username, string password, string firstname, string lastname)
        {
            this.username = username;
            this.password = password;
            this.firstname = firstname;
            this.lastname = lastname;
        }

        public static User GetInstance(User value)
        {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new User();
                        instance.id = value.ID;
                        instance.username = value.Username;
                        instance.firstname = value.firstname;
                        instance.lastname = value.lastname;
                    }
                }
            }
            return instance;
        }

        [JsonPropertyName("id")]
        public string ID
        {
            set { id = value; }
            get { return id; }
        }
        [JsonPropertyName("username")]
        public string Username
        {
            set { username = value; }
            get { return username; }
        }
        [JsonPropertyName("firstname")]
        public string Firstname
        {
            set { firstname = value; }
            get { return firstname; }
        }
        [JsonPropertyName("lastname")]
        public string Lastname
        {
            set { lastname = value; }
            get { return lastname; }
        }

        [JsonPropertyName("password")]
        public string Password
        {
            set { password = value; }
            get { return password; }
        }
    }
}
