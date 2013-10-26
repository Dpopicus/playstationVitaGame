using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace CrateFighter
{
	public class Enemy
	{
		private SpriteTile enemySprite;//Player sprite
		private Vector2 enemyPosition;	//Players current position in the world
		private Vector2 enemySize;	//width and height of the player
		
		private float CurrentFallSpeed;
		
		private bool Falling;
		
		public Enemy ()
		{
			if ( EnemyList.instance == null )
				EnemyList.instance = new EnemyList();
			enemyPosition.X = 200;
			enemyPosition.Y = 400;
			enemySize.X = 15;
			enemySize.Y = 15;
			enemySprite = Support.TiledSpriteFromFile( "Application/assets/platformPlaceholder.png",1 ,1 );
			
			CurrentFallSpeed = - 5.0f;
			Game.Instance.GameScene.AddChild(enemySprite, 2);
			EnemyList.instance.AddEnemyObject(this);
		}
		
		public Vector2 GetSize()
		{
			return enemySize;
		}
		
		public Vector2 GetPosition()
		{
			return enemyPosition;
		}
		
		public void MoveEnemy( float xPos, float yPos )
		{
			enemyPosition.X = xPos;
			enemyPosition.Y = yPos;
			enemySprite.Quad.T = enemyPosition;
		}
		
		public void UpdatePosition()
		{
			if (Falling)
				enemyPosition.Y += CurrentFallSpeed;
			
			enemySprite.Quad.T = enemyPosition;
			Gravity();	
		}
		
		public void CheckEnvironmentCollisions()
		{
			if ( TerrainList.instance != null )
			{
				for ( int i = 0; i < TerrainList.instance.objectCounter; i++ )
				{
					if ( Falling )
					{
						if (!( enemyPosition.Y <= TerrainList.instance.terrainObjects[i].GetPosition().Y ))
						{//First make sure the player isn't already below the object we are checking collision for
							if ( (enemyPosition.Y + CurrentFallSpeed)  <= ( TerrainList.instance.terrainObjects[i].GetPosition().Y + TerrainList.instance.terrainObjects[i].GetSize().Y )  )
							{
								//if we get here in here we need to make sure the player is interacting with the
							//terrain object, anywhere on the x axis
								if (( enemyPosition.X + enemySize.X ) > TerrainList.instance.terrainObjects[i].GetPosition().X )
								{
									if ( enemyPosition.X < ( TerrainList.instance.terrainObjects[i].GetPosition().X + TerrainList.instance.terrainObjects[i].GetSize().X ) )
									{
										Falling = false;
										enemyPosition.Y = ( TerrainList.instance.terrainObjects[i].GetPosition().Y + TerrainList.instance.terrainObjects[i].GetSize().Y); //incase the player was going to stop shy of the ground 
									}
								}
							}
						}
					}
				}
			}
			UpdatePosition();
		}
		
		public void Update()
		{
			Falling = true;
			CheckEnvironmentCollisions();
		}
		public void Gravity()
		{
			if (CurrentFallSpeed != - 10.0f)
			{
				--CurrentFallSpeed;
			}
		}
		
	}
	public class EnemyList
	{
		public static EnemyList instance;	//Singleton instance of this TerrainList class
		public List<Enemy> enemyObjects;	//The list of terrain objects that have been created
		public int objectCounter;	//How many terrain objects have been loaded in so far
		
		public EnemyList()
		{//Instantiates the class
			enemyObjects = new List<Enemy>();
			instance = this;
			objectCounter = 0;
		}
		
		public void AddEnemyObject( Enemy newObject )
		{//Adds new terrain objects to the list
			enemyObjects.Add(newObject);
			objectCounter++;
		}
	}
}

