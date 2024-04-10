using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectorApp.Models
{
    internal class AllItems
    {
        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
        public Dictionary<string, string> ItemTypes { get; set; } = new Dictionary<string, string>();
        public List<string> columnTypes { get; set; } = new List<string>();
        public string collectionName;
        public AllItems(string collectionName) {
            this.collectionName = collectionName;
            this.columnTypes.Add("string");
            this.columnTypes.Add("integer");
            this.columnTypes.Add("multiple-choice");
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
                List<string> itemColumns = new List<string>();

                for(int j = 0; j < itemsData.Length; j++)
                {
                    string[] itemData = itemsData[j].Split("\t");
                    if (j == 1) 
                        itemColumns.AddRange(itemData);
                    else if(j > 1)
                    {
                        Item item = new Item();
                        
                        for(int k = 0; k < itemData.Length; k++)
                        {
                            if (item.Data.ContainsKey(itemColumns[k].Split(";")[0]))
                            {
                                item.Data[itemColumns[k].Split(";")[0]] = itemData[k];
                            }
                            else
                            {
                                item.Data.Add(itemColumns[k].Split(";")[0], itemData[k]);
                            }
                        }
                        items.Add(item);
                    }


                }
                foreach(string column in itemColumns)
                {
                    string[] columnSplit = column.Split(";");
                    if (!ItemTypes.ContainsKey(columnSplit[0]))
                        ItemTypes.Add(columnSplit[0], columnSplit[1]);
                }

                foreach (Item item in items)
                {
                    Items.Add(item);
                }

            }
        }

        public int AddItem(string cName, Dictionary<string, dynamic> data)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, $"{cName}.cl.txt");
            if (!File.Exists(path))
                return 1;

            Item item = new Item();
            item.Data = data;
            string text = File.ReadAllText(path);
            string itemData = text;
            bool first= false;
            foreach(var i in data)
            {
                if (!first)
                {
                    itemData += $"\n{i.Value}";
                    first = true;
                }
                else
                {
                    itemData += $"\t{i.Value}";
                }
            }

            File.WriteAllText(path, itemData);
            Items.Add(item);
            return 0;
        }

        public void AddColumn(string cName, string columnName, string columnType)
        {
           
            if(!ItemTypes.ContainsKey(columnName))
            {
                string path = Path.Combine(FileSystem.AppDataDirectory, $"{cName}.cl.txt");
                string text = File.ReadAllText(path);
                string newText = "";
                string[] itemData = text.Split('\n');
                int counter = 0;
                foreach(string s in itemData)
                {
                    if (counter == 1)
                    {
                        newText += "\n" + s + $"\t{columnName};{columnType}";
                        counter ++;
                    }
                    else if(counter > 1)
                    {
                        newText +=  "\n" + s +"\t";
                    }
                    else
                    {
                        newText += s;
                        counter++;
                    }
                }
                ItemTypes.Add(columnName, columnType);
                
                File.WriteAllText (path, newText);
            }
            
        }
    }
}
