using System;
using System.Collections.Generic;
using Bulletin.Storages;

namespace Bulletin
{
    public class BulletinOptions
    {
        public readonly Dictionary<string, BulletinBoardOptions> BulletinBoardOptions;

        public BulletinOptions(Dictionary<string, BulletinBoardOptions> boardOptions)
        {
            BulletinBoardOptions = boardOptions;
        }
    }
}