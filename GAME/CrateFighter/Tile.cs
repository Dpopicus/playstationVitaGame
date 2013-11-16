//\=====================================
//\Author: Harley Laurie
//\Date Created: 21/10/2013
//\Last Edit: 21/10/2013
//\Brief: Holds the information about a certain
//\tile type that will be used by Level.cs
//\Level.cs contains a 2d array of grid squares,
//\which make up the appearance of the level.
//\=====================================

using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace CrateFighter
{
	public class Tile
	{
		public SpriteTile tileImage;
		public int tileID { get; set; }
		public int tileSize { get; set; }
		public Vector2 position { get; set; }
		public string imageName { get; set; }
		
		public Tile()
		{
		}
		
		public void SetValues( Vector2 a_v2Pos, string a_sImageName )
		{
			position = new Vector2();
			position = a_v2Pos;
			imageName = a_sImageName;
		}
		
		public void Activate()
		{//Creates a sprite for this tile, moves it to the correct position and adds it to the scene
			if ( imageName != null )
				tileImage = Support.SpriteFromFile(("Application/assets/levels/" + imageName + ".jpg"), tileSize, tileSize, position.X, position.Y );
			else
				return;
			Game.Instance.GameScene.AddChild(tileImage, -1);
		}
	}
}

