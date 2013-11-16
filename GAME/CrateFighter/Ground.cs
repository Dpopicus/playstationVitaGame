//\=====================================
//\Author: Harley Laurie / Daniel Popovic
//\Date Created: 21/10/2013
//\Last Edit: 21/10/2013
//\Brief: This is the class used for walls and floors etc
//\Players and enemies alike will be unable to move through these
//\There is also a groundList object which contains a list of
//\references to every ground object that has been created
//\Can be used to iterate through and check for collisions
//\=====================================

using System;
using System.Collections.Generic;	//Needed to use Lists
using Sce.PlayStation.Core;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace CrateFighter
{	
	public class Ground : BaseTerrain
	{
		private SpriteTile groundSprite;	//This grounds sprite
		private Vector2 groundPosition;	//grounds current position in the world
		private Vector2 groundSize;	//The size of this ground object
		
		public Ground ( string imageFile, int xPos, int yPos, int width, int height )
		{
			if ( groundList.instance == null )
				groundList.instance = new groundList();
			this.SetTerrainType(TerrainType.Ground);
			groundSprite = Support.SpriteFromFile(imageFile, width, height, xPos, yPos );	//Create the environment sprite
			groundPosition = new Vector2( xPos, yPos );	//Store the grounds position
			groundSize = new Vector2( width, height );	//Store the grounds size
			Game.Instance.GameScene.AddChild(groundSprite, 1);	//Add the sprite to the scene
			groundList.instance.AddgroundObject(this);	//Add this object to the list of ground objects that have been created
			this.Set ( groundPosition, width, height );
		}
		
		public Ground ( int xPos, int yPos, int width, int height )
		{
			if ( groundList.instance == null )
				groundList.instance = new groundList();
			this.SetTerrainType(TerrainType.Ground);
			groundPosition = new Vector2( xPos, yPos );
			groundSize = new Vector2( width, height );
			groundList.instance.AddgroundObject(this);
			this.Set( groundPosition, width, height );
		}
		
		public Vector2 GetSize()
		{
			return groundSize;
		}
		
		public Vector2 GetPosition()
		{
			return groundPosition;
		}
	}
	
	public class groundList
	{
		public static groundList instance;	//Singleton instance of this groundList class
		public List<Ground> groundObjects;	//The list of ground objects that have been created
		public int objectCounter;	//How many ground objects have been loaded in so far
		
		public groundList()
		{//Instantiates the class
			groundObjects = new List<Ground>();
			instance = this;
			objectCounter = 0;
		}
		
		public void AddgroundObject( Ground newObject )
		{//Adds new ground objects to the list
			groundObjects.Add(newObject);
			objectCounter++;
		}
	}
}

