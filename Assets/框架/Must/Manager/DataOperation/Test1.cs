using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
    public class Test1 : MonoBehaviour
    {
        private void Start()
        {
            Player player = new Player();
            // // JsonSave jsonOperation = new JsonSave();
            // // JsonSave.AddCheckType(new IntJsonSave());
            // // jsonOperation.Save(player,"玩家");
            //
            //
            // JsonOperation jsonOperation = new JsonOperation();
            // jsonOperation.Save(player,"玩家");

            // UnityJsonSerializer unityJsonSerializer = new UnityJsonSerializer();
            // string value = unityJsonSerializer.Serialize(player);
            // Debug.Log(value);

            // Jsons jsons = new Jsons();
            // jsons.InitMethod();
            // string value = jsons.Serialize(player);
            // Debug.Log(value);
            
            
            //Player test1 = unityJsonSerializer.Deserialize<Player>("{\"ID\":2,\"Name\":\"123\",\"Items1\":[1,2,3,4],\"Items2\":[{\"ID\":1,\"Name\":\"物品1\"},{\"ID\":2,\"Name\":\"物品2\"}]}");
        }
    }

    public class Player
    {
        public int ID = 1;
        public string Name = "123";
        public List<int> Items1 = new List<int>() { 1, 2, 3, 4 };
        public List<Item> Items2 = new List<Item>() { new Item(1, "物品1"), new Item(2, "物品2") };
    }

    public class Item
    {
        public Item(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public int ID = 1;
        public string Name = "物品1";
    }
}