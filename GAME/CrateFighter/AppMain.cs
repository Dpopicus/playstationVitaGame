//\=====================================
//\Author: Harley Laurie / Daniel Popovic
//\Date Created: 21/10/2013
//\Last Edit: 21/10/2013
//\Brief: Main Entry point for the crate
//\fighter game.
//\=====================================

using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.UI;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace CrateFighter
{
	public class AppMain
	{
		public static bool menuActive = true;	//Should the menu still be viewing?
		static GraphicsContext graphics;	//Graphics context, only used for the main menu. Changes to using director for the game
		static CrateFighter.menu mainMenu;
		
		public static void Main (string[] args)
		{
			//LoadMenu();
			//while (menuActive)
			//{
			//	RunMenu();	
			//}
			
			Initialize();
			while (true) {
				SystemEvents.CheckEvents ();
				Update ();
				Render ();
			}
		}

		public static void Initialize ()
		{
			//graphics.Dispose();	//Get rid of the old graphics context that is used for the main menu
			//This can't run at the same time as the Director
			
			Director.Initialize();	//Set up the director
			Game.Instance = new Game();	//Create an instance of the game
			Game.Instance.Initialize();	//Intialise the game instance
			
			Vector2 ideal_screen_size = new Vector2(960.0f, 544.0f);	//Set up the screen resolution
			Camera2D camera = Game.Instance.GameScene.Camera as Camera2D;	//create a camera
			camera.SetViewFromHeightAndCenter(ideal_screen_size.Y, ideal_screen_size / 2.0f);	//set up the camera pos
		}
		
		public static void LoadMenu()
		{
			graphics = new GraphicsContext();	//Create a new graphics context for drawing the menu
			UISystem.Initialize(graphics);	//Start up the UI system using the graphics context
			mainMenu = new CrateFighter.menu();	//Create a new main menu
			UISystem.SetScene(mainMenu);	//Set the main menu as the active scene
			Support.MusicSystem.Instance.PlayNoClobber("Saber1.mp3", true);	//Loop some music for the main menu
		}
		
		public static void RunMenu()
		{
			SystemEvents.CheckEvents();
			List<TouchData> touchData = Touch.GetData(0);
			UISystem.Update (touchData);
			
			graphics.SetViewport(0, 0, graphics.Screen.Width, graphics.Screen.Height);
			graphics.SetClearColor(new Vector4(0,0,0,1));
			graphics.SetClearDepth(1.0f);
			graphics.Clear();
			
			UISystem.Render();
			graphics.SwapBuffers();
		}

		public static void Update ()
		{
			var gamePadData = GamePad.GetData (0);	// Query gamepad for current state
			
			Director.Instance.Update();	//Update whatever scene is active
		}

		public static void Render ()
		{
			Director.Instance.Render();	//Draw the current scene
			
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SwapBuffers();	//Present the frame when its ready to draw
			Director.Instance.PostSwap();	//Dunno what this does lol
		}
		
		public static void MoveCamera( float x, float y )
		{
			Vector2 position = new Vector2(x, y);
			Vector2 ideal_screen_size = new Vector2(960.0f, 544.0f);	//Set up the screen resolution
			Camera2D camera = Game.Instance.GameScene.Camera as Camera2D;	//get the camera
			camera.SetViewFromHeightAndCenter(ideal_screen_size.Y, position);	//set the camera pos
		}
	}
}
