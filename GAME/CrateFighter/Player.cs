//\=====================================
//\Author: Harley Laurie / Daniel Popovic
//\Date Created: 21/10/2013
//\Last Edit: 21/10/2013
//\Brief: Player class
//\=====================================

using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

using Sce.PlayStation.Core.Input;	//used for polling for gamepad input

namespace CrateFighter
{
	enum AnimationState
	{
		Idle = 1,
		Walk = 2
	}
	
	public class Player : boxCollider
	{
		public SoundPlayer soundPlayerBullet;
		Sound jumpOneSound;
		Sound jumpTwoSound;
		Sound jumpThreeSound;
		Sound jumpFourSound;
		
		private Vector2 attackPosition;
		private int attackWidth;
		private int attackHeight;
		
		private boxCollider Attack;
		
		private bool facingRight; //This is used to determine whether or not to flip the player sprites
		private Vector2 previousPosition; //Compares current position with previous position to see which
											//direction the player is moving
		private Vector2 playerPosition;	//Players current position in the world
		private int playerWidth;
		private int playerHeight;
		private GamePadData PadData;	//A data structure containing the status of all buttons on the gamepad
		
		private float CurrentMovementSpeed;
		private float NormalMovementSpeed;	//The normal walking speed for the player
		private float SprintingMovementSpeed;
		
		AnimationState CurrentAnimationState;
		Animation CurrentAnimation;
		Animation IdleAnimation;
		Animation WalkAnimation;
		
		private bool moveLeft;
		private bool moveRight;
		
		private bool Jump;
		
		private BaseTerrain groundObject;
		
		private bool isMoving; //This is used to decide whether to use idle or running animations
		private bool isFalling;
		private float GravityStrength;
		private float VerticalVelocity;
		private float MaxFallingSpeed;
		private float JumpStrength;
		
		private Vector2 SpawnPoint;
		
		public Player ()
		{
			//\====================================
			//\Set up player sound effects
			//\====================================
			
			jumpOneSound = new Sound("/Application/assets/sfx/jump1.wav");
			jumpTwoSound = new Sound("/Application/assets/sfx/jump2.wav");
			jumpThreeSound = new Sound("/Application/assets/sfx/jump3.wav");
			jumpFourSound = new Sound("/Application/assets/sfx/jump4.wav");
			
			//\====================================
			//\Set up player animations
			//\====================================
			
			playerWidth = 46;
			playerHeight = 109;
			
			IdleAnimation = new Animation();
			IdleAnimation.LoadAnimation("catIdle");
			IdleAnimation.Move ( playerPosition );
			IdleAnimation.Resize( playerWidth, playerHeight );
			
			WalkAnimation = new Animation();
			WalkAnimation.LoadAnimation("catWalk");
			WalkAnimation.Move ( playerPosition );
			WalkAnimation.Resize( playerWidth, playerHeight );
			WalkAnimation.SetView( false );	//This animation is not used from the start, so we want to hide it from view
			
			CurrentAnimation = IdleAnimation;
			CurrentAnimationState = AnimationState.Idle;
			
			//\====================================
			//\Set up stuff for combat mechanics
			//\====================================
			
			attackPosition = playerPosition;
			attackWidth = 15;
			attackHeight = 15;
			
			SpawnPoint = new Vector2();
			SpawnPoint.X = 100;
			SpawnPoint.Y = 100;
			playerPosition.X = SpawnPoint.X;	//Set the players initial x position
			playerPosition.Y = SpawnPoint.Y;	//Set the players initial y position
			facingRight = true;
			previousPosition = playerPosition;
			
			Attack = new boxCollider();
		
			isFalling = true;
			NormalMovementSpeed = 10.0f;
			SprintingMovementSpeed = 20.0f;
			CurrentMovementSpeed = NormalMovementSpeed;
			VerticalVelocity = 0.0f;
			MaxFallingSpeed = 15.0f;
			JumpStrength = 50.0f;
			GravityStrength = 2.0f;
			
		}
		
		public void MovePlayer( float xPos, float yPos )
		{
			playerPosition.X = xPos;
			playerPosition.Y = yPos;
			CurrentAnimation.Move(playerPosition);
		}
		
		public void Update()
		{
			UpdateSprite ();	//Moves the players sprite to its current position
			GetInput();	//Gets input from the gamepad, then makes a note of what buttons have been pressed
			this.Set ( playerPosition, playerWidth, playerHeight);//Update the players box collider information
			CheckEnvironmentCollisions();	//After getting input from the player, we know know where 
			//the player is trying to move this frame, so we get as close to that position as the environment will allow.
			CurrentAnimation.Play();	//Plays the current player animation
		}
		
