using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.IO;

namespace threadTask
{
    class multipleOpeartions
    {
        private List<String> _URLlist = new List<string>();        
        //public List<string> URLlist
        //{
        //    get { return _URLlist; }            
            
        //}
        WebClient webClient; //= new WebClient();
        public multipleOpeartions()
        {
            _URLlist.AddRange(File.ReadAllLines(@"C:\Users\Saruchi\Desktop\daily dump\Threads exericse\test.txt"));
        }

        static void Main(string[] args)
        {
           new multipleOpeartions(). spawnThreads();
        }

        public void spawnThreads()
        {
            for (int i = 0; i < _URLlist.Count(); i++)
            {
                Thread t1 = new Thread(new multipleOpeartions().downloadFiles);
                Thread t2 = new Thread(new multipleOpeartions().downloadFiles);
                Thread t3 = new Thread(new multipleOpeartions().downloadFiles);

                t1.Start();
                t2.Start();
                t3.Start();

                //while (t1.IsAlive) ;
                //t1.Abort();
                //while (t2.IsAlive) ;
                //t2.Abort();
                //while (t3.IsAlive) ;
                //t3.Abort();

            }  //end for statement
        }

        public  void downloadFiles()
        {   
            /* --- critical section---- may need to use lock!!! ---*/
            for (int i = 0; i < _URLlist.Count(); i++)
            {
                webClient.DownloadFile(_URLlist[i], _URLlist[i].Substring(_URLlist[i].LastIndexOf('/') + 1));
            }
        }


    }
}
