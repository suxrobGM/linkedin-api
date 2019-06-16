using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedIn.Api.SocialActions
{
    public class Thumbnails<T> where T: class
    {
        public Thumbnails()
        {
            Objects = new List<T>();
        }

        public List<T> Objects { get; set; }
    }
}