		public void UpdateSprite()
		{
			isMoving = previousPosition.X != playerPosition.X ? true : false;
			
			switch (CurrentAnimationState)
			{//In here we change what animation state the player is in so we are 
				//viewing the correct animations, and hiding ones that aren't being used
			case AnimationState.Idle:
				if (isMoving)
				{//Enters here when we want to change to the walking animation state
					CurrentAnimationState = AnimationState.Walk;
					CurrentAnimation.SetView(false);	//Hide the idle animation
					CurrentAnimation = WalkAnimation;	//Change to the walking animation
					CurrentAnimation.SetView(true);	//Unhide this animation
				}
				break;
			case AnimationState.Walk:
				if (!isMoving)
				{//Enters here when we want to change to the idle animation state
					CurrentAnimationState = AnimationState.Idle;
					CurrentAnimation.SetView(false);
					CurrentAnimation = IdleAnimation;
					CurrentAnimation.SetView(true);
				}
				break;
			}
			
			if ( previousPosition.X > playerPosition.X )
				facingRight = true;
			if ( previousPosition.X < playerPosition.X )
				facingRight = false;
			
			CurrentAnimation.FaceRight( facingRight );
			previousPosition = playerPosition;
		}
		
		public void UpdatePosition( Vector2 dp )
		{
			playerPosition = dp;
			CurrentAnimation.Move(playerPosition);
		}
		
		public Vector2 GetPosition()
		{
			return playerPosition;
		}
		
		public void GetInput()
		{
			//This function is called 60 times per second
			PadData = GamePad.GetData (0);	//Update the gamepad input
			
			if ( (PadData.Buttons & GamePadButtons.Circle) != 0 )
				Respawn ();
			if ( (PadData.Buttons & GamePadButtons.Square ) != 0 )
				checkRange ();
			moveLeft = ((PadData.AnalogLeftX < 0.0f ) || ((PadData.Buttons & GamePadButtons.Left) != 0)) ? true : false;
			moveRight = ((PadData.AnalogLeftX > 0.0f ) || ((PadData.Buttons & GamePadButtons.Right) != 0)) ? true : false;
			CurrentMovementSpeed = ((PadData.Buttons & GamePadButtons.Square) != 0) ? SprintingMovementSpeed : NormalMovementSpeed;
			if ( !isFalling )
				Jump = ((PadData.Buttons & GamePadButtons.Cross) != 0) ? true : false;
		}
		
