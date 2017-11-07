using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Net;
using System.Reflection;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _Hell_Updater
{
    class Program
    {
        /*
         * формат ключей "args" при запуске:
         * updater article exefile
         * updater hellzkh2site "!Hell zkh2site.exe"
         */

        static void Main(string[] args)
        {
            try
            {
                WebClient client = new WebClient();

                FileVersionInfo myUpdName = FileVersionInfo.GetVersionInfo("updater.exe");

                Console.WriteLine("===================================================");
                Console.WriteLine("=      " + myUpdName.ProductName.ToString() + " v" + Assembly.GetExecutingAssembly().GetName().Version + "                    =");
                Console.WriteLine("===================================================");


                // делаем запрос, в ответ получаем данные в JSON-формате:
                /*
                 * { 
                 *  (
                 *      'art':'article',
                 *      'version':'1.0.0.0',
                 *      'changes':'description of changes'
                 *  )
                 * }
                 * 
                 * Шаги выполнения:
                 *   1) делаем запрос на "http://ai-rus.com/updater/args[0]", где "args[0]" - артикул программы
                 *   2) получаем ответ в JSON-формате.
                 *   3) сравниваем версию, полученную путем считывания из exe-файла с полученной в JSON
                 *   4) если версия на сайте более новая - обновляем ПО
                 *   5) запускаем по, имя exe-файла которого указано в параметре "args[1]"
                 *   
                 *  при любом запросе вывод JSON-данных идет в следующем порядке:
                 *  0 - апдейтер
                 *  1 - тикеты
                 *  2 - прога
                 */

                Console.WriteLine("Get version...");

                WebRequest wr = WebRequest.Create(new Uri("http://ai-rus.com/updater/" + args[0]));
                WebResponse ws = wr.GetResponse();
                StreamReader sr = new StreamReader(ws.GetResponseStream());

                string json = sr.ReadToEnd();

                // парсим JSON
                rootUpdater expJSON = JsonConvert.DeserializeObject<rootUpdater>(json);

                // считываем данные апдейтера
                int updVer = Convert.ToInt16(expJSON.updater[0].version.Replace(".", ""));
                string updFile = expJSON.updater[0].file;

                // считываем данные тикета
                int tikVer = Convert.ToInt16(expJSON.updater[1].version.Replace(".", ""));
                string tikFile = expJSON.updater[1].version;

                // считываем данные обновляемой программы
                int sfwVer = Convert.ToInt16(expJSON.updater[2].version.Replace(".", ""));
                string sftFile = expJSON.updater[2].version;

                /****************************************
                 * Обновляем загрузчик
                 * **************************************/
                if (!File.Exists("updater.exe"))
                {
                    client.DownloadFileAsync(new Uri("http://ai-rus.com/uploads/downloads/" + updFile), "updater.exe");
                }
                else
                {
                    //  Проверяем версию апдейтера
                    int myUpdVer = Convert.ToInt16(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));

                    //  Если апдейтер старый, то качаем новый
                    if (myUpdVer < updVer)
                    {
                        client.DownloadFileAsync(new Uri("http://ai-rus.com/uploads/downloads/" + updFile), "updater.exe");
                    }
                }


                /****************************************
                 * Обновляем модуль тикетов
                 * **************************************/
                if (!File.Exists("tickets.exe"))
                {
                    client.DownloadFileAsync(new Uri("http://ai-rus.com/uploads/downloads/" + tikFile), "tickets.exe");
                }
                else
                {
                    //  Проверяем версию
                    FileVersionInfo myTickets = FileVersionInfo.GetVersionInfo("tickets.exe");

                    //  Если старый, то качаем новый
                    if (Convert.ToInt16(myTickets.FileVersion.Replace(".", "")) < tikVer)
                    {
                        client.DownloadFileAsync(new Uri("http://ai-rus.com/uploads/downloads/" + tikFile), "tickets.exe");
                    }
                }


                /****************************************
                 * Обновляем саму прогу
                 * **************************************/
                FileVersionInfo mySoftware = FileVersionInfo.GetVersionInfo(args[1]);

                if (Convert.ToInt16(mySoftware.FileVersion.Replace(".", "")) < sfwVer)
                {
                    client.DownloadFileAsync(new Uri("http://ai-rus.com/uploads/downloads/" + sftFile), args[1]);
                }


                // Заканчиваем обновление
                Console.WriteLine("Download file. Please wait...");

                client.DownloadFileAsync(new Uri("http://ai-rus.com/updater/" + args[0]), args[2]);

                Console.WriteLine("Download succesfully");
                Console.WriteLine("Install updates. Please wait...");
                Console.WriteLine("Update succesfully installed");
                Console.WriteLine("Starting software...");

                System.Diagnostics.Process.Start(args[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine("Error :: " + e.Message);
                Console.ReadKey(true);
            }
        }
    }
}
