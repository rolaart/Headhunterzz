using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace World {

	public static class IslandNamesDB {
		private static readonly List<string> Names = new List<string>();
		
		static IslandNamesDB() {
			using (StreamReader sr = File.OpenText("Assets/Database/island-names.txt")) {
				string islandName;
				while ((islandName = sr.ReadLine()) != null) {
					Names.Add(islandName);
				}
			}
		}

		public static string GetRandomName() {
			return Names[Random.Range(0, Names.Count)];
		}
		
	}

}