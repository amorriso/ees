using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EESTesterClientAPI
{
   
        public class jhlogs
        {
            StreamWriter logs = null;

            public jhlogs(string path)
            {
                //logs =  new StreamWriter("c:\\logs\\GFTApplicationlog.txt", true);
                logs = new StreamWriter(path, true);
            }
            public void logAdd(string strLog)
            {
                logs.WriteLine(strLog);
                logs.Flush();
            }
            public void logAdd(string strLog, int algoid)
            {
                try
                {
                    logs.WriteLine(DateTime.Now + " " + strLog);
                    logs.Flush();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
                finally
                {

                }
            }

            public void Close()
            {
                logs.Close();
            }
        }
    
}
