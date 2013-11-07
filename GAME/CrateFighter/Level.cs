//\=====================================
//\Author: Harley Laurie
//\Date Created: 21/10/2013
//\Last Edit: 21/10/2013
//\Brief: Class which loads in all information
//\about a level from xml sheet (created by tiled)
//\and sets it all up for playing
//\=====================================

using System;
using System.Collections;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using System.Xml.Linq;
using System.IO;

namespace CrateFighter
{
	public class Level
	{
		
		//Level start and end
		private Vector2 spawnPosition;
		private boxCollider levelFinish;	//When the player collides with this box, the level is complete
		
		//Size of the level
		private Vector2 levelSize;	//width x height of the tilemap
		private int tileSize;	//The size of tiles on the map (in pixels)
		
		private List<Tile> usedTiles;	//This is a list of each type of tile used in the level
		private List<List<Tile>> levelTiles;	//2d array which holds all the information of the tiles in the level
		
		public Level ()
		{
		}
		
		public void LoadLevel( int levelNumber )
		{//If 1 is passed in, loads the first level, 2 loads second level etc.
			switch(levelNumber)
			{
			case 1:
				usedTiles = new List<Tile>();	//create a list for holding information about the different types of tiles that will be used
				levelTiles = new List<List<Tile>>();	//create the 2d array of level tiles
				
				FileStream fileStream = File.Open( ("/Application/assets/levels/levelOne.xml"), FileMode.Open, FileAccess.Read);
				StreamReader fileStreamReader = new StreamReader(fileStream);
				string xml = fileStreamReader.ReadToEnd ();
				fileStreamReader.Close ();
				fileStream.Close ();
				XDocument doc = XDocument.Parse (xml);
				//\=========================
				//\This case in the switch statement will later end here, and pass the doc to another function which reads in the information
				//\The switch statement will just load in the correct document for the corresponding level, and pass it onto the function which
				//\reads in the data
				
				tileSize = int.Parse (doc.Root.Attribute ("tilewidth").Value);//Read in the size of tiles (in pixels)
				levelSize = new Vector2 ( ( int.Parse (doc.Root.Attribute ("width").Value)), (int.Parse (doc.Root.Attribute ("height").Value)));	//Read in the width / height of the tile map
				
				//After the size of the level has been read in, we want to set up the 2D array so it has enough memory to hold all the required information
				for ( int i = 0; i < levelSize.X; i++ )
				{//We just want to set up the first dimension of the 2D array, so it holds enough lists
					//These lists will be populated further down
					levelTiles.Add(new List<Tile>());
				}
				
				foreach ( var tileSet in doc.Root.Elements("tileset"))
				{//Loop through and read the information about the tiles that are used in this level
					
					int firstGid = int.Parse (tileSet.Attribute("firstgid").Value);//Get the tiles ID
					string imageName = tileSet.Attribute ("name").Value;//Get the tiles name
					Tile tl = new Tile( firstGid, imageName );//Create a tile class which stores this data
					usedTiles.Add (tl);	//Add this to the list of used tiles so we can use it later
				}
				
				//Next we are going to read all the information in the background layer, so we know what to draw for the level later on
				int currentRow = 0;//Create a couple variables to help us keep track of which part of the level we are reading in
				int currentColumn = 0;
				var backgroundLayer = doc.Root.Element("layer").Element("data");//Get the element which contains this data
				foreach ( var tile in backgroundLayer.Elements("tile"))
				{//Loop through all the tile elements in here
					
					Tile tl = new Tile();
					for ( int i = 0; i < usedTiles.Count; i++ )
					{//We need to read in the ID of this tile, and figure out what type of image it has etc.
						if ( int.Parse(tile.Attribute("gid").Value) == usedTiles[i].tileID )
						{
							tl.imageName = usedTiles[i].imageName;//Copy over the image name that is used
							break;//Exit the for loop now that we got the data
						}
					}
					
					Vector2 tilePos = new Vector2();//Figure out the world position of where this tile will be drawn
					tilePos.X = currentColumn * tileSize;
					tilePos.Y = currentRow * tileSize;
					tl.position = tilePos;
					
					levelTiles[currentColumn].Add (tl);//Add this tile to the level
					
					//Now just figure out where we are up to in adding the tiles for the level
					currentColumn++;
					if ( currentColumn > levelSize.X )
					{
						currentRow++;
						currentColumn = 0;
					}
				}
				
				break;
			case 2:
				
				
				break;
			case 3:
				
				
				break;
			}
		}
	}
}

