using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Fault {
	public class FileUtilities {
		public static List<FileStream> OPEN_STREAMS = new List<FileStream>();
		
		public static FileStream getResource(String resource) {
			FileStream fs = File.OpenRead(resource);
			OPEN_STREAMS.Add (fs);
			return fs;
		}
		
		public static String readResourceToString(FileStream fs) {
			StringBuilder sb = new StringBuilder();
			using(StreamReader sr = new StreamReader(fs))  {
			    String line;
			    while ((line = sr.ReadLine()) != null)  {
			        sb.AppendLine(line);
			    }
				sr.Close();
			}
			String data = sb.ToString();
			try {fs.Close();} catch(Exception e) {}
			OPEN_STREAMS.Remove(fs);
			return data;
		}
		
		public static String getResourceAsString(String resource) {
			return readResourceToString(getResource(resource));
		}
		
		public static void closeOpenStreams() {
			foreach(FileStream fs in OPEN_STREAMS) {
				try {
					fs.Close();
				} catch(Exception ex) {
					
				}
			}
		}
	}
}

