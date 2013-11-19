//\=====================================
//\Author: Harley Laurie / Daniel Popovic
//\Date Created: 21/10/2013
//\Last Edit: 21/10/2013
//\Brief: Class for players HUD / GUI
//\=====================================
using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace CrateFighter
{
	enum HealthState
	{
		state_One = 1,// when the enemy sees the player and moves towards them
		state_Two = 2,
		state_Three = 3,
		state_Four = 4,
		state_Five = 5
	};
		
	public class GUI
	{
		public Vector2 HealthBarPosition;
		private int HealthBarWidth;
		private int HealthBarHeight;
		
		Animation CurrentAnimation;
		Animation AnimationOne;
		Animation AnimationTwo;
		Animation AnimationThree;
		Animation AnimationFour;
		Animation AnimationFive;
		
		HealthState CurrentHealthState;
		
		public GUI ()
		{
			HealthBarWidth = 300;
			HealthBarHeight = 60;
			
			AnimationOne = new Animation();
			AnimationOne.LoadAnimation("1Hit");
			AnimationOne.Move ( HealthBarPosition );
			AnimationOne.Resize( HealthBarWidth, HealthBarHeight );
			
			AnimationTwo = new Animation();
			AnimationTwo.LoadAnimation("2Hits");
			AnimationTwo.Move ( HealthBarPosition );
			AnimationTwo.Resize( HealthBarWidth, HealthBarHeight );
			AnimationTwo.SetView( false );
			
			AnimationThree = new Animation();
			AnimationThree.LoadAnimation("3Hits");
			AnimationThree.Move ( HealthBarPosition );
			AnimationThree.Resize( HealthBarWidth, HealthBarHeight );
			AnimationThree.SetView( false );
			
			AnimationFour = new Animation();
			AnimationFour.LoadAnimation("4Hits");
			AnimationFour.Move ( HealthBarPosition );
			AnimationFour.Resize( HealthBarWidth, HealthBarHeight );
			AnimationFour.SetView( false );
			
			AnimationFive = new Animation();
			AnimationFive.LoadAnimation("5HITS");
			AnimationFive.Move ( HealthBarPosition );
			AnimationFive.Resize( HealthBarWidth, HealthBarHeight );
			AnimationFive.SetView( false );
			
			CurrentAnimation = AnimationOne;
			CurrentHealthState = HealthState.state_Five;
		}
		
		public void MoveGUI(float xPos, float yPos)
		{
			HealthBarPosition.X = (xPos - 480);
			HealthBarPosition.Y = (yPos - 150);
			CurrentAnimation.Move(HealthBarPosition);
		}
		
		public void Update(int health)
		{
			// changed player health down to 5 and the damage the enemies deal down to one for sompliciry in this function
			
			if (health == 1)
			{
				CurrentHealthState = HealthState.state_One;
			}
			else if (health == 2)
			{
				CurrentHealthState = HealthState.state_Two;
			}
			else if (health == 3)
			{
				CurrentHealthState = HealthState.state_Three;
			}
			else if (health == 4)
			{
				CurrentHealthState = HealthState.state_Four;
			}
			else if (health == 5)
			{
				CurrentHealthState = HealthState.state_Five;
			}
			
			switch(	CurrentHealthState ) //sets the behavior and the animation for the enemy
			{
			case HealthState.state_One:
				CurrentAnimation.SetView(false);
				CurrentAnimation = AnimationOne;
				CurrentAnimation.SetView(true);
				break;
			case HealthState.state_Two:
				CurrentAnimation.SetView(false);
				CurrentAnimation = AnimationTwo;
				CurrentAnimation.SetView(true);
				break;
			case HealthState.state_Three:
				CurrentAnimation.SetView(false);
				CurrentAnimation = AnimationThree;
				CurrentAnimation.SetView(true);
				break;
			case HealthState.state_Four:
				CurrentAnimation.SetView(false);
				CurrentAnimation = AnimationFour;
				CurrentAnimation.SetView(true);
				break;
			case HealthState.state_Five:
				CurrentAnimation.SetView(false);
				CurrentAnimation = AnimationFive;
				CurrentAnimation.SetView(true);
				break;
			}			
			CurrentAnimation.Play();
		}
	}
}

