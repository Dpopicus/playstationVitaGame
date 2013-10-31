using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace CrateFighter
{
	public class Enemy : boxCollider
	{
		private SpriteTile enemySprite;//Player sprite
		public Vector2 enemyPosition;	//Players current position in the world
		private Vector2 enemySize;	//width and height of the player
		
		private float CurrentFallSpeed;
		private float NormalMovementSpeed;
		private int enemyWidth;
		private int enemyHeight;
		public int health;
		private Vector2 PlayerPosition;
		
		public bool ToughingRight;
		public bool TouchngLeft;
		public bool OnScreen;
		private bool MoveLeft;
		private bool MoveRight;
		
		private bool Falling;
		
		public Enemy ()
		{
			if ( EnemyList.instance == null )
				EnemyList.instance = new EnemyList();
			enemyPosition.X = 500;
			enemyPosition.Y = 400;
			PlayerPosition.X = 100;
			PlayerPosition.Y = 100;
			enemySize.X = 100;
			enemySize.Y = 100;
			enemyWidth = 100;
			enemyHeight = 100;
			TouchngLeft = false;
			ToughingRight =false;
			enemySprite = Support.TiledSpriteFromFile( "Application/assets/platformPlaceholder.png",1 ,1 );
			OnScreen = false;
			CurrentFallSpeed = - 5.0f;
			Game.Instance.GameScene.AddChild(enemySprite, 2);
			EnemyList.instance.AddEnemyObject(this);
			NormalMovementSpeed = 9.0f;
			MoveLeft = false;
			MoveRight = false;
			health = 100;
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
			if (MoveRight)
				enemyPosition.X += NormalMovementSpeed;
			if (MoveLeft)
				enemyPosition.X -= NormalMovementSpeed;
			
			this.Set ( enemyPosition, enemyWidth, enemyHeight);
			
			enemySprite.Quad.T = enemyPosition;
			Gravity();	
			if(health <= 0)
			{
				enemySprite = Support.TiledSpriteFromFile( "Application/assets/playerPlaceholder.png",1 ,1 );
			}
		}
		
		public void CheckOnScreen()
		{
			if (( enemyPosition.X + enemyWidth ) > (PlayerPosition.X - 240)) // they will only react to the player if they are withing 240 pixels of the player on either side
			{
				if ( enemyPosition.X < ( PlayerPosition.X + 240) )
				{
					OnScreen = true;
				}
				else
					OnScreen = false;
			}
			else
				OnScreen = false;
		}
		
		public void CheckEnvironmentCollisions()
		{
			if ( groundList.instance != null )
			{
				for ( int i = 0; i < groundList.instance.objectCounter; i++ )
				{
					if ( Falling )
					{
						if (!( enemyPosition.Y <= groundList.instance.groundObjects[i].GetPosition().Y ))
						{//First make sure the player isn't already below the object we are checking collision for
							if ( (enemyPosition.Y + CurrentFallSpeed)  <= ( groundList.instance.groundObjects[i].GetPosition().Y + groundList.instance.groundObjects[i].GetSize().Y )  )
							{
								//if we get here in here we need to make sure the player is interacting with the
							//terrain object, anywhere on the x axis
								if (( enemyPosition.X + enemySize.X ) > groundList.instance.groundObjects[i].GetPosition().X )
								{
									if ( enemyPosition.X < ( groundList.instance.groundObjects[i].GetPosition().X + groundList.instance.groundObjects[i].GetSize().X ) )
									{
										Falling = false;
										enemyPosition.Y = ( groundList.instance.groundObjects[i].GetPosition().Y + groundList.instance.groundObjects[i].GetSize().Y); //incase the player was going to stop shy of the ground 
									}
								}
							}
						}
					}
				}
			}
			if( OnScreen  )
			{
				if(!ToughingRight)
				{
					if( (enemyPosition.X + enemyWidth  ) < (PlayerPosition.X))
					{
						MoveRight = true;
					}
				}
				if(!TouchngLeft)
				{
					if( enemyPosition.X > (PlayerPosition.X + 10))
					{
						MoveLeft = true;
					}
				}
			}
			UpdatePosition();
		}
		
		public void Update()
		{
			Falling = true;
			MoveRight = false;
			MoveLeft = false;
			CheckOnScreen();
			CheckEnvironmentCollisions();
		}
		
		public void GetPlayerPos( float xPos, float yPos )
		{
			PlayerPosition.X = xPos;
			PlayerPosition.Y = yPos;
		}
		
		public void Gravity()
		{
			if (CurrentFallSpeed != - 10.0f)
			{
				--CurrentFallSpeed;
			}
		}
		
		public bool GetOnScreen()
		{
			return OnScreen;
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

