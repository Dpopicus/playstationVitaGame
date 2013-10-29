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
		private int frameCount;
		private int currentFrame;
		private SpriteUV image;
		
		public Animation ()
		{
			/*
			herp = Support.SpriteUVFromFile ( "Application/assets/combinedImage.jpg", 1, 1 );
			Game.Instance.GameScene.AddChild( herp, 2 );
			herp.UV.T = new Vector2( 0, 0 );	//this is the bottom left which is drawn
			herp.UV.S = new Vector2( .3332f, 1 );	//this is the top right point which is drawn
			*/
		}
		
		public void LoadAnimation( string animationName )
		{
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
				
				Console.Write ("UVMin " + minuv.X + ", " + minuv.Y + ".\n");
				
				Vector2 maxuv = new Vector2();
				maxuv.X = float.Parse (sprite.Element("maxUV").Attribute("x").Value);
				maxuv.Y = float.Parse (sprite.Element("maxUV").Attribute("y").Value);
				
				Console.Write ("View time " + viewtime + ".\n");
				
				Console.Write ("UVMax " + maxuv.X + ", " + maxuv.Y + ".\n");
				
				fr.Set (minuv, maxuv, viewtime);
				frames.Add (fr);
			}
			
			//image.UV.T = frames[0]
			image.UV.T = frames[0].UVMin;
			image.UV.S = frames[0].UVMax;
			image.Quad.S = new Vector2(100, 100);
			
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

