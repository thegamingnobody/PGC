using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using PGC.Models;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace PGC.ViewModels
{
    partial class MainViewModel : ObservableObject
    {
        // Players
        [ObservableProperty]
        private ObservableCollection<Player> _players;

        [ObservableProperty]
        private string _newPlayerName = string.Empty;

        //// Sorted view of players
        //[ObservableProperty]
        //private ObservableCollection<Player> _sortedPlayers;

        [ObservableProperty]
        private ObservableCollection<PlayerGroup> _playerGroups;

        [ObservableProperty]
        private PlayerGroup _selectedGroup;

        [ObservableProperty]
        private int _groupCount = 3;

        [ObservableProperty]
        private string _groupCountText = string.Empty;

        // Constructor additions
        public MainViewModel()
        {
            Players = new ObservableCollection<Player>();
            PlayerGroups = new ObservableCollection<PlayerGroup>();

            //// Initialize with some default players
            Players.Add(new Player { Name = "Alice", Score = 10 });
            Players.Add(new Player { Name = "Bob", Score = 20 });
            Players.Add(new Player { Name = "Charlie", Score = 15 });
            Players.Add(new Player { Name = "Diana", Score = 25 });
            Players.Add(new Player { Name = "Eve", Score = 30 });
            Players.Add(new Player { Name = "Frank", Score = 5 });
            Players.Add(new Player { Name = "Grace", Score = 12 });
            Players.Add(new Player { Name = "Heidi", Score = 18 });
            //Players.Add(new Player { Name = "Ivan", Score = 22 });

            CreateRandomGroups(); 

            if (!(PlayerGroups.Count() == 0 || Players.Count() == 0))
            {
                SelectedGroup = PlayerGroups.First();
            }
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

            foreach (var p in Players)
            {
                if (p.Name == player.Name)
                {
                    // Player already exists, do not add
                    return;
                }
            }

            Players.Add(player);

            Players = new ObservableCollection<Player>(SortByScoreDescending(Players.ToList()));
        }

        [RelayCommand]
        void RemovePlayer(Player player)
        {
            if (Players.Contains(player))
                Players.Remove(player);

            Players = new ObservableCollection<Player>(SortByScoreDescending(Players.ToList()));
        }

        [RelayCommand]
        void CreateRandomGroups()
        {
            if (GroupCount <= 0 || Players.Count == 0)
                return;

            PlayerGroups.Clear();

            var rnd = new Random();
            var shuffled = Players.OrderBy(_ => rnd.Next()).ToList();
            var groups = Enumerable.Range(0, GroupCount).Select(i =>
                new PlayerGroup($"Group {i + 1}", new ObservableCollection<Player>())
            ).ToList();

            for (int i = 0; i < shuffled.Count; i++)
            {
                groups[i % GroupCount].Players.Add(shuffled[i]);
            }

            foreach (var group in groups)
                PlayerGroups.Add(group);

            SelectedGroup = PlayerGroups.FirstOrDefault();

            Players = new ObservableCollection<Player>(SortByScoreDescending(Players.ToList()));
        }

        public bool IsGroupSelected(PlayerGroup group)
        {
            return group.IsSelected;
        }

        [RelayCommand]
        void SelectGroup(PlayerGroup group)
        {
            foreach (var g in PlayerGroups)
                g.IsSelected = false;

            group.IsSelected = true;
            SelectedGroup = group;
        }

        [RelayCommand]
        void AddPointsToSelectedGroup()
        {
            if (SelectedGroup == null)
                return;

            foreach (var player in SelectedGroup.Players)
            {
                if (Players.Contains(player))
                {
                    foreach (var p in Players)
                    {
                        if (p.Name == player.Name)
                        {
                            p.Score += 1; // Increment score by 1
                        }
                    }
                }
            }

            Players = new ObservableCollection<Player>(SortByScoreDescending(Players.ToList()));
        }

        partial void OnGroupCountTextChanged(string value)
        {
            if (int.TryParse(value, out int count))
            {
                GroupCount = count;
            }
            else
            {
                GroupCount = 0; // or whatever fallback
            }
        }

        [RelayCommand]
        void SaveGroups()
        {
            //path > Save-path for Match data (to json)
            string? path = DialogService.SaveFile();
            if (path == null) return;
            
            MatchGame matchGame = new MatchGame
            {
                Players = Players.ToList(),
                PlayerGroups = PlayerGroups.ToList()
            };

            string serializedObject = JsonConvert.SerializeObject(matchGame);
            var test = Path.GetDirectoryName(path);
            if (Path.Exists(test))
            {
                File.WriteAllText(path, serializedObject);
            }
        }

        [RelayCommand]
        void LoadGroups()
        {
            string? path = DialogService.OpenFile();
            if (path == null) return;

            Players.Clear();
            PlayerGroups.Clear();

            var match = new MatchGame();

            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            var json = File.ReadAllText(path);
            match = JsonConvert.DeserializeObject<MatchGame>(json);

            foreach (Player player in match.Players)
            {
                Players.Add(player);
            }

            foreach (PlayerGroup group in match.PlayerGroups)
            {
                PlayerGroups.Add(group);
            }

            SelectedGroup = PlayerGroups.FirstOrDefault();

        }
        public static List<Player> SortByScoreDescending(List<Player> players)
        {
            var result = players.OrderByDescending(p => p.Score).ToList();

            for (int i = 0; i < result.Count; i++)
            {
                result[i].Rank = i + 1; // Assign rank (1-based)
            }

            return result;
        }
    }
}
