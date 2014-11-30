using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Grid
{
	public event Action<int,int,int> OnOpen;
	public event Action<int,int> OnClose;
	public event Action OnFinish;

	private readonly int[,] origin;
	private int[,] modified;
	//index first opened
	private int x = -1;
	private int y = 0;
	//full opened count
	private int count = 0;

	public Grid(int x, int y)
	{
		origin = new int[x,y];
		modified = new int[x,y];
		List<int> pool = Enumerable.Range(0,x*y/2).ToList();
		pool.AddRange(pool);//x2
		System.Random rndm =  new System.Random();
		int count = pool.Count;
		for (int i = 0; i < count; i++)
		{
			int index = rndm.Next(0, pool.Count - 1);
			//int index = (int)Random.Range(0, pool.Count - 1);
			origin[i%x,i/x] = pool[index];
			modified[i%x,i/x] = pool[index];
			pool.RemoveAt(index);
		}
	}

	public void Open(int x, int y)
	{
		if (modified[x, y] != -1)
		{
			if (OnOpen!=null)
			{
				OnOpen(x, y, modified[x, y]);
			}
			if (this.x == -1)//if first not open
			{//open first
				modified[x, y] = -1;
				this.x = x;
				this.y = y;
			}
			else
			{	//if second = first opened 
				if (origin[this.x,this.y] == origin[x, y])
				{//open second
					modified[x, y] = -1;
					count+=2;
					if (OnFinish!=null && count>=x*y)
					{
						OnFinish();
					}
				}
				else
				{//close first
					modified[this.x,this.y] = origin[this.x,this.y];
					if (OnClose!=null)
					{
						OnClose(x, y);
						OnClose(this.x, this.y);
					}
				}
				//reset first
				this.x = -1;
			}
		}
	}

	public int[,] GetAll()
	{
		return origin;
	}
}
