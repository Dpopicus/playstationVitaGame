//\=====================================
//\Author: Harley Laurie / Daniel Popovic
//\Date Created: 21/10/2013
//\Last Edit: 21/10/2013
//\Brief: This is an overload of the Scene
//\class, to make a few things a little more
//\easy.
//\=====================================

using Sce.PlayStation.HighLevel.GameEngine2D;

namespace CrateFighter
{
	public class NoCleanupScene : Scene
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}

		public override void OnExit()
		{
			StopAllActions();
		}
	}
}

