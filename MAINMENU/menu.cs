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
        }
		
		private void NewButtonTouched(object sender, TouchEventArgs eventArgs)
		{
			CrateFighter.AppMain.menuActive = false;
		}
    }
}
