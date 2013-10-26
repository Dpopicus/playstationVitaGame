//\=====================================
//\Author: Harley Laurie / Daniel Popovic
//\Date Created: 21/10/2013
//\Last Edit: 21/10/2013
//\Brief: Class for players HUD / GUI
//\=====================================

using System;

using Sce.PlayStation.Core;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace CrateFighter
{
	public class GUI
	{
		private SpriteTile GUISprite;
		private Vector2 GUIPosition;	
		public GUI ()
		{
			GUISprite = Support.SpriteFromFile("Application/assets/guitest.png", 960, 544, 0, 0);
			GUIPosition.X = 0;
			GUIPosition.Y = 0;
			Game.Instance.GameScene.AddChild(GUISprite, 3);
		}
		public void MoveGUI( float xPos, float yPos )
		{
			GUIPosition.X = (xPos) - 480; // sets the gui position to the bottom left corner of the screen in relation to the player position
			GUIPosition.Y = (yPos) - 142;
			GUISprite.Quad.T = GUIPosition;
		}
		public void UpdatePosition()
		{
			GUISprite.Quad.T = GUIPosition;
		}
	}
}

