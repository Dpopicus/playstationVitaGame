// AUTOMATICALLY GENERATED CODE

using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.HighLevel.UI;

namespace CrateFighter
{
    partial class menu
    {
        ImageBox ImageBox_2;
        ImageBox ImageBox_1;
        Button newGameButton;

        private void InitializeWidget()
        {
            InitializeWidget(LayoutOrientation.Horizontal);
        }

        private void InitializeWidget(LayoutOrientation orientation)
        {
            ImageBox_2 = new ImageBox();
            ImageBox_2.Name = "ImageBox_2";
            ImageBox_1 = new ImageBox();
            ImageBox_1.Name = "ImageBox_1";
            newGameButton = new Button();
            newGameButton.Name = "newGameButton";

            // menu
            this.RootWidget.AddChildLast(ImageBox_2);
            this.RootWidget.AddChildLast(ImageBox_1);
            this.RootWidget.AddChildLast(newGameButton);
            this.Transition = new JumpFlipTransition();
            this.Showing += new EventHandler(onShowing);
            this.Shown += new EventHandler(onShown);

            // ImageBox_2
            ImageBox_2.Image = new ImageAsset("/Application/assets/crate_sideup.png");
            ImageBox_2.ImageScaleType = ImageScaleType.Center;

            // ImageBox_1
            ImageBox_1.Image = new ImageAsset("/Application/assets/blade.png");

            // newGameButton
            newGameButton.TextColor = new UIColor(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            newGameButton.TextFont = new UIFont(FontAlias.System, 25, FontStyle.Regular);
            newGameButton.Style = ButtonStyle.Custom;
            newGameButton.CustomImage = new CustomButtonImageSettings()
            {
                BackgroundNormalImage = new ImageAsset("/Application/assets/crate_sideup.png"),
                BackgroundPressedImage = new ImageAsset("/Application/assets/crate_sidedown.png"),
                BackgroundDisabledImage = null,
                BackgroundNinePatchMargin = new NinePatchMargin(42, 27, 42, 27),
            };

            SetWidgetLayout(orientation);

            UpdateLanguage();
        }

        private LayoutOrientation _currentLayoutOrientation;
        public void SetWidgetLayout(LayoutOrientation orientation)
        {
            switch (orientation)
            {
                case LayoutOrientation.Vertical:
                    this.DesignWidth = 544;
                    this.DesignHeight = 960;

                    ImageBox_2.SetPosition(0, 0);
                    ImageBox_2.SetSize(200, 200);
                    ImageBox_2.Anchors = Anchors.None;
                    ImageBox_2.Visible = true;

                    ImageBox_1.SetPosition(147, 35);
                    ImageBox_1.SetSize(200, 200);
                    ImageBox_1.Anchors = Anchors.None;
                    ImageBox_1.Visible = true;

                    newGameButton.SetPosition(598, 89);
                    newGameButton.SetSize(214, 56);
                    newGameButton.Anchors = Anchors.None;
                    newGameButton.Visible = true;

                    break;

                default:
                    this.DesignWidth = 960;
                    this.DesignHeight = 544;

                    ImageBox_2.SetPosition(0, 0);
                    ImageBox_2.SetSize(960, 544);
                    ImageBox_2.Anchors = Anchors.None;
                    ImageBox_2.Visible = true;

                    ImageBox_1.SetPosition(0, 0);
                    ImageBox_1.SetSize(960, 544);
                    ImageBox_1.Anchors = Anchors.None;
                    ImageBox_1.Visible = true;

                    newGameButton.SetPosition(700, 250);
                    newGameButton.SetSize(214, 56);
                    newGameButton.Anchors = Anchors.None;
                    newGameButton.Visible = true;

                    break;
            }
            _currentLayoutOrientation = orientation;
        }

        public void UpdateLanguage()
        {
            this.Title = "Main";

            newGameButton.Text = "Start Game";
        }

        private void onShowing(object sender, EventArgs e)
        {
            switch (_currentLayoutOrientation)
            {
                case LayoutOrientation.Vertical:
                    ImageBox_2.Visible = false;
                    ImageBox_1.Visible = false;
                    newGameButton.Visible = false;
                    break;

                default:
                    ImageBox_2.Visible = false;
                    ImageBox_1.Visible = false;
                    newGameButton.Visible = false;
                    break;
            }
        }

        private void onShown(object sender, EventArgs e)
        {
            switch (_currentLayoutOrientation)
            {
                case LayoutOrientation.Vertical:
                    new BunjeeJumpEffect()
                    {
                        Widget = ImageBox_2,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = ImageBox_1,
                        MoveDirection = FourWayDirection.Right,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = newGameButton,
                        MoveDirection = FourWayDirection.Down,
                    }.Start();
                    break;

                default:
                    new BunjeeJumpEffect()
                    {
                        Widget = ImageBox_2,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = ImageBox_1,
                        MoveDirection = FourWayDirection.Right,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = newGameButton,
                        MoveDirection = FourWayDirection.Down,
                    }.Start();
                    break;
            }
        }

    }
}
