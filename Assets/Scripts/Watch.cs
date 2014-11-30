using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Watch : MonoBehaviour 
{
	public Image image;
	float time = 0;
	void Start()
	{

	}
	void Update () 
	{
		image.rectTransform.Rotate (new Vector3(0,0,- Time.deltaTime * 6));
	}
}
