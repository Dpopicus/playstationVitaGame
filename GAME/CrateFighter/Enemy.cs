using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace CrateFighter
{
	enum BehavioralState
	{
		state_Follow = 1,// when the enemy sees the player and moves towards them
		state_Patrol = 2,
		state_Attack = 3,
		state_Stunned = 4
	};
	public class Enemy : boxCollider
	{
		public Vector2 enemyPosition;	//Players current position in the world
		private Vector2 enemySize;	//width and height of the player
		
		//private Vector2 previousPosition;
		
		private float CurrentFallSpeed;
		private float NormalMovementSpeed;
		private int enemyWidth;
		private int enemyHeight;
		public int health;
		private Vector2 PlayerPosition;
		
		private bool facingRight;
		
		Animation CurrentAnimation;
		Animation IdleAnimation;
		Animation WalkAnimation;
		Animation KickAnimation;
		Animation StunnedAnimation;
		
		BehavioralState CurrentBehavioralState;
		
		
		public bool TouchingRight;
		public bool TouchingLeft;
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
			enemySize.X = 140;
			enemySize.Y = 140;
			enemyWidth = 140;
			enemyHeight = 140;
			TouchingLeft = false;
			TouchingRight =false;
			OnScreen = false;
			CurrentFallSpeed = - 5.0f;
			EnemyList.instance.AddEnemyObject(this);
			NormalMovementSpeed = 9.0f;
			MoveLeft = false;
			MoveRight = false;
			health = 100;
			facingRight = false;
			
			//previousPosition = enemyPosition;
			
			IdleAnimation = new Animation();
			IdleAnimation.LoadAnimation("catIdle");
			IdleAnimation.Move ( enemyPosition );
			IdleAnimation.Resize( enemyWidth, enemyHeight );
			
			WalkAnimation = new Animation();
			WalkAnimation.LoadAnimation("SamuraiRun");
			WalkAnimation.Move ( enemyPosition );
			WalkAnimation.Resize( enemyWidth, enemyHeight );
			WalkAnimation.SetView( false );
			
			KickAnimation = new Animation();
			KickAnimation.LoadAnimation("CatKick");
			KickAnimation.Move ( enemyPosition );
			KickAnimation.Resize( enemyWidth, enemyHeight );
			KickAnimation.SetView( false );
			
			StunnedAnimation = new Animation();
			StunnedAnimation.LoadAnimation("CatStunned");
			StunnedAnimation.Move ( enemyPosition );
			StunnedAnimation.Resize( enemyWidth, enemyHeight );
			StunnedAnimation.SetView( false );
			
			CurrentAnimation = IdleAnimation;
			CurrentBehavioralState = BehavioralState.state_Patrol;
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
			CurrentAnimation.Move(enemyPosition);
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
			
			CurrentAnimation.Move(enemyPosition);
			Gravity();	
			if(health <= 0)
			{
				enemyPosition.X = 100;
				enemyPosition.Y = 100;
				health = 100;
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
				if(!TouchingRight)
				{
					if( (enemyPosition.X + enemyWidth  ) < (PlayerPosition.X))
					{
						MoveRight = true;
						facingRight = false;
					}
				}
				if(!TouchingLeft)
				{
					if( enemyPosition.X > (PlayerPosition.X + 10))
					{
						facingRight = true;
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
			switch(CurrentBehavioralState) //sets the behavior and the animation for the enemy
			{
			case BehavioralState.state_Patrol:
				if(!OnScreen)
				{
					CurrentAnimation.SetView(false);
					CurrentAnimation = IdleAnimation;
					CurrentAnimation.SetView(true);
				}
				if(OnScreen)
				{
					CurrentBehavioralState = BehavioralState.state_Follow;
				}
				break;
			case BehavioralState.state_Follow:
				if(!OnScreen )
				{
					CurrentBehavioralState = BehavioralState.state_Patrol;
				}
				if(OnScreen)
				{
					//general follow code
					CurrentAnimation.SetView(false);
					CurrentAnimation = WalkAnimation;
					CurrentAnimation.SetView(true);
					if(TouchingLeft)
					{
						CurrentBehavioralState = BehavioralState.state_Attack;
					}
					if(TouchingRight)
					{
						CurrentBehavioralState = BehavioralState.state_Attack;
					}
					if (health == 50)
					{
						CurrentBehavioralState = BehavioralState.state_Stunned;
					}
				}
				break;
			case BehavioralState.state_Attack:
				/*if(TouchingLeft )
				{
					CurrentAnimation.SetView(false);
					CurrentAnimation = KickAnimation;
					CurrentAnimation.SetView(true);
				}
				if(TouchingRight )
				{
					CurrentAnimation.SetView(false);
					CurrentAnimation = KickAnimation;
					CurrentAnimation.SetView(true);
				}*/
				if(OnScreen )
				{
					//if moveright = true faceright
					CurrentAnimation.SetView(false);
					CurrentAnimation = KickAnimation;
					CurrentAnimation.SetView(true);
					if (health == 50)
					{
						CurrentBehavioralState = BehavioralState.state_Stunned;
					}
				}
				
				else
				{
					CurrentBehavioralState = BehavioralState.state_Follow;
				}
			break;
			case BehavioralState.state_Stunned:
				if (health == 50)
				{
					CurrentAnimation.SetView(false);
					CurrentAnimation = StunnedAnimation;
					CurrentAnimation.SetView(true);	
				}
				else
				{
					CurrentBehavioralState = BehavioralState.state_Patrol;
				}
			break;
			}			
			CurrentAnimation.FaceRight( facingRight );
			CurrentAnimation.Play();
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

