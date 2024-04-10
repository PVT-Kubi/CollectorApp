
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectorApp.Models
{
    internal class AllCollections
    {
        public ObservableCollection<Collection> Collections { get; set; } = new ObservableCollection<Collection>();

        public AllCollections() { }

        public void LoadCollections()
        {
            Collections.Clear();

            string appDataPath = FileSystem.AppDataDirectory;

            IEnumerable<Collection> collections = Directory
                .EnumerateFiles(appDataPath, "*.cl.txt")
                .Select(filename => new Collection()
                {
                    Filename = filename,
                    CollectionName = File.ReadAllText(filename).Split('\n')[0],
                    Text = File.ReadAllText(filename)
                });

            foreach (Collection c in collections)
            {
                Collections.Add(c);
            }
        }

        public int AddCollection(string name)
        {
            string appDataPath = FileSystem.AppDataDirectory;
            string collectionName = $"{name}.cl.txt";

            if (File.Exists(Path.Combine(appDataPath, collectionName)))
            {
                return 1;
            }

            File.WriteAllText(Path.Combine(appDataPath, collectionName), $"{name}\nName;string\tPrice;int\tState;nowy_używany_na-sprzedaż_sprzedany\tRating;int\tComment;string\tPicture;string");
            LoadCollections();
            return 0;
        }

        public int DeleteCollection(string name)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, $"{name}.cl.txt");

            if (!File.Exists(path)) return 1;

            File.Delete(path);

            return 0;
        }

    }
}
