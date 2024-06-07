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

        public void ShowSongs(User user)
        {
            var songs = (user.UserType == UserType.Admin)
                ? _songInventory.GetAllSongs()
                : _songInventory.GetSongsByUser(user.Id);

            foreach (var song in songs)
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
            int songID = _songInventory.GetSongID(newSong);
            if (songID < 0)
            {
                songID = _songInventory.AddNewSongToDb(newSong);
            }
            _songInventory.AssignSongToUser(user.Id, songID);
        }
        public void RemoveSong(User user) 
        {
            var song = FindSong("remove", user);
            if (song != null)
            {
                _songInventory.RemoveAssignSongToUser(user.Id,song.Id);
                _songInventory.DeleteSongByID(song.Id);
            }
        }
        public void EditSong(User user) 
        {
            var song = FindSong("edit", user);
            if (song != null)
            {
                Console.WriteLine("Enter new data for this song:");
                var newSong = EnterSongData();
                _songInventory.UpdateSong(newSong, song.Id);
            }
        }
        public Song FindSong(string option, User user)
        {
            int songId=-1;
            var songsId = Array.Empty<int>();
            Song[] songs = Array.Empty<Song>();

            do
            {
                Console.WriteLine("Search for songs:");
                var searchString = Console.ReadLine();
                if (searchString.Length > 0)
                {
                    songs = user.UserType == UserType.Admin 
                        ? _songInventory.SearchSongs(searchString) 
                        : SearchSongsByUser(searchString, user.Id);
                }

                if (songs.Length > 0)
                {
                    foreach (var song in songs)
                    {
                        Console.WriteLine($"[{song.Id}] {song.Title} - {song.Author} ({song.Year}) /{song.Album}/");
                        Array.Resize(ref songsId, songsId.Length + 1);
                        songsId[songsId.Length - 1] = song.Id;
                    }

                    string input;
                    do
                    {
                        Console.WriteLine($"Enter the ID of the song you want to {option}:");
                        input = Console.ReadLine();
                        int.TryParse(input, out songId);
                    } while (!songsId.Contains(songId));
                    
                    return _songInventory.GetSongByID(songId);
                }
                else
                {
                    Console.WriteLine("Nothing found");
                    Console.WriteLine("You want to try again?");
                    if (!YesOrNo()) break;
                }
            } while (songs.Length <= 0); 
            return null;
        }
        
        public Song[] SearchSongsByUser(string searchString, int userId)
        {
            var userSongs = _songInventory.GetSongsByUser(userId);
            var foundSongs = Array.Empty<Song>();
            searchString = searchString.ToLower();
            foreach (var song in userSongs)
            {
                if (song.Title.ToLower().Contains(searchString) || song.Author.ToLower().Contains(searchString) || song.Album.ToLower().Contains(searchString))
                {
                    Array.Resize(ref foundSongs, foundSongs.Length + 1);
                    foundSongs[foundSongs.Length - 1] = song;
                }
            }
            return foundSongs;
        }
        
        private Song EnterSongData()
        {
            Console.WriteLine("Enter song title: ");
            var title = Console.ReadLine();
            Console.WriteLine("Enter song artist: ");
            var artist = Console.ReadLine();
            Console.WriteLine("Enter song album: ");
            var album = Console.ReadLine();
            int year;
            do
            {
                Console.WriteLine("Enter song year: ");
            } while (!int.TryParse(Console.ReadLine(), out year));

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
