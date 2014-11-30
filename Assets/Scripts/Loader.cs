using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.IO;
using UnityEngine.UI;

public class Loader : MonoBehaviour 
{
	[SerializeField]
	private Vector2 gridSize;

	[SerializeField]
	private RectTransform panel;
	private GridLayoutGroup gridLayout;

	bool portraitValue;
	bool portrait
	{
		get { return portraitValue; }
		set
		{
			if (portraitValue != value)
			{
				portraitValue = value;
				OnOrientationChange();
			}
		}
	}

	void Start()
	{
		portrait = Screen.orientation == ScreenOrientation.Portrait;
	}

	void Update()
	{
		UpdateOrientation();
	}

	public void UpdateOrientation()
	{
		if (portrait != (Screen.orientation == ScreenOrientation.Portrait))
		{
			portrait = !portrait;
		}
	}

	void OnOrientationChange()
	{
		float min = portrait?Screen.width:Screen.height;
		float max = portrait?Screen.height:Screen.width;
		Vector2 v2 = portrait ? new Vector2 (5 * min, 6 * min) : new Vector2 (6 * min, 5 * min);
		//panel
	}


	public void Awake()
	{
		sprites = Resources.LoadAll<Sprite>(currentResourcePath);

		Vector2 gridRatio = new Vector2 (frame.x / gridSize.x, frame.y / gridSize.y);
		float imageScale = Mathf.Min (gridRatio.x, gridRatio.y);
		Vector2 panelSize = new Vector2 (imageScale * gridSize.x, imageScale * gridSize.y);
		Vector2 panelOffset = new Vector2 ((frame.x - panelSize.x) / 2 / frame.x,
		                                   (frame.y - panelSize.y) / 2 / frame.y);
		Vector2 panelBaseAnchor = new Vector2 (imageScale / frame.x, imageScale / frame.y);

		Grid grid = new Grid ((int)gridSize.x, (int)gridSize.y);
		cells = new Image[(int)gridSize.x, (int)gridSize.y];
		int[,] origin = grid.GetAll ();
		for (int i  = 0; i < gridSize.x; i++)
		{
			for (int j  = 0; j < gridSize.y; j++)
			{
				GameObject go = new GameObject ();
				go.name = "["+i+","+j+"]";
				go.transform.SetParent(panel.transform);
				Image image = go.AddComponent<Image> ();
				int x = i;
				int y = j;
				go.AddComponent<Button>().onClick.AddListener(()=>{ 
					grid.Open(x, y); 
				});
				//image.rectTransform.anchoredPosition = Vector2.zero;
				//image.rectTransform.anchorMax = new Vector2(panelOffset.x + (i+0.5f)*panelBaseAnchor.x,
				//                                            panelOffset.y + (j+0.5f)*panelBaseAnchor.y);
				//image.rectTransform.anchorMin = image.rectTransform.anchorMax;
				cells[i,j]=image;

				image.sprite = sprites[origin[i,j]];
				StartCoroutine(Wait(2f,()=>{image.sprite = baseSprite;}));
			}
		}
		grid.OnClose += CloseHandler;
		grid.OnOpen += OpenHandler;
	}




	[SerializeField]
	private string currentResourcePath;
	[SerializeField]
	private Sprite baseSprite;
	
	
	private Sprite[] sprites;
	
	private Image[,] cells;
	
	private Vector2? nullableFrame = null; 
	private Vector2 frame
	{
		get 
		{
			if (nullableFrame == null)
			{
				nullableFrame = new Vector2(Screen.width, Screen.height);
			}
			return nullableFrame??Vector2.zero;
		}
	}

	private void OpenHandler(int i, int j, int spriteIndex)
	{
		cells [i, j].sprite = sprites[spriteIndex];
	}
	private void CloseHandler(int i, int j)
	{
		StartCoroutine(Wait(0.5f,()=>{cells [i, j].sprite = baseSprite;}));
	}
	IEnumerator Wait(float second, Action action)
	{
		yield return new WaitForSeconds (second);
		action();
	}
}
