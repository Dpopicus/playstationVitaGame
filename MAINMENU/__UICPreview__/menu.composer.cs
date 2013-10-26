// AUTOMATICALLY GENERATED CODE

using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.HighLevel.UI;

namespace Preview
{
    partial class menu
    {
        ImageBox ImageBox_2;
        ImageBox ImageBox_1;
        Button Button_1;
        Button Button_2;
        Button Button_3;
        Button Button_4;
        Button Button_5;
        Button Button_6;

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
            Button_1 = new Button();
            Button_1.Name = "Button_1";
            Button_2 = new Button();
            Button_2.Name = "Button_2";
            Button_3 = new Button();
            Button_3.Name = "Button_3";
            Button_4 = new Button();
            Button_4.Name = "Button_4";
            Button_5 = new Button();
            Button_5.Name = "Button_5";
            Button_6 = new Button();
            Button_6.Name = "Button_6";

            // menu
            this.RootWidget.AddChildLast(ImageBox_2);
            this.RootWidget.AddChildLast(ImageBox_1);
            this.RootWidget.AddChildLast(Button_1);
            this.RootWidget.AddChildLast(Button_2);
            this.RootWidget.AddChildLast(Button_3);
            this.RootWidget.AddChildLast(Button_4);
            this.RootWidget.AddChildLast(Button_5);
            this.RootWidget.AddChildLast(Button_6);
            this.Transition = new JumpFlipTransition();
            this.Showing += new EventHandler(onShowing);
            this.Shown += new EventHandler(onShown);

            // ImageBox_2
            ImageBox_2.Image = new ImageAsset("/Application/assets/crate_sideup.png");
            ImageBox_2.ImageScaleType = ImageScaleType.Center;

            // ImageBox_1
            ImageBox_1.Image = new ImageAsset("/Application/assets/blade.png");

