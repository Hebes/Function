using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace 唯一ID
{
    public class Test : MonoBehaviour
    {
        private void Awake()
        {
            Machine1();
            //Machine5();
        }

        static void Machine1()
        {
            for (int j = 0; j < 500; j++)
            {
                Task.Run(() =>
                {
                    IdWorker idworker = IdWorker.Singleton;
                    for (int i = 0; i < 10; i++)
                    {
                        Debug.Log(idworker.nextId());
                    }
                });
            }
        }

        static void Machine5()
        {
            List<IdWorker> workers = new List<IdWorker>();
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                workers.Add(new IdWorker(1, i + 1));
            }

            for (int j = 0; j < 500; j++)
            {
                Task.Run(() =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        int mid = random.Next(0, 5);
                        Debug.Log(workers[mid].nextId());
                    }
                });
            }
        }
    }
}