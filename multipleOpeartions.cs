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
       // WebClient webClient= new WebClient();      
       

        public multipleOpeartions()
        {
            _URLlist.AddRange(File.ReadAllLines(@"C:\Users\Saruchi\Desktop\daily dump\Threads exericse\test.txt"));
        }

        static void Main(string[] args)
        {
            new multipleOpeartions().spawnThreads ();    //is it ok to have a main here and create an instance of the same class itself ??!
            Console.ReadLine();
        }

        public void spawnThreads()
        {
                Thread t1 = new Thread(new ThreadStart ( downloadFiles));
                Thread t2 = new Thread(new ThreadStart (downloadFiles));

               // Thread t3 = new Thread(new multipleOpeartions().downloadFiles);

                t1.Start();
                t2.Start();
               // t3.Start();

                //while (t1.IsAlive) ;
                //t1.Abort();
                //while (t2.IsAlive) ;
                //t2.Abort();
                //while (t3.IsAlive) ;
                //t3.Abort();
 
        }

        public void downloadFiles()
        {
            int bytesProcessed = 0;

            Stream remoteStream = null;
            Stream localStream = null;
            WebResponse response = null;
            try
  {
    // Create a request for the specified remote file name

      List<string> URLlist = new List<string>();
      lock (URLlist)
      {
          URLlist.Add("http://static.isango.com/activitylocationmaps/12981.pdf");
          URLlist.Add("http://static.isango.com/activitylocationmaps/13034.pdf");
      }

      foreach (String url in URLlist)
      {
          WebRequest request = WebRequest.Create(url);

          if (request != null)
          {
              // Send the request to the server and retrieve the
              // WebResponse object 
              response = request.GetResponse();
              if (response != null)
              {
                  // Once the WebResponse object has been retrieved,
                  // get the stream object associated with the response's data
                  remoteStream = response.GetResponseStream();

                  lock (localStream)
                  {
                      localStream = File.Create(url.Substring (url.LastIndexOf('/') ));
                  }
                  // Allocate a 1k buffer
                  byte[] buffer = new byte[1024];
                  int bytesRead;

                  // Simple do/while loop to read from stream until
                  // no bytes are returned
                  do
                  {

                      bytesRead = remoteStream.Read(buffer, 0, buffer.Length);

                      localStream.Write(buffer, 0, bytesRead);


                      bytesProcessed += bytesRead;
                  } while (bytesRead > 0);
              }
          }
      }// end foreach
  }
  catch(Exception e)
  {
    Console.WriteLine(e.Message);
  }
  finally
  {
    // Close the response and streams objects here 
    // to make sure they're closed even if an exception
    // is thrown at some point
    if (response     != null) response.Close();
    if (remoteStream != null) remoteStream.Close();
    if (localStream  != null) localStream.Close();
  }

  // Return total bytes processed to caller.
 
}
            //Queue<string> URLqueue = new Queue<string>();
            //lock (URLqueue)
            //{
            //    URLqueue.Enqueue("http://static.isango.com/activitylocationmaps/12981.pdf");
            //    URLqueue.Enqueue("http://static.isango.com/activitylocationmaps/13034.pdf");
            //}
            
            //   while (URLqueue.Any())
            //    {
            //        lock (URLqueue)
            //        {
            //            var nextItem = URLqueue.Dequeue();
            //            webClient.DownloadDataAsync(new Uri(nextItem), nextItem.Substring(nextItem.LastIndexOf('/') + 1));
            //        }
            //    }
            
        }

        


    }



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