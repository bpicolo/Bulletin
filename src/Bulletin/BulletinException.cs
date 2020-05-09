using System;

namespace Bulletin
{
    public class BulletinException : Exception
    {
        public BulletinException() { }
        public BulletinException(string message) : base(message) { }
        public BulletinException(string message, Exception inner) : base(message, inner) { }
    }
}
