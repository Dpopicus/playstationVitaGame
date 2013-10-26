//\=====================================
//\Author: Harley Laurie / Daniel Popovic
//\Date Created: 21/10/2013
//\Last Edit: 21/10/2013
//\Brief: Player class
//\=====================================

using System;

using Sce.PlayStation.Core;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

using Sce.PlayStation.Core.Input;	//used for polling for gamepad input

namespace CrateFighter
{
	public class Player
	{
		private SpriteTile playerSprite;//Player sprite
		private Vector2 playerPosition;	//Players current position in the world
		private Vector2 playerSize;	//width and height of the player
		private GamePadData PadData;	//A data structure containing the status of all buttons on the gamepad
		private float NormalMovementSpeed;	//The normal walking speed for the player
		private float CurrentFallSpeed; //players current fall speed
		
		private bool Falling;
		
		
		private bool moveLeft;
		private bool moveRight;
		private bool Jump;
		
		public Player ()
		{
			playerSize.X = 15;	//Set the players width
			playerSize.Y = 15;	//Set the players height
			playerPosition.X = 100;	//Set the players initial x position
			playerPosition.Y = 100;	//Set the players initial y position
			//playerSprite = Support.SpriteFromFile("Application/assets/playerPlaceholder.png", playerSize.X, playerSize.Y, playerPosition.X, playerPosition.Y);	//Create the player sprite
			playerSprite = Support.TiledSpriteFromFile( "Application/assets/playerPlaceholder.png",1 ,1 ); // image name and the fraction of the base image that each frame will take up
			NormalMovementSpeed = 10.0f;

			CurrentFallSpeed = - 5.0f;
			Game.Instance.GameScene.AddChild(playerSprite, 2);	//Add this sprite as a child to the game scene
		}
		
		public Vector2 GetSize()
		{
			return playerSize;
		}
		
		public void MovePlayer( float xPos, float yPos )
		{
			playerPosition.X = xPos;
			playerPosition.Y = yPos;
			playerSprite.Quad.T = playerPosition;
		}
		
		public void UpdatePosition()
		{
			if (moveLeft)
				playerPosition.X -= NormalMovementSpeed;
			if (moveRight)
				playerPosition.X += NormalMovementSpeed;
			if (Falling)
				playerPosition.Y += CurrentFallSpeed;
			
			playerSprite.Quad.T = playerPosition;
			Gravity();
			
		}
		
		public Vector2 GetPosition()
		{
			return playerPosition;
		}
		
		public void GetInput()
		{
			//This function is called 60 times per second
			PadData = GamePad.GetData (0);	//Update the gamepad input
			Falling = true; 
			
			if((PadData.AnalogLeftX > 0.0f ) || ((PadData.Buttons & GamePadButtons.Right) != 0))
				moveRight = true;
			else
				moveRight = false;
			
			if((PadData.AnalogLeftX < 0.0f ) || ((PadData.Buttons & GamePadButtons.Left) != 0))
				moveLeft = true;
			else
				moveLeft = false;
			
			if((PadData.Buttons & GamePadButtons.Cross) != 0)
				Jump = true;
			else
				Jump = false;
		}
		
