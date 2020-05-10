using System;
using System.Collections.Generic;
using Bulletin.EFCore;
using Bulletin.Models;
using Bulletin.Storages;

namespace Bulletin
{
    public class Bulletin<TContext> : IBulletin where TContext : IBulletinDbContext
    {
        private readonly IBulletinDbContext _dbContext;
        private readonly BulletinOptions _options;

        internal Bulletin(TContext dbContext, BulletinOptions options)
        {
            _options = options;
            _dbContext = dbContext;
        }

        public IBulletinBoard GetBoard(string name)
        {
            if (!_options.BulletinBoardOptions.ContainsKey(name))
            {
                throw new BulletinArgumentException($"No bulletin board named `{name}` is configured.");
            }

            return new BulletinBoard(_dbContext, _options.BulletinBoardOptions[name]);
        }

        public string AbsoluteUrlFor(Attachment attachment)
        {
            if (!_options.BulletinBoardOptions.ContainsKey(attachment.Board))
            {
                throw new BulletinArgumentException(
                    $"Attachment {attachment.Id} is associated with bulletin board {attachment.Board} which is not registered");
            }

            return GetBoard(attachment.Board).AbsoluteUrlFor(attachment);
        }
    }
}