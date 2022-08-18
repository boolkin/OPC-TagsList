/*=====================================================================
  File:      OPCCSharp.cs

  Summary:   OPC sample client for C#

-----------------------------------------------------------------------
  This file is part of the Viscom OPC Code Samples.

  Copyright(c) 2001 Viscom (www.viscomvisual.com) All rights reserved.

THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
PARTICULAR PURPOSE.
======================================================================*/

using System;
using System.Threading;
using OPC.Common;
using OPC.Data.Interface;
using OPC.Data;
using System.Collections;
using System.IO;

namespace CSSample
{
    class Tester
    { 
        public void Work()
        {
            //Создаем экземпляр объекта OpcServerList
            OpcServerList ServersList = new OpcServerList();

            //Создаем массив OpcServers[]
            OpcServers[] SrvListArray;

            //формируем массив серверов
            ServersList.ListAllData20(out SrvListArray);

            Console.WriteLine("Доступные OPC сервера:");

            for (int i=0; i< SrvListArray.Length;i++)
            {
                Console.Write("{0}  {1}", i+1, SrvListArray[i]); // +1, т.к. индекс начинается с нуля
                Console.WriteLine();
            }
            Console.WriteLine("----------");
            Console.Write("Введите номер сервера из списка выше: ");
            try
            {
                int numb = Int32.Parse(Console.ReadLine()) - 1; //-1, т.к. индекс начинается с нуля

                //Создаем экземпляр объекта OpcServerList
                OpcServer theSrv = new OpcServer();
                //получаем ProgID выбранного сервера
                string serverProgID = SrvListArray[numb].ProgID;
                //подключаемся к нему
                theSrv.Connect(serverProgID);

                Console.WriteLine("Формирую список...");
                Thread.Sleep(500); //задержка для подключения

                //создаем ArrayList, и помещаем в него все теги
                ArrayList tagList = new ArrayList();
                theSrv.Browse(OPCBROWSETYPE.OPC_FLAT, out tagList);

                //записываем ArrayList в текстовый файл
                TextWriter textFile = null;
                try
                {
                    textFile = File.CreateText("tagList.txt");
                    textFile.WriteLine("Всего объектов {0}", tagList.Count);
                    foreach (object tag in tagList)
                        textFile.WriteLine(tag.ToString());
                }
                catch (Exception ex)
                {
                    Console.Write("Error saving file!");
                }
                finally
                {
                    if (textFile != null)
                        textFile.Close();
                    Console.Write("Список доступных тегов успешно сохранен в файл");
                }
            }
            catch
            {
                Console.Write("Ошибка. Либо неправильный ввод, либо не удалось подключиться к серверу");
            }

            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            Tester tst = new Tester();
            tst.Work();
        }
    }
}
