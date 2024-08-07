﻿using System;
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

        //两种测试方法，均为500并发，生成5000个Id：
        //Machine1() 模拟1台主机，单例模式获取实例
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
        
        //Machine5() 模拟5台主机，创建5个实例
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