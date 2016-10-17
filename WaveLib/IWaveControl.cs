using System;

namespace WaveLib
{
	public delegate void ErrorEventHandle(Exception e,string error);
	/// <summary>
	/// IWaveControl 的摘要说明。
	/// </summary>
	public interface IWaveControl : IWaveRecordInfo
	{
		void Stop();
		void Start();		
		event ErrorEventHandle ErrorEvent;
	}
}
