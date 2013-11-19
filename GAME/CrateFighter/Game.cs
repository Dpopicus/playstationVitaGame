//\=====================================
//\Author: Harley Laurie / Daniel Popovic
//\Date Created: 21/10/2013
//\Last Edit: 27/10/2013
//\Brief: Main Game state to be used until
//\a proper base state / state manager is
//\implemented.
//\=====================================

using System;

using Sce.PlayStation.Core;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

using Sce.PlayStation.Core.Audio;

namespace CrateFighter
{
	public class Game
	{
		public SoundPlayer soundPlayer;
		public static Game Instance;	//Singleton instance of this game state
		
		public Level levelOneTest; //Class test to load in a level from Tiled level editor XML doc
		
		public Scene SplashScreen;	//The level begin splash screen
		public Scene GameScene;	//The first game scene for testing gameplay
		
		public SpriteTile Splash;	//This is a sprite used as the background of the main menu
		public SpriteTile Background; //The background image for the testing level
		
		public GUI guiTest;	//Test for drawing player HUD / GUI
		
		public Player playerInstance;	//Instance of the player class
		
		private float timeSinceLastUpdate;	//Will count up until it reaches the updateDelay, will update the game then begin counting again from zero
		private float updateDelay;	//Time in milliseconds between game updates (60 per second)
		
		public Game ()
		{
			updateDelay = 0.03f;	//60 / 1000 = 0.06
			timeSinceLastUpdate = 0.0f;
		}
		
		public void Stahp()
		{
			Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.UnscheduleAll();	
		}
		
		public void Initialize()
		{
			SplashScreen = new Scene();	//Initialise the games menu scene
			Splash = Support.TiledSpriteFromFile("/Application/assets/levelStart.png", 1, 1);	//Create a sprite for the background
			SplashScreen.AddChild(Splash);		//Add this new sprite as a child to the menu scene
			
			Vector2 ideal_screen_size = new Vector2(960.0f, 544.0f);	//Set the screen resolution
			
			Camera2D Splash_camera = SplashScreen.Camera as Camera2D;	//Create a camera for viewing the scene
			Splash_camera.SetViewFromHeightAndCenter(ideal_screen_size.Y, ideal_screen_size / 2.0f);	//Position the camera in the desired position
			
			GameScene = new Scene();	//Initialise the game scene
			
			Director.Instance.RunWithScene(new Scene(), true);	//Set the game to run with a new blank scene
			
			Director.Instance.Update();	//Force tick so the scene is set
			
			StartSplash();
		}
		
		public void Cleanup()
		{
			soundPlayer.Dispose();
			Instance = null;
		}
		
		public void StartSplash()
		{
			
			Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Unschedule(GameScene, TickGame);	//Set the game scene as inactive
			Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(SplashScreen, TickSplash, 0.0f, false);	//Set the Splash scene as active
			
			//Set up a transition between the scenes
			//var transition = new TransitionSolidFade(SplashScreen) { PreviousScene = Director.Instance.CurrentScene, Duration = 3.0f, Tween = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.Linear };
			Director.Instance.ReplaceScene(SplashScreen);
		}
		
		public void TickSplash(float dt)
		{
			if (Director.Instance.CurrentScene != SplashScreen)
				return;		//Wait until the transition between the blank scene and the Splash scene is complete
			
			Input2.TouchData touch = Input2.Touch00;	//Get touch input from the vita
			if (touch.Down)		//If someone taps on the screen
				StartGame();	//Then proceed to the game state
		}
		
		public void StartGame()
		{
			
			Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Unschedule(SplashScreen, TickSplash); //Set Splash as inactive / unload it
			Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(GameScene, TickGame, 0.0f, false);	//Set game scene as active / load in the assets
			
			//Start the transition between the two scenes
			//var transition = new TransitionSolidFade(GameScene) { PreviousScene = Director.Instance.CurrentScene, Duration = 1.5f, Tween = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.Linear };
			Director.Instance.ReplaceScene(GameScene);
			
			Support.MusicSystem.Instance.PlayNoClobber("Ackman.mp3", true);	//Play some music for the first level
			
			playerInstance = new Player();	//Create a player
			guiTest = new GUI();
			//ground = new Ground( "Application/assets/platformPlaceholder.png", 0, 0, 1000, 25 );
			
			levelOneTest = new Level();
			levelOneTest.LoadLevel(1);
		}
		
		public void TickGame(float dt)
		{
			timeSinceLastUpdate += dt;
			if ( timeSinceLastUpdate >= updateDelay )
			{
				UpdateGame ();
				timeSinceLastUpdate = 0.0f;
			}
			CrateFighter.AppMain.MoveCamera( playerInstance.GetPosition().X, playerInstance.GetPosition().Y + 130 );	//Move the camera to the players position
			guiTest.MoveGUI( playerInstance.GetPosition().X,  playerInstance.GetPosition().Y );	//Move the GUI to stay on the screen
		}
		
		private void UpdateGame()
		{
			playerInstance.Update();
			guiTest.Update(playerInstance.Health);
			
			if ( EnemyList.instance != null )
			{
				foreach ( var enemy in EnemyList.instance.enemyObjects )
				{
					enemy.Update();
					enemy.GetPlayerPos( playerInstance.GetPosition().X, playerInstance.GetPosition().Y );
				}
			}
		}
		
		public void DamagePlayer()
		{
			playerInstance.DamagePlayer();
		}
	}
}

