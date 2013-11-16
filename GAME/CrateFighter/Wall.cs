//\=====================================
//\Author: Harley Laurie / Daniel Popovic
//\Date Created: 21/10/2013
//\Last Edit: 21/10/2013
//\Brief: This is the class used for walls and floors etc
//\Players and enemies alike will be unable to move through these
//\There is also a wallList object which contains a list of
//\references to every wall object that has been created
//\Can be used to iterate through and check for collisions
//\=====================================

using System;
using System.Collections.Generic;	//Needed to use Lists
using Sce.PlayStation.Core;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace CrateFighter
{	
	public class Wall : BaseTerrain
	{
		private SpriteTile wallSprite;	//This walls sprite
		private Vector2 wallPosition;	//walls current position in the world
		private Vector2 wallSize;	//The size of this wall object
		
		public Wall ( string imageFile, int xPos, int yPos, int width, int height )
		{
			if ( wallList.instance == null )
				wallList.instance = new wallList();
			this.SetTerrainType(TerrainType.Wall);
			wallSprite = Support.SpriteFromFile(imageFile, width, height, xPos, yPos );	//Create the environment sprite
			wallPosition = new Vector2( xPos, yPos );	//Store the walls position
			wallSize = new Vector2( width, height );	//Store the walls size
			Game.Instance.GameScene.AddChild(wallSprite, 1);	//Add the sprite to the scene
			wallList.instance.AddwallObject(this);	//Add this object to the list of wall objects that have been created
			this.Set ( wallPosition, width, height );
		}
		
		public Wall ( int xPos, int yPos, int width, int height )
		{
			if ( wallList.instance == null )
				wallList.instance = new wallList();
			this.SetTerrainType(TerrainType.Wall);
			wallPosition = new Vector2( xPos, yPos );
			wallSize = new Vector2( width, height );
			wallList.instance.AddwallObject(this);
			this.Set (wallPosition, width, height );
		}
		
		public Vector2 GetSize()
		{
			return wallSize;
		}
		
		public Vector2 GetPosition()
		{
			return wallPosition;
		}
	}
	
	public class wallList
	{
		public static wallList instance;	//Singleton instance of this wallList class
		public List<Wall> wallObjects;	//The list of wall objects that have been created
		public int objectCounter;	//How many wall objects have been loaded in so far
		
		public wallList()
		{//Instantiates the class
			wallObjects = new List<Wall>();
			instance = this;
			objectCounter = 0;
		}
		
		public void AddwallObject( Wall newObject )
		{//Adds new wall objects to the list
			wallObjects.Add(newObject);
			objectCounter++;
		}
	}
}

