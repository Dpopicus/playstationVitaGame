//\=====================================
//\Author: Harley Laurie / Daniel Popovic
//\Date Created: 21/10/2013
//\Last Edit: 21/10/2013
//\Brief: Takes in sprite sheets + xml files
//\from harleys sprite sheet tool and creates
//\animations out of them
//\=====================================

using System;
using System.Collections;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using System.Xml.Linq;
using System.IO;

namespace CrateFighter
{
	public class Animation
	{
		private List<Frame> frames;
		public int frameCount;
		public int currentFrame;
		private SpriteUV image;
		private float viewTime;	//This is how many frames this animation has been viewing for
		public bool LastFrame { get; set; }
		
		public Animation ()
		{
			viewTime = new float();
		}
		
		public void SetView( bool aView )
		{//Takes in a bool, if false hides the animation, if true it views it
			image.Visible = aView;
		}
		
		public void FaceRight( bool aFlip )
		{//Takes in bool, if false it faces right, if true it faces left
			image.FlipU = aFlip;
		}
		
		public void Move( Vector2 newPos )
		{
			image.Quad.T = newPos;
		}
		
		public void Resize( int width, int height )
		{//Changes the size of this sprite
			image.Quad.S = new Vector2 ( width, height );
		}
		
		public void Play()
		{//This function will be called 60 times per second (unless the frame rate falls below 60)
			//so we will time the frames off that
			image.Visible = true;
			viewTime += 0.01666f; // 1 / 60 = .016
			if ( viewTime >= ( frames[currentFrame-1].viewTime ) )
			{
				viewTime = 0;
				ChangeFrame();
			}
		}
		
		public void ChangeFrame()
		{//This changes to the next frame of the animation
			//or back to the first one if we reach the end
			if ( currentFrame == frames.Count )
			{//Enters here if we have reached the end of the animation
				LastFrame = true;
				currentFrame = 1;
			}
			else
			{
				++currentFrame;
			}
			
			image.UV.T = frames[currentFrame-1].UVMin;
			//for some reason you only have to change the minUV coordinates
			//to swap to the next frame.  
			//This is very strange, as leaving the maxUV at the default value
			//and only changing to minimum values, it should make the image
			//cut of more and more of the image as the animation goes along
			//But, it works like this for some reason, so we will just leave it.
		}
		
		public void LoadAnimation( string animationName )
		{//When passed in the name of an animation (both the xml and the sprite sheet must have the same name)
			//this will load in all the information from the xml file and prepare the sprite sheet to be used for animations
			frames = new List<Frame>();
			image = Support.SpriteUVFromFile( ("Application/assets/animations/" + animationName + ".jpg"), 1, 1 );
			FileStream fileStream = File.Open( ("/Application/assets/animations/" + animationName + ".xml"), FileMode.Open, FileAccess.Read);
			StreamReader fileStreamReader = new StreamReader(fileStream);
			string xml = fileStreamReader.ReadToEnd ();
			fileStreamReader.Close ();
			fileStream.Close ();
			XDocument doc = XDocument.Parse (xml);
			
			frameCount = int.Parse (doc.Root.Attribute("frame-count").Value);
			
			foreach ( var sprite in doc.Root.Elements("frame"))
			{
				Frame fr = new Frame();
				
				float viewtime = new float();
				viewtime =   float.Parse (sprite.Attribute("time").Value);
				
				Vector2 minuv = new Vector2();
				minuv.X = float.Parse (sprite.Element("minUV").Attribute("x").Value);
				minuv.Y = float.Parse (sprite.Element("minUV").Attribute("y").Value);
				
				Vector2 maxuv = new Vector2();
				maxuv.X = float.Parse (sprite.Element("maxUV").Attribute("x").Value);
				maxuv.Y = float.Parse (sprite.Element("maxUV").Attribute("y").Value);
				
				fr.Set (minuv, maxuv, viewtime);
				frames.Add (fr);
			}
			
			//When the animation starts, we want to set the UV coordinates to
			//the first frame in the animation
			image.UV.T = frames[0].UVMin;
			image.UV.S = frames[0].UVMax;
			image.UV.S.Y = 1.0f;
			currentFrame = 1;	//Make a note of what frame of the animation we are on
			image.Quad.S = new Vector2(100, 100);//This is the size of the sprite
			
			Game.Instance.GameScene.AddChild(image);
		}
	}
	
	public class Frame
	{
		public Vector2 UVMin;
		public Vector2 UVMax;
		public float viewTime;
		
		public Frame()
		{
		}
		
		public void Set( Vector2 a_UVMin, Vector2 a_UVMax, float a_viewTime )
		{
			this.UVMin = a_UVMin;
			this.UVMax = a_UVMax;
			this.viewTime = a_viewTime;
		}
	}
}

