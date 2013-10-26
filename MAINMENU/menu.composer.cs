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
        Button continueButton;
        Button loadGameButton;
        Button newGameButton;
        Button optionsButton;
        Button extrasButton;
        Button quitButton;

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
            continueButton = new Button();
            continueButton.Name = "continueButton";
            loadGameButton = new Button();
            loadGameButton.Name = "loadGameButton";
            newGameButton = new Button();
            newGameButton.Name = "newGameButton";
            optionsButton = new Button();
            optionsButton.Name = "optionsButton";
            extrasButton = new Button();
            extrasButton.Name = "extrasButton";
            quitButton = new Button();
            quitButton.Name = "quitButton";

            // menu
            this.RootWidget.AddChildLast(ImageBox_2);
            this.RootWidget.AddChildLast(ImageBox_1);
            this.RootWidget.AddChildLast(continueButton);
            this.RootWidget.AddChildLast(loadGameButton);
            this.RootWidget.AddChildLast(newGameButton);
            this.RootWidget.AddChildLast(optionsButton);
            this.RootWidget.AddChildLast(extrasButton);
            this.RootWidget.AddChildLast(quitButton);
            this.Transition = new JumpFlipTransition();
            this.Showing += new EventHandler(onShowing);
            this.Shown += new EventHandler(onShown);

            // ImageBox_2
            ImageBox_2.Image = new ImageAsset("/Application/assets/crate_sideup.png");
            ImageBox_2.ImageScaleType = ImageScaleType.Center;

            // ImageBox_1
            ImageBox_1.Image = new ImageAsset("/Application/assets/blade.png");

            // continueButton
            continueButton.TextColor = new UIColor(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            continueButton.TextFont = new UIFont(FontAlias.System, 25, FontStyle.Regular);
            continueButton.Style = ButtonStyle.Custom;
            continueButton.CustomImage = new CustomButtonImageSettings()
            {
                BackgroundNormalImage = new ImageAsset("/Application/assets/crate_sideup.png"),
                BackgroundPressedImage = new ImageAsset("/Application/assets/crate_sidedown.png"),
                BackgroundDisabledImage = null,
                BackgroundNinePatchMargin = new NinePatchMargin(42, 27, 42, 27),
            };

            // loadGameButton
            loadGameButton.TextColor = new UIColor(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            loadGameButton.TextFont = new UIFont(FontAlias.System, 25, FontStyle.Regular);
            loadGameButton.Style = ButtonStyle.Custom;
            loadGameButton.CustomImage = new CustomButtonImageSettings()
            {
                BackgroundNormalImage = new ImageAsset("/Application/assets/crate_sideup.png"),
                BackgroundPressedImage = new ImageAsset("/Application/assets/crate_sidedown.png"),
                BackgroundDisabledImage = null,
                BackgroundNinePatchMargin = new NinePatchMargin(42, 27, 42, 27),
            };

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

            // optionsButton
            optionsButton.TextColor = new UIColor(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            optionsButton.TextFont = new UIFont(FontAlias.System, 25, FontStyle.Regular);
            optionsButton.Style = ButtonStyle.Custom;
            optionsButton.CustomImage = new CustomButtonImageSettings()
            {
                BackgroundNormalImage = new ImageAsset("/Application/assets/crate_sideup.png"),
                BackgroundPressedImage = new ImageAsset("/Application/assets/crate_sidedown.png"),
                BackgroundDisabledImage = null,
                BackgroundNinePatchMargin = new NinePatchMargin(42, 27, 42, 27),
            };

            // extrasButton
            extrasButton.TextColor = new UIColor(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            extrasButton.TextFont = new UIFont(FontAlias.System, 25, FontStyle.Regular);
            extrasButton.Style = ButtonStyle.Custom;
            extrasButton.CustomImage = new CustomButtonImageSettings()
            {
                BackgroundNormalImage = new ImageAsset("/Application/assets/crate_sideup.png"),
                BackgroundPressedImage = new ImageAsset("/Application/assets/crate_sidedown.png"),
                BackgroundDisabledImage = null,
                BackgroundNinePatchMargin = new NinePatchMargin(42, 27, 42, 27),
            };

            // quitButton
            quitButton.TextColor = new UIColor(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            quitButton.TextFont = new UIFont(FontAlias.System, 25, FontStyle.Regular);
            quitButton.Style = ButtonStyle.Custom;
            quitButton.CustomImage = new CustomButtonImageSettings()
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

                    continueButton.SetPosition(96, 98);
                    continueButton.SetSize(214, 56);
                    continueButton.Anchors = Anchors.None;
                    continueButton.Visible = true;

                    loadGameButton.SetPosition(313, 89);
                    loadGameButton.SetSize(214, 56);
                    loadGameButton.Anchors = Anchors.None;
                    loadGameButton.Visible = true;

                    newGameButton.SetPosition(598, 89);
                    newGameButton.SetSize(214, 56);
                    newGameButton.Anchors = Anchors.None;
                    newGameButton.Visible = true;

                    optionsButton.SetPosition(130, 290);
                    optionsButton.SetSize(214, 56);
                    optionsButton.Anchors = Anchors.None;
                    optionsButton.Visible = true;

                    extrasButton.SetPosition(373, 418);
                    extrasButton.SetSize(214, 56);
                    extrasButton.Anchors = Anchors.None;
                    extrasButton.Visible = true;

                    quitButton.SetPosition(606, 418);
                    quitButton.SetSize(214, 56);
                    quitButton.Anchors = Anchors.None;
                    quitButton.Visible = true;

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

                    continueButton.SetPosition(373, 68);
                    continueButton.SetSize(214, 56);
                    continueButton.Anchors = Anchors.None;
                    continueButton.Visible = true;

                    loadGameButton.SetPosition(606, 68);
                    loadGameButton.SetSize(214, 56);
                    loadGameButton.Anchors = Anchors.None;
                    loadGameButton.Visible = true;

                    newGameButton.SetPosition(142, 68);
                    newGameButton.SetSize(214, 56);
                    newGameButton.Anchors = Anchors.None;
                    newGameButton.Visible = true;

                    optionsButton.SetPosition(142, 418);
                    optionsButton.SetSize(214, 56);
                    optionsButton.Anchors = Anchors.None;
                    optionsButton.Visible = true;

                    extrasButton.SetPosition(373, 418);
                    extrasButton.SetSize(214, 56);
                    extrasButton.Anchors = Anchors.None;
                    extrasButton.Visible = true;

                    quitButton.SetPosition(606, 418);
                    quitButton.SetSize(214, 56);
                    quitButton.Anchors = Anchors.None;
                    quitButton.Visible = true;

                    break;
            }
            _currentLayoutOrientation = orientation;
        }

        public void UpdateLanguage()
        {
            this.Title = "Main";

            continueButton.Text = "Continue";

            loadGameButton.Text = "Load Game";

            newGameButton.Text = "New Game";

            optionsButton.Text = "Options";

            extrasButton.Text = "Extras";

            quitButton.Text = "Quit";
        }

        private void onShowing(object sender, EventArgs e)
        {
            switch (_currentLayoutOrientation)
            {
                case LayoutOrientation.Vertical:
                    ImageBox_2.Visible = false;
                    ImageBox_1.Visible = false;
                    continueButton.Visible = false;
                    loadGameButton.Visible = false;
                    newGameButton.Visible = false;
                    optionsButton.Visible = false;
                    extrasButton.Visible = false;
                    quitButton.Visible = false;
                    break;

                default:
                    ImageBox_2.Visible = false;
                    ImageBox_1.Visible = false;
                    continueButton.Visible = false;
                    loadGameButton.Visible = false;
                    newGameButton.Visible = false;
                    optionsButton.Visible = false;
                    extrasButton.Visible = false;
                    quitButton.Visible = false;
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
                        Widget = continueButton,
                        MoveDirection = FourWayDirection.Down,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = loadGameButton,
                        MoveDirection = FourWayDirection.Down,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = newGameButton,
                        MoveDirection = FourWayDirection.Down,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = optionsButton,
                        MoveDirection = FourWayDirection.Up,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = extrasButton,
                        MoveDirection = FourWayDirection.Up,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = quitButton,
                        MoveDirection = FourWayDirection.Up,
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
                        Widget = continueButton,
                        MoveDirection = FourWayDirection.Down,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = loadGameButton,
                        MoveDirection = FourWayDirection.Down,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = newGameButton,
                        MoveDirection = FourWayDirection.Down,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = optionsButton,
                        MoveDirection = FourWayDirection.Up,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = extrasButton,
                        MoveDirection = FourWayDirection.Up,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = quitButton,
                        MoveDirection = FourWayDirection.Up,
                    }.Start();
                    break;
            }
        }

    }
}
