//\=====================================
//\Author: Harley Laurie / Daniel Popovic
//\Date Created: 21/10/2013
//\Last Edit: 21/10/2013
//\Brief: Player class
//\=====================================

using System;

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
		
		private bool isFalling;
		private float GravityStrength;
		private float VerticalVelocity;
		private float MaxFallingSpeed;
		private float JumpStrength;
		
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
			
			currentSprite = idleSprite;
		
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
		}
		
		private void CheckEnvironmentCollisions()
		{
			TerrainList tl = TerrainList.instance;
			if ( tl != null )
			{
				boxCollider desiredLocation = new boxCollider();
				Vector2 desiredPosition = new Vector2();
				desiredPosition = playerPosition;
				desiredPosition.X += moveRight ? CurrentMovementSpeed : 0;
				desiredPosition.X -= moveLeft ? CurrentMovementSpeed : 0;
				
				if ( isFalling )
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
				
				foreach ( Terrain obj in tl.terrainObjects )
				{
					if ( isFalling )
					{
						if ( desiredLocation.bottomCollide(obj) )
						{
							desiredPosition.Y += ( ( obj.position.Y + obj.height ) - desiredPosition.Y );
							desiredLocation.Set ( desiredPosition, playerWidth, playerHeight );
							isFalling = false;
						}
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
					/*
					if ( moveUp )
					{
						if ( desiredLocation.topCollide(obj) )
						{
							desiredPosition.Y -= ( ( desiredPosition.Y + playerHeight ) - obj.position.Y );
							desiredLocation.Set (desiredPosition, playerWidth, playerHeight);
							moveUp = false;
						}
					}
					if ( moveDown )
					{
						if ( desiredLocation.bottomCollide(obj) )
						{
							desiredPosition.Y += ( ( obj.position.Y + obj.height ) - desiredPosition.Y );
							desiredLocation.Set ( desiredPosition, playerWidth, playerHeight );
							moveDown = false;
						}
					}
					*/
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

