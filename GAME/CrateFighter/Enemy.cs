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
		state_Dying = 4
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
		private Vector2 SpawnPoint;
		
		private int attackFrame; // right now constantly ticks when attacking but may be per touching instance if i can think of a better way
		
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
			
			attackFrame = 0;
			
			//previousPosition = enemyPosition;
			
			IdleAnimation = new Animation();
			IdleAnimation.LoadAnimation("Mon1Idle");
			IdleAnimation.Move ( enemyPosition );
			IdleAnimation.Resize( enemyWidth, enemyHeight );
			
			WalkAnimation = new Animation();
			WalkAnimation.LoadAnimation("Mon1Walk");
			WalkAnimation.Move ( enemyPosition );
			WalkAnimation.Resize( enemyWidth, enemyHeight );
			WalkAnimation.SetView( false );
			
			KickAnimation = new Animation();
			KickAnimation.LoadAnimation("Mon1Attack");
			KickAnimation.Move ( enemyPosition );
			KickAnimation.Resize( enemyWidth, enemyHeight );
			KickAnimation.SetView( false );
			
			StunnedAnimation = new Animation();
			StunnedAnimation.LoadAnimation("Mon1Death");
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
		
		public void SetSpawn( float xPos, float yPos ) // a bit superflouse but fuck you
		{
			SpawnPoint.X = xPos;
			SpawnPoint.Y = yPos;
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
		}
		
		public void CheckOnScreen()
		{
			if (( enemyPosition.X + enemyWidth ) > (PlayerPosition.X - 240)) // they will only react to the player if they are within 240 pixels of the player on either side
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
					FollowPlayer();
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
				}
				break;
			case BehavioralState.state_Attack:
				// attack damage handling
				if(OnScreen ) // before we do anything we check if the enemy is still on screen as we need to check  this regardless and to do any other kind of checks before this one would be superflous
				{
					FollowPlayer();
					//if moveright = true faceright
					CurrentAnimation.SetView(false);
					CurrentAnimation = KickAnimation;
					CurrentAnimation.SetView(true);
					
					if (attackFrame == 15) // checks the attack frame before colision because this if statement will only be true every 1/50 frames but the enemy may e constantly touching the player
					{
						if((TouchingLeft )||(TouchingRight ))
						{
							Game.Instance.DamagePlayer(); //have to go through game to damage the player for now because of an error caused by the way i was getting playerinstance
						}
					
					}
					
					++attackFrame; // increment attackframe after checking damage so that we dont skip frames
					if (attackFrame > 15)
					{
						attackFrame = 0;
					}
				}
				
				else
				{
					CurrentBehavioralState = BehavioralState.state_Follow;
				}
			break;
			case BehavioralState.state_Dying:
				if (health < 51)
				{
					CurrentAnimation.SetView(false);
					CurrentAnimation = StunnedAnimation;
					CurrentAnimation.SetView(true);
					if (CurrentAnimation.LastFrame)
					{
						Respawn();
					}
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
		
		public void Respawn()
		{
			enemyPosition.X = SpawnPoint.X;
			enemyPosition.Y = SpawnPoint.Y;
			health = 100;
			StunnedAnimation.LastFrame = false;
		}
		
		public void TakeDamage(int iDamage)
		{
			health -= iDamage; // will add block functionality
			if (CurrentBehavioralState != BehavioralState.state_Dying )// no use setting the state to stunned if it already is
			{
				if (health < 51) // if health is less than 51 enter stunned state
				{
					CurrentBehavioralState = BehavioralState.state_Dying;
				}
			}
			if(health <= 0) // rather than checking this every frame only checks when hit
			{
				Respawn();
			}
		}
		
		public void FollowPlayer()
		{
			//not going to check if onscreen first as this function will only be called when on screen
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

