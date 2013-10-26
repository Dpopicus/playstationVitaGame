using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.HighLevel.UI;

namespace CrateFighter
{
    public partial class menu : Scene
    {
        public menu()
        {
            InitializeWidget();
			newGameButton.TouchEventReceived += new EventHandler<TouchEventArgs>(NewButtonTouched);
			continueButton.TouchEventReceived += new EventHandler<TouchEventArgs>(ContinueButtonTouched);
			loadGameButton.TouchEventReceived += new EventHandler<TouchEventArgs>(LoadButtonTouched);
			optionsButton.TouchEventReceived += new EventHandler<TouchEventArgs>(OptionsButtonTouched);
			extrasButton.TouchEventReceived += new EventHandler<TouchEventArgs>(ExtrasButtonTouched);
			quitButton.TouchEventReceived += new EventHandler<TouchEventArgs>(QuitButtonTouched);
        }
		
		private void NewButtonTouched(object sender, TouchEventArgs eventArgs)
		{
			CrateFighter.AppMain.menuActive = false;
		}
		
		private void ContinueButtonTouched(object sender, TouchEventArgs eventArgs)
		{
			Console.Write ("Touching continue button.\n");
		}
		
		private void LoadButtonTouched(object sender, TouchEventArgs eventArgs)
		{
			Console.Write ("Touching load button.\n");
		}
		
		private void OptionsButtonTouched(object sender, TouchEventArgs eventArgs)
		{
			Console.Write ("Touching options button.\n");
		}
		
		private void ExtrasButtonTouched(object sender, TouchEventArgs eventArgs)
		{
			Console.Write ("Touching extras button.\n");
		}
		
		private void QuitButtonTouched(object sender, TouchEventArgs eventArgs)
		{
			Console.Write ("Touching quit button.\n");
		}
    }
}
