using UnityEngine;
using System.Collections;

namespace GameMaker
{
	[AddComponentMenu("GameMaker/GameObject/Dont Destroy")]
	public class DontDestroyOnLoad : MonoBehaviour 
	{
		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}