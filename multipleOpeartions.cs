using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.IO;
using System.ComponentModel;

namespace threadTask
{
    class multipleOpeartions
    {
        private List<String> _URLlist = new List<string>();     
        WebClient webClient= new WebClient();
        Queue<string> URLqueue;

        public multipleOpeartions()
        {
            _URLlist.AddRange(File.ReadAllLines(@"C:\Users\Saruchi\Desktop\daily dump\Threads exericse\test.txt"));
             URLqueue = new Queue<string> (_URLlist); 
        }

        static void Main(string[] args)
        {
            new multipleOpeartions().spawnThreads();    //is it ok to have a main here and create an instance of the same class itself here??!
        }

        public void spawnThreads()
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
 
        }

        public bool downloadFiles()
        {
            try
            {

                while (URLqueue.Any())
                {
                    object _lock = new object();
                    lock (_lock)
                    {
                        var nextItem = URLqueue.Dequeue();
                        webClient.DownloadFile(new Uri(nextItem), nextItem.Substring(nextItem.LastIndexOf('/') + 1)) ;
                    }
                }
            }//end try
            catch(IOException)
            {
                return true;
            }
            return false;
            
        }

        


    } //end class
}// end namespace


#region commentedOutSection

/* --- critical section---- may need to use lock!!! ---*/
//public  void downloadFiles()
//        {
//for (int i = 0; i < _URLlist.Count(); i++)
//{
//    webClient.DownloadFile(_URLlist[i], _URLlist[i].Substring(_URLlist[i].LastIndexOf('/') + 1));
// }
//}
#endregion