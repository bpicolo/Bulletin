using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Bulletin.Storages;

namespace Bulletin
{

    public class BulletinOptionsBuilder
    {
        private readonly Dictionary<string, Tuple<IStorage, BulletinBoardOptions>> _boards = new Dictionary<string, Tuple<IStorage, BulletinBoardOptions>>();
        private static readonly Regex NameRe = new Regex(@"^[a-zA-Z0-9_-]+$");

        public BulletinOptionsBuilder AddBoard(string name, IStorage storage, BulletinBoardOptions options)
        {
            if (_boards.ContainsKey(name))
            {
                throw new ArgumentException($"Board with name `{name}` already registered.");
            }

            if (!IsValidName(name))
            {
                throw new ArgumentException($"Bulletin board name must be url-path safe, got {name}");
            }

            _boards.Add(name, Tuple.Create(storage, options));
            return this;
        }

        public Bulletin Create()
        {
            return new Bulletin(_boards.ToDictionary(
                k => k.Key,
                k => new BulletinBoard(k.Key, k.Value.Item1, k.Value.Item2)));
        }

        private static bool IsValidName(string name)
        {
            return NameRe.IsMatch(name);
        }
    }
}