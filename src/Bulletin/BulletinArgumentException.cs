using System;

namespace Bulletin
{
    public class BulletinArgumentException : BulletinException
    {

        public BulletinArgumentException() { }
        public BulletinArgumentException(string message) : base(message) { }
        public BulletinArgumentException(string message, Exception inner) : base(message, inner) { }
    }
}