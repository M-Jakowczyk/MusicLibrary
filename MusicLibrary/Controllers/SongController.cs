using Microsoft.VisualBasic.FileIO;
using MusicLibrary.Db;
using MusicLibrary.Models;
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

        public void ShowSongs()
        {
            foreach (var song in _songInventory.GetAllSongs())
            {
                Console.WriteLine($"[{song.Id}] {song.Title} - {song.Author} ({song.Year}) /{song.Album}/");
            }
        }
        //public void ShowAllUserSongs(int userID)
        //{
        //    var userSongs = _songInventory.GetSongsByUser();
        //    foreach (var song in userSongs)
        //    {
        //        Console.WriteLine($"[{song.Id}] {song.Title} - {song.Author} ({song.Year}) /{song.Album}/");
        //    }
        //}

        public void AddNewSong(User user) {
            var newSong = EnterSongData();
            var songID = _songInventory.AddNewSongToDb(newSong);
        }
        public void RemoveSong() 
        {
            var song = FindSong("remove");
            _songInventory.DeleteSongByID(song.Id);
        }
        public void EditSong() 
        {
            var song = FindSong("edit");

            Console.WriteLine("Enter new data for this song:");
            var newSong = EnterSongData();
            _songInventory.UpdateSong(newSong, song.Id);
        }
        public Song FindSong(string option)
        {
            int songID;
            Song song;
            do
            {
                Console.WriteLine($"Enter the ID of the song you want to {option}:");
                songID = int.Parse(Console.ReadLine());
                song = _songInventory.GetSongByID(songID);
                Console.WriteLine("is this the song?");
                Console.WriteLine($"[{song.Id}] {song.Title} - {song.Author} ({song.Year}) /{song.Album}/");
            } while (YesOrNo());
            return song;
        }

        private Song EnterSongData()
        {
            Console.WriteLine("Enter song title: ");
            var title = Console.ReadLine();
            Console.WriteLine("Enter song artist: ");
            var artist = Console.ReadLine();
            Console.WriteLine("Enter song album: ");
            var album = Console.ReadLine();
            Console.WriteLine("Enter song year: ");
            var year = int.Parse(Console.ReadLine());
            return new Song(title, artist, album, year);
        }

        private static bool YesOrNo()
        {
            string YorN;
            do
            {
                Console.WriteLine("Yes / No:");
                YorN = Console.ReadLine();
            } while (!(YorN.ToLower() == "y" || YorN.ToLower() == "n"));
            return (YorN.ToLower() == "y");
        }
    }
}
