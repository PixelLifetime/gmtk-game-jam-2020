using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UDebug = UnityEngine.Debug;

namespace GMTKGameJam2020
{
	public class Debugger : MonoBehaviour
	{
		public void Debug(object message) => UDebug.Log(message: message);
	}
}
