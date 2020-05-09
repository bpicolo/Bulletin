using System;
using System.Collections.Generic;
using Bulletin.Models;
using Bulletin.Storages;

namespace Bulletin
{
    public class Bulletin : IBulletin
    {
        private readonly Dictionary<string, BulletinBoard> _boards;

        internal Bulletin(Dictionary<string, BulletinBoard> boards)
        {
            _boards = boards;
        }

        public IBulletinBoard GetBoard(string name)
            =>  _boards[name] ?? throw new BulletinArgumentException($"No bulletin board named `{name}` is configured.");

        public string AbsoluteUrlFor(Attachment attachment)
        {
            if (!_boards.ContainsKey(attachment.Board))
            {
                throw new BulletinArgumentException(
                    $"Attachment {attachment.Id} is associated with bulletin board {attachment.Board} which is not registered");
            }

            return GetBoard(attachment.Board).AbsoluteUrlFor(attachment);
        }
    }
}