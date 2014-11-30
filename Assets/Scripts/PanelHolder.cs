using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class PanelHolder : MonoBehaviour 
{
	[SerializeField]
	private GridLayoutGroup grid;

	const int perfectElementSize = 256;
	const int minElementPadding = 3;

	private int additionOffset = 0;

	[SerializeField]
	private RectTransform rectTransform;

	private int minGridSize = 5;
	private int maxGridSize = 6;
	[SerializeField]
	private int padding;
	
	private int screenWidht { get { return Screen.width; } }
	private int screenHeigth { get { return Screen.height; } }

	Transform cachedTransform;

	void Awake()
	{
		cachedTransform = transform;
	}

	float GetAnchorRatio(int screenMinSize, int screenMaxSize)
	{
		Debug.LogWarning (screenMinSize + " : " + screenMaxSize);
		int panelMinSize = (int)(screenMinSize - 2 * padding);
		int elementPadding = 0;
		int elementSideSize = panelMinSize / minGridSize;
		float additionOffsetSumm;
		if (elementSideSize > perfectElementSize)
		{
			elementSideSize = perfectElementSize;
			int elementPaddingSum = (panelMinSize - minGridSize * elementSideSize);
			elementPadding = (int)(elementPaddingSum / (minGridSize - 1));
			additionOffset = (elementPaddingSum - elementPadding * (minGridSize - 1));
		}
		else
		{
			elementPadding = minElementPadding;
			elementSideSize = (int)(panelMinSize - elementPadding * (minGridSize - 1)) / minGridSize;
			additionOffset = (panelMinSize - ((elementPadding + elementSideSize) * minGridSize - elementPadding));
		}

		int panelMaxSize = (elementSideSize + elementPadding) * maxGridSize - elementPadding + 2*padding;
		grid.spacing = new Vector2(elementPadding,elementPadding);
		grid.cellSize = new Vector2 (elementSideSize, elementSideSize);
		return (float)panelMaxSize / screenMaxSize;
	}



	void SetAnchors()
	{

		if (portrait)
		{
			rectTransform.anchorMax = new Vector2(1, GetAnchorRatio(screenWidht, screenHeigth));
			int halfAdditionOffset = additionOffset / 2;
			int asimmetricPixel = additionOffset % 2;
			grid.padding.left = padding + halfAdditionOffset;
			grid.padding.right = padding + halfAdditionOffset + asimmetricPixel;
			grid.padding.top = padding;
			grid.padding.bottom = padding;
		}
		else
		{
			rectTransform.anchorMax = new Vector2(GetAnchorRatio(screenHeigth, screenWidht), 1);
			int halfAdditionOffset = additionOffset / 2;
			int asimmetricPixel = additionOffset % 2;
			grid.padding.top = padding + halfAdditionOffset;
			grid.padding.bottom = padding + halfAdditionOffset + asimmetricPixel;
			grid.padding.left = padding;
			grid.padding.right = padding;
		}
	}

	bool? portraitValue = null;
	bool portrait
	{
		get { return portraitValue??(isPortraitOrientation); }
		set
		{
			if (portraitValue != value)
			{
				portraitValue = value;
				OrientationChange();
			}
		}
	}
	bool isPortraitOrientation
	{
		get 
		{
			if (Application.isEditor)
			{
				return Screen.width < Screen.height;
			}
			else
			{
				return Screen.orientation == ScreenOrientation.Portrait;
			}
		}
	}
	void Start()
	{
		Debug.LogError (Screen.orientation);
		portrait = isPortraitOrientation;
	}
	
	void Update()
	{
		UpdateOrientation();
	}
	
	public void UpdateOrientation()
	{
		if (portrait != isPortraitOrientation)
		{
			portrait = !portrait;
		}
	}
	public event Action OnOrientationChange;
	void OrientationChange()
	{
		SetAnchors();
		if (OnOrientationChange!=null)
		{
			OnOrientationChange();
		}
	}
	public Vector2 GetAnchorMax() 
	{
		return rectTransform.anchorMax;
	}
}
