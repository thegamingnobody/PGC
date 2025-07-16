using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PGC.Models
{
    partial class PlayerGroup : ObservableObject
    {
        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private ObservableCollection<Player> _players;

        [ObservableProperty]
        private bool _isSelected;

        public PlayerGroup(string name, ObservableCollection<Player> players)
        {
            Name = name;
            Players = new ObservableCollection<Player>(players);
        }
    }
}