            // Button_1
            Button_1.TextColor = new UIColor(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            Button_1.TextFont = new UIFont(FontAlias.System, 25, FontStyle.Regular);
            Button_1.Style = ButtonStyle.Custom;
            Button_1.CustomImage = new CustomButtonImageSettings()
            {
                BackgroundNormalImage = new ImageAsset("/Application/assets/crate_sideup.png"),
                BackgroundPressedImage = new ImageAsset("/Application/assets/crate_sidedown.png"),
                BackgroundDisabledImage = null,
                BackgroundNinePatchMargin = new NinePatchMargin(42, 27, 42, 27),
            };

            // Button_2
            Button_2.TextColor = new UIColor(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            Button_2.TextFont = new UIFont(FontAlias.System, 25, FontStyle.Regular);
            Button_2.Style = ButtonStyle.Custom;
            Button_2.CustomImage = new CustomButtonImageSettings()
            {
                BackgroundNormalImage = new ImageAsset("/Application/assets/crate_sideup.png"),
                BackgroundPressedImage = new ImageAsset("/Application/assets/crate_sidedown.png"),
                BackgroundDisabledImage = null,
                BackgroundNinePatchMargin = new NinePatchMargin(42, 27, 42, 27),
            };

            // Button_3
            Button_3.TextColor = new UIColor(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            Button_3.TextFont = new UIFont(FontAlias.System, 25, FontStyle.Regular);
            Button_3.Style = ButtonStyle.Custom;
            Button_3.CustomImage = new CustomButtonImageSettings()
            {
                BackgroundNormalImage = new ImageAsset("/Application/assets/crate_sideup.png"),
                BackgroundPressedImage = new ImageAsset("/Application/assets/crate_sidedown.png"),
                BackgroundDisabledImage = null,
                BackgroundNinePatchMargin = new NinePatchMargin(42, 27, 42, 27),
            };

            // Button_4
            Button_4.TextColor = new UIColor(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            Button_4.TextFont = new UIFont(FontAlias.System, 25, FontStyle.Regular);
            Button_4.Style = ButtonStyle.Custom;
            Button_4.CustomImage = new CustomButtonImageSettings()
            {
                BackgroundNormalImage = new ImageAsset("/Application/assets/crate_sideup.png"),
                BackgroundPressedImage = new ImageAsset("/Application/assets/crate_sidedown.png"),
                BackgroundDisabledImage = null,
                BackgroundNinePatchMargin = new NinePatchMargin(42, 27, 42, 27),
            };

            // Button_5
            Button_5.TextColor = new UIColor(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            Button_5.TextFont = new UIFont(FontAlias.System, 25, FontStyle.Regular);
            Button_5.Style = ButtonStyle.Custom;
            Button_5.CustomImage = new CustomButtonImageSettings()
            {
                BackgroundNormalImage = new ImageAsset("/Application/assets/crate_sideup.png"),
                BackgroundPressedImage = new ImageAsset("/Application/assets/crate_sidedown.png"),
                BackgroundDisabledImage = null,
                BackgroundNinePatchMargin = new NinePatchMargin(42, 27, 42, 27),
            };

            // Button_6
            Button_6.TextColor = new UIColor(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            Button_6.TextFont = new UIFont(FontAlias.System, 25, FontStyle.Regular);
            Button_6.Style = ButtonStyle.Custom;
            Button_6.CustomImage = new CustomButtonImageSettings()
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

                    Button_1.SetPosition(96, 98);
                    Button_1.SetSize(214, 56);
                    Button_1.Anchors = Anchors.None;
                    Button_1.Visible = true;

                    Button_2.SetPosition(313, 89);
                    Button_2.SetSize(214, 56);
                    Button_2.Anchors = Anchors.None;
                    Button_2.Visible = true;

                    Button_3.SetPosition(598, 89);
                    Button_3.SetSize(214, 56);
                    Button_3.Anchors = Anchors.None;
                    Button_3.Visible = true;

                    Button_4.SetPosition(130, 290);
                    Button_4.SetSize(214, 56);
                    Button_4.Anchors = Anchors.None;
                    Button_4.Visible = true;

                    Button_5.SetPosition(373, 418);
                    Button_5.SetSize(214, 56);
                    Button_5.Anchors = Anchors.None;
                    Button_5.Visible = true;

                    Button_6.SetPosition(606, 418);
                    Button_6.SetSize(214, 56);
                    Button_6.Anchors = Anchors.None;
                    Button_6.Visible = true;

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

                    Button_1.SetPosition(373, 68);
                    Button_1.SetSize(214, 56);
                    Button_1.Anchors = Anchors.None;
                    Button_1.Visible = true;

                    Button_2.SetPosition(606, 68);
                    Button_2.SetSize(214, 56);
                    Button_2.Anchors = Anchors.None;
                    Button_2.Visible = true;

                    Button_3.SetPosition(142, 68);
                    Button_3.SetSize(214, 56);
                    Button_3.Anchors = Anchors.None;
                    Button_3.Visible = true;

                    Button_4.SetPosition(142, 418);
                    Button_4.SetSize(214, 56);
                    Button_4.Anchors = Anchors.None;
                    Button_4.Visible = true;

                    Button_5.SetPosition(373, 418);
                    Button_5.SetSize(214, 56);
                    Button_5.Anchors = Anchors.None;
                    Button_5.Visible = true;

                    Button_6.SetPosition(606, 418);
                    Button_6.SetSize(214, 56);
                    Button_6.Anchors = Anchors.None;
                    Button_6.Visible = true;

                    break;
            }
            _currentLayoutOrientation = orientation;
        }

        public void UpdateLanguage()
        {
            this.Title = "Main";

            Button_1.Text = "Continue";

            Button_2.Text = "Load Game";

            Button_3.Text = "New Game";

            Button_4.Text = "Options";

            Button_5.Text = "Extras";

            Button_6.Text = "Quit";
        }

        private void onShowing(object sender, EventArgs e)
        {
            switch (_currentLayoutOrientation)
            {
                case LayoutOrientation.Vertical:
                    ImageBox_2.Visible = false;
                    ImageBox_1.Visible = false;
                    Button_1.Visible = false;
                    Button_2.Visible = false;
                    Button_3.Visible = false;
                    Button_4.Visible = false;
                    Button_5.Visible = false;
                    Button_6.Visible = false;
                    break;

                default:
                    ImageBox_2.Visible = false;
                    ImageBox_1.Visible = false;
                    Button_1.Visible = false;
                    Button_2.Visible = false;
                    Button_3.Visible = false;
                    Button_4.Visible = false;
                    Button_5.Visible = false;
                    Button_6.Visible = false;
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
                        Widget = Button_1,
                        MoveDirection = FourWayDirection.Down,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = Button_2,
                        MoveDirection = FourWayDirection.Down,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = Button_3,
                        MoveDirection = FourWayDirection.Down,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = Button_4,
                        MoveDirection = FourWayDirection.Up,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = Button_5,
                        MoveDirection = FourWayDirection.Up,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = Button_6,
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
                        Widget = Button_1,
                        MoveDirection = FourWayDirection.Down,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = Button_2,
                        MoveDirection = FourWayDirection.Down,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = Button_3,
                        MoveDirection = FourWayDirection.Down,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = Button_4,
                        MoveDirection = FourWayDirection.Up,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = Button_5,
                        MoveDirection = FourWayDirection.Up,
                    }.Start();
                    new SlideInEffect()
                    {
                        Widget = Button_6,
                        MoveDirection = FourWayDirection.Up,
                    }.Start();
                    break;
            }
        }

    }
}
