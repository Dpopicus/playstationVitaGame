//\=====================================
//\Author: Harley Laurie / Daniel Popovic
//\Date Created: 21/10/2013
//\Last Edit: 21/10/2013
//\Brief: This is the class used for walls and floors etc
//\Players and enemies alike will be unable to move through these
//\There is also a TerrainList object which contains a list of
//\references to every Terrain object that has been created
//\Can be used to iterate through and check for collisions
//\=====================================

using System;
using System.Collections.Generic;	//Needed to use Lists
using Sce.PlayStation.Core;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace CrateFighter
{	
	public class Terrain
	{
		private SpriteTile terrainSprite;	//This terrains sprite
		private Vector2 terrainPosition;	//Terrains current position in the world
		private Vector2 terrainSize;	//The size of this terrain object
		
		public Terrain ( string imageFile, int xPos, int yPos, int width, int height )
		{
			if ( TerrainList.instance == null )
				TerrainList.instance = new TerrainList();
			terrainSprite = Support.SpriteFromFile("Application/assets/platformPlaceholder.png", width, height, xPos, yPos );	//Create the environment sprite
			terrainPosition = new Vector2( xPos, yPos );	//Store the terrains position
			terrainSize = new Vector2( width, height );	//Store the terrains size
			Game.Instance.GameScene.AddChild(terrainSprite, 1);	//Add the sprite to the scene
			TerrainList.instance.AddTerrainObject(this);	//Add this object to the list of terrain objects that have been created
		}
		
		public Vector2 GetSize()
		{
			return terrainSize;
		}
		
		public Vector2 GetPosition()
		{
			return terrainPosition;
		}
	}
	
	public class TerrainList
	{
		public static TerrainList instance;	//Singleton instance of this TerrainList class
		public List<Terrain> terrainObjects;	//The list of terrain objects that have been created
		public int objectCounter;	//How many terrain objects have been loaded in so far
		
		public TerrainList()
		{//Instantiates the class
			terrainObjects = new List<Terrain>();
			instance = this;
			objectCounter = 0;
		}
		
		public void AddTerrainObject( Terrain newObject )
		{//Adds new terrain objects to the list
			terrainObjects.Add(newObject);
			objectCounter++;
		}
	}
}

