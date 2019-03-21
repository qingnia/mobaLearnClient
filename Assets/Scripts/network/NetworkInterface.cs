
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/

/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using Google.ProtocolBuffers;
using MyLib;

namespace KBEngine
{
  	using UnityEngine; 
	using System; 
	using System.Net.Sockets; 
	using System.Net; 
	using System.Collections; 
	using System.Collections.Generic;
	using System.Text;
	using System.Threading;


	using MessageModuleID = System.SByte;
	using MessageID = System.UInt16;
	using MessageLength = System.UInt32;

	public delegate void MessageHandler(Packet msg);


} 
