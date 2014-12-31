using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Appender;

namespace Albian.Kernel.log4netPlugin
{
   public class AlbianRollingFileAppender : RollingFileAppender
{
    //protected String format = "yyyyMMddHHmmss";
    //protected String suffix="log";
    //protected String fileName = "albianj";
	
	
    // public String getFormat()
    //{
    //    return format;
    //}


    //public void setFormat(String format)
    //{
    //    this.format = format;
    //}


    //public String getSuffix()
    //{
    //    return suffix;
    //}


    //public void setSuffix(String suffix)
    //{
    //    this.suffix = suffix;
    //}


    //public String getFileName()
    //{
    //    return fileName;
    //}


    //public void setFileName(String fileName)
    //{
    //    this.fileName = fileName;
    //}


    //public void setFile(String fileName, bool append, bool bufferedIO, int bufferSize)
    //{
    //    if(fileName.endsWith(this.suffix))
    //    {
    //        fileName = fileName.substring(0,fileName.lastIndexOf(this.getFileName()));
    //    }
    //    StringBuilder sbFileName = new StringBuilder();
    //         sbFileName.Append(fileName)
    //         .append(this.getFileName()).append("_").append(DateTime.getDateTimeString())
    //         .append(".").append(this.suffix);	    	
		 
    //     base.SetQWForFiles(sbFileName.ToString(), append, this.bufferedIO, this.bufferSize);
    //  }
}
}
