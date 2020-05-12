using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Bulletin.EFCore;
using Bulletin.Storage;

namespace Bulletin
{

    public class BulletinOptionsBuilder
    {
        private readonly Dictionary<string, BulletinBoardOptions> _boardOptions = new Dictionary<string, BulletinBoardOptions>();
        private static readonly Regex NameRe = new Regex(@"^[a-zA-Z0-9_-]+$");

        public BulletinOptionsBuilder AddBoard(BulletinBoardOptions options)
        {
            if (_boardOptions.ContainsKey(options.Name))
            {
                throw new ArgumentException($"Board with name `{options.Name}` already registered.");
            }

            if (!IsValidName(options.Name))
            {
                throw new ArgumentException($"Bulletin board name must be url-path safe, got {options.Name}");
            }

            _boardOptions.Add(options.Name, options);
            return this;
        }

        public BulletinOptions Create()
        {
            return new BulletinOptions(_boardOptions);
        }

        private static bool IsValidName(string name)
        {
            return NameRe.IsMatch(name);
        }
    }
}