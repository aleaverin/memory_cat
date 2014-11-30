using UnityEngine;
using System.Collections;

public class UIPanelHolder : MonoBehaviour 
{
	[SerializeField]
	private RectTransform rectTransform;
	[SerializeField]
	private PanelHolder gridPanel;

	// Use this for initialization
	void Awake () {
		gridPanel.OnOrientationChange += ChangeAnchor;
	}

	void ChangeAnchor()
	{
		Vector2 gridAnchor = gridPanel.GetAnchorMax();
		rectTransform.anchorMin = new Vector2 (gridAnchor.x==1?0:gridAnchor.x,
		                                       gridAnchor.y==1?0:gridAnchor.y);
	}

	void OnDestroy()
	{
		gridPanel.OnOrientationChange -= ChangeAnchor;
	}
}
