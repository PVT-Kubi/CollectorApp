using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectorApp.Models
{
    internal class AllItems
    {
        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
        public string collectionName;
        public AllItems(string collectionName) {
            this.collectionName = collectionName;
            LoadItems(collectionName);
        }

        public string getColllectionName() { return this.collectionName; }

        public void LoadItems(string cName)
        {
            Items.Clear();

            string path = FileSystem.AppDataDirectory;
            List<Item> items= new List<Item>();

            if (File.Exists(Path.Combine(path, $"{cName}.cl.txt")))
            {
                string text = File.ReadAllText(Path.Combine(path, $"{cName}.cl.txt"));
                string[] itemsData = text.Split('\n');
                string[] itemColumns = {};

                for(int j = 0; j < itemsData.Length; j++)
                {
                    string[] itemData = itemsData[j].Split("\t");
                    if(j == 1) 
                        itemColumns = itemData;
                    else if(j > 1)
                    {
                        Item item = new Item();
                        for(int k = 0; k < itemData.Length; k++)
                        {
                            if (item.Data.ContainsKey(itemColumns[k]))
                            {
                                item.Data[itemColumns[k]] = itemData[k];
                            }
                            else
                            {
                                item.Data.Add(itemColumns[k], itemData[k]);
                            }
                        }
                        items.Add(item);
                    }
                }

                foreach (Item item in items) 
                { 
                    Items.Add(item);
                }
            }
        }

        public int AddItem(string cName, Dictionary<string, dynamic> data)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, $"{cName}");
            if (!File.Exists(path))
                return 1;

            Item item = new Item();
            item.Data = data;
            string text = File.ReadAllText(path);
            string itemData = text + "\n";
            bool first= false;
            foreach(var i in data)
            {
                if (first) {
                    itemData += $"{i.Value}";
                    first = true;
                }
                itemData += $"\t{i.Value}";
            }

            File.WriteAllText(path, itemData);
            Items.Add(item);
            return 0;
        }
    }
}
