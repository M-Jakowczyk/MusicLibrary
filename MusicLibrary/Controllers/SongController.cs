using MusicLibrary.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MusicLibrary.Controllers
{
    internal class SongController
    {
        private static SongInventory _songInventory = new SongInventory();

        public void ShowAllSongs()
        {
            foreach (var song in _songInventory.GetAllSongs())
            {
                Console.WriteLine($"[{song.Id}] {song.Title} - {song.Author} ({song.Year}) /{song.Album}/");
            }
        }
    }
}