		private void CheckEnvironmentCollisions()
		{
			List<BaseTerrain> terrainList = TerrainObjects.Instance.GetObjectList();
			if ( terrainList.Count > 0 )
			{
				boxCollider desiredLocation = new boxCollider();
				Vector2 desiredPosition = new Vector2();
				desiredPosition = playerPosition;
				desiredPosition.X += moveRight ? CurrentMovementSpeed : 0;
				desiredPosition.X -= moveLeft ? CurrentMovementSpeed : 0;
				
				if ( isFalling && ( VerticalVelocity <= MaxFallingSpeed ) )
					VerticalVelocity -= GravityStrength;
				if ( VerticalVelocity > MaxFallingSpeed )
					VerticalVelocity = MaxFallingSpeed;
				
				if ( Jump )
				{
					VerticalVelocity += JumpStrength;
					isFalling = true;
					Jump = false;
					var r = new Random();
					switch(r.Next (4))
					{
					case 0:
						soundPlayerBullet = jumpOneSound.CreatePlayer();
						soundPlayerBullet.Volume = .3f;
						soundPlayerBullet.Play();
						break;
					case 1:
						soundPlayerBullet = jumpTwoSound.CreatePlayer();
						soundPlayerBullet.Volume = .3f;
						soundPlayerBullet.Play();
						break;
					case 2:
						soundPlayerBullet = jumpThreeSound.CreatePlayer();
						soundPlayerBullet.Volume = .3f;
						soundPlayerBullet.Play();
						break;
					case 3:
						soundPlayerBullet = jumpFourSound.CreatePlayer();
						soundPlayerBullet.Volume = .3f;
						soundPlayerBullet.Play();
						break;
					}
				}
				
				desiredPosition.Y += isFalling ? VerticalVelocity : 0;
				/*
				desiredPosition.Y += moveUp ? CurrentMovementSpeed : 0;
				desiredPosition.Y -= moveDown ? CurrentMovementSpeed : 0;
				*/
				desiredLocation.Set( desiredPosition, playerWidth, playerHeight );
				
				foreach ( BaseTerrain obj in terrainList )
				{
					switch(obj.GetTerrainType())
					{
						case TerrainType.Ground:
						{
							if ( isFalling )
							{
								if ( desiredLocation.bottomCollide(obj) )
								{
									desiredPosition.Y += ( ( obj.position.Y + obj.height ) - desiredPosition.Y );
									desiredLocation.Set ( desiredPosition, playerWidth, playerHeight );
									isFalling = false;
									groundObject = obj;
								}
							}
							else
							{
								if ( playerPosition.X > ( groundObject.position.X + groundObject.width ) || ( playerPosition.X + playerWidth ) < groundObject.position.X )
									isFalling = true;
							}
							if ( moveLeft )
							{
								if ( desiredLocation.leftCollide(obj) )
								{
									desiredPosition.X += ( ( obj.position.X + obj.width ) - desiredLocation.position.X );
									desiredLocation.Set(desiredPosition, playerWidth, playerHeight);
									moveLeft = false;
								}
							}
							if ( moveRight )
							{
								if ( desiredLocation.rightCollide(obj) )
								{
									desiredPosition.X -= ( ( desiredPosition.X + playerWidth ) - obj.position.X );
									desiredLocation.Set(desiredPosition, playerWidth, playerHeight);
									moveRight = false;
								}
							}
						}
						break;
						case TerrainType.Wall:
						{
							if ( moveLeft )
							{
								if ( desiredLocation.leftCollide(obj) )
								{
									desiredPosition.X += ( ( obj.position.X + obj.width ) - desiredLocation.position.X );
									desiredLocation.Set(desiredPosition, playerWidth, playerHeight);
									moveLeft = false;
								}
							}
							if ( moveRight )
							{
								if ( desiredLocation.rightCollide(obj) )
								{
									desiredPosition.X -= ( ( desiredPosition.X + playerWidth ) - obj.position.X );
									desiredLocation.Set(desiredPosition, playerWidth, playerHeight);
									moveRight = false;
								}
							}
							if ( isFalling )
							{
								if ( desiredLocation.bottomCollide(obj) )
								{
									desiredPosition.Y += ( ( obj.position.Y + obj.height ) - desiredPosition.Y );
									desiredLocation.Set ( desiredPosition, playerWidth, playerHeight );
									isFalling = false;
									groundObject = obj;
								}
							}
						}
						break;
					}
				}
				CheckEnemyCollisions( desiredPosition );
			}
		}
		
		private void CheckEnemyCollisions( Vector2 dp )
		{
			
			UpdatePosition( dp );
		}
		
		private void checkRange()
		{
			if ( EnemyList.instance != null )
			{
				if (facingRight ) //move the attack box  before setting it
					attackPosition.X = ( playerPosition.X + playerWidth );	
				else 
					attackPosition.X = ( playerPosition.X - attackWidth );
				
				attackPosition.Y = playerPosition.Y; // the y possition is the same either way
				// no point updating the collision box if there are no enemies
				Attack.Set ( attackPosition, attackWidth, attackHeight ); //update the box collision's information up here so we only need to call it once when we call check range and we dont need to constantly update it
				for ( int i = 0; i < EnemyList.instance.objectCounter; i++ )
				{
					if(EnemyList.instance.enemyObjects[i].OnScreen)
					{
						if(facingRight )
						{
							if (EnemyList.instance.enemyObjects[i].GetPosition().X > attackPosition.X  )
							{
								if(Attack.isColliding(EnemyList.instance.enemyObjects[i]))
								{
									EnemyList.instance.enemyObjects[i].MoveEnemy(100, 100);
								}
							}
						}
						else
						{
							if (EnemyList.instance.enemyObjects[i].GetPosition().X < playerPosition.X  )
							{
								if(Attack.isColliding(EnemyList.instance.enemyObjects[i]))
								{
									EnemyList.instance.enemyObjects[i].MoveEnemy(100, 100);
								}
							}
						}
					}
				}
			}
		}
		
		private void Respawn()
		{//Sends the player back to the start of the level
			playerPosition = SpawnPoint;
			CurrentAnimation.Move(playerPosition);
			isFalling = true;
			VerticalVelocity = 0.0f;
		}
	}
}

