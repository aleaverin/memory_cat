using UnityEngine;
using System;
using System.Collections;

public class Missions : MonoBehaviour 
{
	[SerializeField]
	private Mission[] missions;
	[Serializable]
	private class Mission
	{
		[SerializeField]
		private MissionTypes type;
		[SerializeField]
		private bool less;
		[SerializeField]
		private int N;
		[SerializeField]
		private string text;
		[SerializeField]
		private string engText;
	}
	enum MissionTypes
	{
		Clicks,
		Times,
		Matches,
		Top,
		Days
	}
}
