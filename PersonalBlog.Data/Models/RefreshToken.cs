﻿using PersonalBlog.Data.Models.Base;

namespace PersonalBlog.Data.Models
{
    public class RefreshToken : BaseModel
    {
        public string Token { get; set; }
        public User? User { get; set; }
    }
}