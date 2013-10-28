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
	public class Player : boxCollider
	{
		public SoundPlayer soundPlayerBullet;
		Sound jumpOneSound;
		Sound jumpTwoSound;
		Sound jumpThreeSound;
		Sound jumpFourSound;
		
		private SpriteTile currentSprite;
		private SpriteTile playerSprite;//Player sprite
		private SpriteTile idleSprite;
		
		private SpriteUV herp;
		
		private Support.AnimationAction IdleAnimation { get; set; }
		private Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile Sprite { get; set; }
		
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
		
		private bool moveLeft;
		private bool moveRight;
		
		private bool Jump;
		
		private BaseTerrain groundObject;
		
		private bool isFalling;
		private bool isWallSliding;
		
		private float NormalGravityStrength;
		private float WallSlideGravityStrength;
		private float CurrentGravityStrength;
		
		private float VerticalVelocity;
		private float MaxFallingSpeed;
		
		private float NormalJumpStrength;
		private float SlideJumpStrength;
		private float CurrentJumpStrength;
		
		private Vector2 SpawnPoint;
		
		public Player ()
		{
			jumpOneSound = new Sound("/Application/assets/sfx/jump1.wav");
			jumpTwoSound = new Sound("/Application/assets/sfx/jump2.wav");
			jumpThreeSound = new Sound("/Application/assets/sfx/jump3.wav");
			jumpFourSound = new Sound("/Application/assets/sfx/jump4.wav");
			
			playerWidth = 46;
			playerHeight = 109;
			SpawnPoint = new Vector2();
			SpawnPoint.X = 100;
			SpawnPoint.Y = 100;
			playerPosition.X = SpawnPoint.X;	//Set the players initial x position
			playerPosition.Y = SpawnPoint.Y;	//Set the players initial y position
			facingRight = true;
			previousPosition = playerPosition;
			
			//\====================
			//\Create player sprites
			//\====================
			
			playerSprite = Support.TiledSpriteFromFile( "Application/assets/playerPlaceholder.png",1 ,1 ); // image name and the fraction of the base image that each frame will take up
			playerSprite.Quad.S = new Vector2(playerWidth, playerHeight);
			Game.Instance.GameScene.AddChild(playerSprite, 2);	//Add this sprite as a child to the game scene
			playerSprite.Visible = false;
			
			idleSprite = Support.TiledSpriteFromFile ( "Application/assets/playerIdle.png", 1, 1 );
			idleSprite.Quad.S = new Vector2(playerWidth, playerHeight);
			Game.Instance.GameScene.AddChild(idleSprite, 2);
			
			herp = Support.SpriteUVFromFile ( "Application/assets/zombie_frames.png", 1, 1 );
			Game.Instance.GameScene.AddChild( herp, 2 );
			herp.UV.T = new Vector2( 0, 0 );	//this is the bottom left which is drawn
			herp.UV.S = new Vector2( 0.5f, 0.5f );	//this is the top right point which is drawn
				
			currentSprite = idleSprite;
		
			isFalling = true;
			NormalMovementSpeed = 10.0f;
			SprintingMovementSpeed = 20.0f;
			CurrentMovementSpeed = NormalMovementSpeed;
			VerticalVelocity = 0.0f;
			MaxFallingSpeed = 15.0f;
			
			NormalJumpStrength = 50.0f;
			SlideJumpStrength = 25.0f;
			CurrentJumpStrength = NormalJumpStrength;
			
			NormalGravityStrength = 2.0f;
			CurrentGravityStrength = NormalGravityStrength;
			WallSlideGravityStrength = 0.5f;
		}
		
		public void MovePlayer( float xPos, float yPos )
		{
			playerPosition.X = xPos;
			playerPosition.Y = yPos;
			playerSprite.Quad.T = playerPosition;
		}
		
		public void Update()
		{
			UpdateSprite ();
			GetInput();
			this.Set ( playerPosition, playerWidth, playerHeight);//Update the players box collider information
			CheckEnvironmentCollisions();
		}
		
		public void UpdateSprite()
		{
			if ( previousPosition.X == playerPosition.X )
				return;
			else
			{
				isWallSliding = false;
				CurrentJumpStrength = NormalJumpStrength;
			}
			
			facingRight = previousPosition.X > playerPosition.X ? false : true;
			if (facingRight)
				currentSprite.FlipU = false;
			else
				currentSprite.FlipU = true;
			previousPosition = playerPosition;
		}
		
		public void UpdatePosition( Vector2 dp )
		{
			playerPosition = dp;
			currentSprite.Quad.T = playerPosition;
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
			moveLeft = ((PadData.AnalogLeftX < 0.0f ) || ((PadData.Buttons & GamePadButtons.Left) != 0)) ? true : false;
			moveRight = ((PadData.AnalogLeftX > 0.0f ) || ((PadData.Buttons & GamePadButtons.Right) != 0)) ? true : false;
			CurrentMovementSpeed = ((PadData.Buttons & GamePadButtons.Square) != 0) ? SprintingMovementSpeed : NormalMovementSpeed;
			if ( !isFalling )
				Jump = ((PadData.Buttons & GamePadButtons.Cross) != 0) ? true : false;
			CurrentGravityStrength = isWallSliding ? WallSlideGravityStrength : NormalGravityStrength;
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
					VerticalVelocity -= CurrentGravityStrength;
				if ( VerticalVelocity > MaxFallingSpeed )
					VerticalVelocity = MaxFallingSpeed;
				
				if ( Jump )
				{
					VerticalVelocity += CurrentJumpStrength;
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
									if(isFalling)
									{
										isWallSliding = true;
										CurrentJumpStrength = SlideJumpStrength;
									}
								}
							}
							if ( moveRight )
							{
								if ( desiredLocation.rightCollide(obj) )
								{
									desiredPosition.X -= ( ( desiredPosition.X + playerWidth ) - obj.position.X );
									desiredLocation.Set(desiredPosition, playerWidth, playerHeight);
									moveRight = false;
									if(isFalling)
									{
										isWallSliding = true;
										CurrentJumpStrength = SlideJumpStrength;
									}
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
		
		private void Respawn()
		{//Sends the player back to the start of the level
			playerPosition = SpawnPoint;
			playerSprite.Quad.T = playerPosition;
			isFalling = true;
			VerticalVelocity = 0.0f;
		}
	}
}

