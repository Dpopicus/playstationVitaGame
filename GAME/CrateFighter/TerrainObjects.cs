//\=====================================
//\Author: Harley Laurie / Daniel Popovic
//\Date Created: 21/10/2013
//\Last Edit: 21/10/2013
//\Brief: This just holds a list of all the different
//\terrain objects loaded into the game scene
//\=====================================

using System;
using System.Collections.Generic;	//Needed to use Lists
using Sce.PlayStation.Core;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace CrateFighter
{
	public class TerrainObjects
	{
		private static TerrainObjects instance;
		private List<BaseTerrain> objectList;
		
		private TerrainObjects ()
		{
			objectList = new List<BaseTerrain>();
		}
		
		public static TerrainObjects Instance
		{
			get
			{
				if (instance == null)
					instance = new TerrainObjects();
				return instance;
			}
		}
		
		public void NewObject( BaseTerrain obj )
		{
			if ( instance != null )
			{
				objectList.Add(obj);
			}
		}
		
		public List<BaseTerrain> GetObjectList()
		{
			return objectList;
		}
	}
}

