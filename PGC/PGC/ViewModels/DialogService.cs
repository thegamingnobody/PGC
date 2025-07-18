using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGC.ViewModels
{
	static class DialogService
	{
		public static string? OpenFile()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Filter = "JSON files (*.json)|*.json",
				Multiselect = false
			};

			return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
		}

		public static string? SaveFile()
		{
			SaveFileDialog dialog = new SaveFileDialog()
			{
				Filter = "JSON files (*.json)|*.json",
				FileName = "Matches.json"
			};
			return dialog.ShowDialog() == true ? dialog.FileName : null;
		}
	}
}
