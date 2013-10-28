//\=====================================
//\Author: Harley Laurie / Daniel Popovic
//\Date Created: 21/10/2013
//\Last Edit: 21/10/2013
//\Brief: Base class that stuff like wall
//\and ground objects derive from
//\=====================================

using System;
using System.Collections.Generic;	//Needed to use Lists
using Sce.PlayStation.Core;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace CrateFighter
{
	public enum TerrainType
	{
		Ground,
		Wall
	}
	
	public class BaseTerrain : boxCollider
	{
		private TerrainType type;
		
		public BaseTerrain ()
		{
			TerrainObjects.Instance.NewObject( this );
		}
		
		public void SetTerrainType( TerrainType newType )
		{
			type = newType;
		}
		
		public TerrainType GetTerrainType()
		{
			return type;
		}
	}
}

