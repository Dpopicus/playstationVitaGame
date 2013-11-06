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
				levelTiles = new List<List<Tile>>();	//set aside memory for the levels tile data
				FileStream fileStream = File.Open( ("/Application/assets/levels/levelOne.xml"), FileMode.Open, FileAccess.Read);
				StreamReader fileStreamReader = new StreamReader(fileStream);
				string xml = fileStreamReader.ReadToEnd ();
				fileStreamReader.Close ();
				fileStream.Close ();
				XDocument doc = XDocument.Parse (xml);
				
				tileSize = int.Parse (doc.map.Attribute("tileheight").Value);//Read in the size of tiles
				levelSize = new Vector2( ( int.Parse (doc.map.Attribute("width").Value)), (int.Parse (doc.map.Attribute("height").Value)));//Read in the size of the levels
				
				foreach ( var tileSet in doc.Root.Elements("tileset"))
				{//Loop through and read the information about the tiles that are used in this level
					
					int firstGid = float.Parse (tileSet.Attribute("firstgid").Value);//read in the tile ID
					string imageName = tileSet.Attribute("name").Value;	//Get the name of the image used for this sprite
					
					
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

