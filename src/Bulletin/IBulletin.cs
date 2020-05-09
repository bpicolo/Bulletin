using System;
using Bulletin.Models;

namespace Bulletin
{
    public interface IBulletin
    {
        public IBulletinBoard GetBoard(string name);
        public string AbsoluteUrlFor(Attachment attachment);
    }
}