		public void CheckEnvironmentCollisions()
		{
			if ( TerrainList.instance != null )
			{
				for ( int i = 0; i < TerrainList.instance.objectCounter; i++ )
				{
					
					if ( moveRight)
					{//Only check collisions with stuff if we are actually trying to move into it
						if (!( playerPosition.X >= ( TerrainList.instance.terrainObjects[i].GetPosition().X + TerrainList.instance.terrainObjects[i].GetSize().X ) ))
						{//First make sure the player isn't already to the right of the object we are checking collision for
							if ( playerPosition.X + playerSize.X >= TerrainList.instance.terrainObjects[i].GetPosition().X )
							{
								//if we get here in here we need to make sure the player is interacting with the
								//terrain object, anywhere on the y axis
								if (( playerPosition.Y + playerSize.Y ) > TerrainList.instance.terrainObjects[i].GetPosition().Y )
								{
									if ( playerPosition.Y < ( TerrainList.instance.terrainObjects[i].GetPosition().Y + TerrainList.instance.terrainObjects[i].GetSize().Y ) )
									{
										moveRight = false;
									}
								}
							}
						}
					}
					
					if ( moveLeft )
					{
						if (!( playerPosition.X <= TerrainList.instance.terrainObjects[i].GetPosition().X ))
						{//First make sure the player isn't already to the right of the object we are checking collision for
							// didnt need the + movement speed stuff dude
							if ( playerPosition.X <= ( TerrainList.instance.terrainObjects[i].GetPosition().X + TerrainList.instance.terrainObjects[i].GetSize().X )  )
							{
								//if we get here in here we need to make sure the player is interacting with the
								//terrain object, anywhere on the y axis
								if (( playerPosition.Y + playerSize.Y ) > TerrainList.instance.terrainObjects[i].GetPosition().Y )
								{
									if ( playerPosition.Y < ( TerrainList.instance.terrainObjects[i].GetPosition().Y + TerrainList.instance.terrainObjects[i].GetSize().Y ) )
									{
										moveLeft = false;
									}
								}
							}
						}
					}
					if ( Falling )
					{
						if (!( playerPosition.Y <= TerrainList.instance.terrainObjects[i].GetPosition().Y ))
						{//First make sure the player isn't already below the object we are checking collision for
							if ( (playerPosition.Y + CurrentFallSpeed)  <= ( TerrainList.instance.terrainObjects[i].GetPosition().Y + TerrainList.instance.terrainObjects[i].GetSize().Y )  )
							{
								//if we get here in here we need to make sure the player is interacting with the
							//terrain object, anywhere on the x axis
								if (( playerPosition.X + playerSize.X ) > TerrainList.instance.terrainObjects[i].GetPosition().X )
								{
									if ( playerPosition.X < ( TerrainList.instance.terrainObjects[i].GetPosition().X + TerrainList.instance.terrainObjects[i].GetSize().X ) )
									{
										Falling = false;
										playerPosition.Y = ( TerrainList.instance.terrainObjects[i].GetPosition().Y + TerrainList.instance.terrainObjects[i].GetSize().Y); //incase the player was going to stop shy of the ground 
									}
								}
							}
						}
					}
					if ( Jump )
					{
						if (!Falling)
						{
							CurrentFallSpeed = 15.0f;
							++playerPosition.Y;
						}
					}
					
				}
			}
			
			//Enemy
			//Collisions
			if ( EnemyList.instance != null )
			{
				for ( int i = 0; i < EnemyList.instance.objectCounter; i++ )
				{
					
					if ( moveRight)
					{//Only check collisions with stuff if we are actually trying to move into it
						if (!( playerPosition.X >= ( EnemyList.instance.enemyObjects[i].GetPosition().X + EnemyList.instance.enemyObjects[i].GetSize().X ) ))
						{//First make sure the player isn't already to the right of the object we are checking collision for
							if ( playerPosition.X + playerSize.X >= EnemyList.instance.enemyObjects[i].GetPosition().X )
							{
								//if we get here in here we need to make sure the player is interacting with the
								//terrain object, anywhere on the y axis
								if (( playerPosition.Y + playerSize.Y ) > EnemyList.instance.enemyObjects[i].GetPosition().Y )
								{
									if ( playerPosition.Y < ( EnemyList.instance.enemyObjects[i].GetPosition().Y + EnemyList.instance.enemyObjects[i].GetSize().Y ) )
									{
										moveRight = false;
									}
								}
							}
						}
					}
					
					if ( moveLeft )
					{
						if (!( playerPosition.X <= EnemyList.instance.enemyObjects[i].GetPosition().X ))
						{//First make sure the player isn't already to the right of the object we are checking collision for
							// didnt need the + movement speed stuff dude
							if ( playerPosition.X <= ( EnemyList.instance.enemyObjects[i].GetPosition().X + EnemyList.instance.enemyObjects[i].GetSize().X )  )
							{
								//if we get here in here we need to make sure the player is interacting with the
								//terrain object, anywhere on the y axis
								if (( playerPosition.Y + playerSize.Y ) > EnemyList.instance.enemyObjects[i].GetPosition().Y )
								{
									if ( playerPosition.Y < ( EnemyList.instance.enemyObjects[i].GetPosition().Y + EnemyList.instance.enemyObjects[i].GetSize().Y ) )
									{
										moveLeft = false;
									}
								}
							}
						}
					}
					if ( Falling )
					{
						if (!( playerPosition.Y <= EnemyList.instance.enemyObjects[i].GetPosition().Y ))
						{//First make sure the player isn't already below the object we are checking collision for
							if ( (playerPosition.Y + CurrentFallSpeed)  <= ( EnemyList.instance.enemyObjects[i].GetPosition().Y + EnemyList.instance.enemyObjects[i].GetSize().Y )  )
							{
								//if we get here in here we need to make sure the player is interacting with the
							//terrain object, anywhere on the x axis
								if (( playerPosition.X + playerSize.X ) > EnemyList.instance.enemyObjects[i].GetPosition().X )
								{
									if ( playerPosition.X < ( EnemyList.instance.enemyObjects[i].GetPosition().X + EnemyList.instance.enemyObjects[i].GetSize().X ) )
									{
										Falling = false;
										playerPosition.Y = ( EnemyList.instance.enemyObjects[i].GetPosition().Y + EnemyList.instance.enemyObjects[i].GetSize().Y); //incase the player was going to stop shy of the ground 
									}
								}
							}
						}
					}
					if ( Jump )
					{
						if (!Falling)
						{
							CurrentFallSpeed = 15.0f;
							++playerPosition.Y;
						}
					}
					
				}
			}
			UpdatePosition();
		}
		
		public void Update()
		{
			GetInput();
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
}

