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
		
		private bool lastDirection;	//True is last direction player tried to move is left, false if right
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
		
		private bool HealdDown; //checks if attack is being heald down (dan wtf is heald) you dont know how to spell held? -_-
		private bool enemyHit; // checks if any of the attack collisions went through, used to avoid counting damage multiple times per hit
		
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
			HealdDown = false;
			enemyHit = false;
			lastDirection = true;	//Set last direction to facing left
			
			//Player idle animation
			IdleAnimation = new Animation();
			IdleAnimation.LoadAnimation("catIdle");
			IdleAnimation.Move ( playerPosition );
			IdleAnimation.Resize( playerWidth, playerHeight );
			
			//Player run animation
			WalkAnimation = new Animation();
			WalkAnimation.LoadAnimation("SamuraiRun");
			WalkAnimation.Move ( playerPosition );
			WalkAnimation.Resize( playerWidth, playerHeight );
			WalkAnimation.SetView( false );	//This animation is not used from the start, so we want to hide it from view
			
			CurrentAnimation = IdleAnimation;
			CurrentAnimationState = AnimationState.Idle;
			
			//\====================================
			//\Set up stuff for combat mechanics
			//\====================================
			
			attackPosition = playerPosition;
			attackWidth = 35;
			attackHeight = 35;
			
			SpawnPoint = new Vector2();
			SpawnPoint.X = 100;//This will be read in from the level data later on
			SpawnPoint.Y = 100;
			playerPosition.X = SpawnPoint.X;	//Set the players initial x position
			playerPosition.Y = SpawnPoint.Y;	//Set the players initial y position
			previousPosition = playerPosition;
			
			Attack = new boxCollider();
		
			isFalling = true;
			NormalMovementSpeed = 10.0f;
			SprintingMovementSpeed = 20.0f;
			CurrentMovementSpeed = NormalMovementSpeed;
			VerticalVelocity = 0.0f;
			MaxFallingSpeed = 15.0f;
			JumpStrength = 120.0f;	//How much the players vertical velocity is changed when it jumps
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
			
			CurrentAnimation.FaceRight( lastDirection );
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
				Respawn ();//Send the player back to spawn in pressed circle
			
			//Set flags to say the player is trying to move left or right this frame
			moveLeft = ((PadData.AnalogLeftX < 0.0f ) || ((PadData.Buttons & GamePadButtons.Left) != 0)) ? true : false;
			moveRight = ((PadData.AnalogLeftX > 0.0f ) || ((PadData.Buttons & GamePadButtons.Right) != 0)) ? true : false;
			
			//Keep track of the last direction the player tried to move, for sprite orientation
			if (moveLeft)
				lastDirection = true;
			if (moveRight)
				lastDirection = false;
			
			//Sprite in holding down square
			CurrentMovementSpeed = ((PadData.Buttons & GamePadButtons.Square) != 0) ? SprintingMovementSpeed : NormalMovementSpeed;
			
			//If the player isn't already in the air, check for jump
			if ( !isFalling )
				Jump = ((PadData.Buttons & GamePadButtons.Cross) != 0) ? true : false;
		}
		
		private void CheckEnvironmentCollisions()
		{//After player input has been detected for this frame, we create a temporary
			//box collider, and try to move it to where the player wants to go.
			//Wont go through terrain or enemies, but will get as close as possible
			//to where the player wants to be, based on their input.
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
					{//When the player jumps, we want to play a sound effect
						//There a 4 different jump sound effects, and a random 
						//one is played each time
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
				{//Loop through all the terrain objects in the level, and check collision
					//with them
					switch(obj.GetTerrainType())
					{//Different objects have different ways to check for collisions (walls and floor are different for example)
						//so we use a switch statement to use the right calculations for the corresponding terrain object
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
				//After we have finished doing environment collisions
				//we want to pass on the value that the player should be
				//able to move to, and check this against enemies in the world
				CheckEnemyCollisions(desiredPosition, desiredLocation);
			}
		}
		
		private void CheckEnemyCollisions( Vector2 a_v2DesiredPosition, boxCollider a_bcDesiredLocation )
		{
			if ( EnemyList.instance != null )
			{
				for ( int i = 0; i < EnemyList.instance.objectCounter; i++ )
				{
					if(EnemyList.instance.enemyObjects[i].OnScreen)
					{
						if (EnemyList.instance.enemyObjects[i].GetPosition().X > a_v2DesiredPosition.X  )
						{
							if(a_bcDesiredLocation.rightCollide(EnemyList.instance.enemyObjects[i]))
							{
								a_v2DesiredPosition.X -= ( ( a_v2DesiredPosition.X + playerWidth ) - EnemyList.instance.enemyObjects[i].position.X );
								a_bcDesiredLocation.Set(a_v2DesiredPosition, playerWidth, playerHeight);
								moveRight = false;
								EnemyList.instance.enemyObjects[i].TouchingLeft = true;
							}
							else
								EnemyList.instance.enemyObjects[i].TouchingLeft = false;
						}
						else
							EnemyList.instance.enemyObjects[i].TouchingLeft = false;
						if (EnemyList.instance.enemyObjects[i].GetPosition().X < (playerPosition.X + CurrentMovementSpeed)  )
						{
							if(a_bcDesiredLocation.leftCollide( EnemyList.instance.enemyObjects[i] ))
							{
								a_v2DesiredPosition.X += ( ( EnemyList.instance.enemyObjects[i].position.X + EnemyList.instance.enemyObjects[i].width ) - a_bcDesiredLocation.position.X );
								a_bcDesiredLocation.Set(a_v2DesiredPosition, playerWidth, playerHeight);
								moveLeft = false;
								EnemyList.instance.enemyObjects[i].TouchingRight = true;
							}
							else EnemyList.instance.enemyObjects[i].TouchingRight = false;
						}
						else
							EnemyList.instance.enemyObjects[i].TouchingRight = false;
					}
				}
			}
			//Now we are finished enemy collisions, we pass the date on to move the player 
			//to where they should be, able update sprites etc
			UpdatePosition(a_v2DesiredPosition);
		}
				
		private void checkRange()
		{
			if ( EnemyList.instance != null )
			{
				if (!lastDirection ) //move the attack box  before setting it
					attackPosition.X = ( playerPosition.X + attackWidth );	
				if(lastDirection) 
					attackPosition.X = ( playerPosition.X - attackWidth );
				
				enemyHit = false;
				attackPosition.Y = playerPosition.Y; // the y possition is the same either way
				// no point updating the collision box if there are no enemies
				Attack.Set ( attackPosition, attackWidth, attackHeight ); //update the box collision's information up here so we only need to call it once when we call check range and we dont need to constantly update it
				for ( int i = 0; i < EnemyList.instance.objectCounter; i++ )
				{
					if(EnemyList.instance.enemyObjects[i].OnScreen)
					{
						if(!lastDirection )
						{
							if (EnemyList.instance.enemyObjects[i].GetPosition().X > attackPosition.X  )
							{
								if(Attack.isColliding(EnemyList.instance.enemyObjects[i]))
								{
									enemyHit = true;
								}
								if(Attack.leftCollide(EnemyList.instance.enemyObjects[i]))
								{
									enemyHit = true;
								}
								if(Attack.rightCollide(EnemyList.instance.enemyObjects[i]))
								{
									enemyHit = true;
								}
							}
						}
						else
						{
							if (EnemyList.instance.enemyObjects[i].GetPosition().X < playerPosition.X  )
							{
								if(Attack.isColliding(EnemyList.instance.enemyObjects[i]))
								{
									enemyHit = true;
								}
								if(Attack.leftCollide(EnemyList.instance.enemyObjects[i]))
								{
									enemyHit = true;
								}
								if(Attack.rightCollide(EnemyList.instance.enemyObjects[i]))
								{
									enemyHit = true;
								}
							}
						}
						if (enemyHit )
						{
							EnemyList.instance.enemyObjects[i].health -= 50;
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

