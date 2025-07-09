using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PGC.Models;
using System.Collections.ObjectModel;

namespace PGC.ViewModels
{
    partial class MainViewModel : ObservableObject
    {
        // Players
        [ObservableProperty]
        private ObservableCollection<Player> _players;

        [ObservableProperty]
        private string _newPlayerName;

        //// Sorted view of players
        //[ObservableProperty]
        //private ObservableCollection<Player> _sortedPlayers;

        // Constructor additions
        public MainViewModel()
        {
            Players = new ObservableCollection<Player>();
        }

        [RelayCommand]
        void AddPlayer()
        {
            if (string.IsNullOrWhiteSpace(NewPlayerName))
                return;

            var player = new Player
            {
                Name = NewPlayerName,
                Score = 0 // default score
            };

            if (!Players.Contains(player))
                Players.Add(player);
        }

        [RelayCommand]
        void RemovePlayer(Player player)
        {
            if (Players.Contains(player))
                Players.Remove(player);
        }

    }
}
