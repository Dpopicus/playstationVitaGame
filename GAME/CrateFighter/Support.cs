//\=====================================
//\Author: Harley Laurie / Daniel Popovic
//\Date Created: 21/10/2013
//\Last Edit: 21/10/2013
//\Brief: In here are a few functions that
//\just wrap up some of the low level stuff
//\done with the vita, to make stuff like
//\drawing sprites and shit a little more simple
//\This file also contains a music/sound manager
//\=====================================

using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace CrateFighter
{
	public class Support
	{
		public static TextureFilterMode DefaultTextureFilterMode = TextureFilterMode.Linear;	//Type of texture filtering
		public static Dictionary<string, Texture2D> TextureCache = new Dictionary<string, Texture2D>();	//A list of textures that have been loaded in
		public static Dictionary<string, TextureInfo> TextureInfoCache = new Dictionary<string, TextureInfo>();	//A list of the information about textures that have been loaded in=
		
		public static Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile SpriteFromFile(string filename)
		{
			if(TextureCache.ContainsKey (filename)	== false)
			{
				TextureCache[filename] = new Texture2D(filename, false);
				TextureInfoCache[filename] = new TextureInfo(TextureCache[filename], new Vector2i(1, 1));
			}
			
			var tex = TextureCache[filename];
			var info = TextureInfoCache[filename];
			var result = new Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile() { TextureInfo = info, };
			
			result.Quad.S = new Vector2(info.Texture.Width, info.Texture.Height);
			
			result.Scale = new Vector2(1.0f);
			
			tex.SetFilter(DefaultTextureFilterMode);
			
			return result;
		}
		
		public static void SetTile(Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile sprite, int n)
		{
            sprite.TileIndex1D = n;
		}
		
		public static int IncrementTile(Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile sprite, int steps, int min, int max, bool looping)
		{
			int x = sprite.TextureInfo.NumTiles.X;
			int y = sprite.TextureInfo.NumTiles.Y;
			
			int current = sprite.TileIndex2D.X + sprite.TileIndex2D.Y * x;
			
			if (looping)
			{
				current -= min;
				current += steps;
				current %= max - min;
				current += min;
			}
			else
			{
				current += steps;
				current = System.Math.Min(current, max - 1);
			}
			
            sprite.TileIndex1D = current;
			
			return current;
		}
		
		public static Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile SpriteFromFile(string filename, float width, float height)
		{
			if(TextureCache.ContainsKey (filename) == false)
			{
				TextureCache[filename] = new Texture2D(filename, false);
				TextureInfoCache[filename] = new TextureInfo(TextureCache[filename], new Vector2i(1, 1));
			}
			
			var tex = TextureCache[filename];
			var info = TextureInfoCache[filename];
			var result = new Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile() { TextureInfo = info, };
			
			result.Quad.S = new Vector2(width, height);
			result.Scale = new Vector2(1.0f);
			tex.SetFilter(DefaultTextureFilterMode);
			return result;
		}
		
		public static Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile SpriteFromFile(string filename, float width, float height, float xPos, float yPos)
		{
			if(TextureCache.ContainsKey (filename) == false)
			{
				TextureCache[filename] = new Texture2D(filename, false);
				TextureInfoCache[filename] = new TextureInfo(TextureCache[filename], new Vector2i(1, 1));
			}
			
			var tex = TextureCache[filename];
			var info = TextureInfoCache[filename];
			var result = new Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile() { TextureInfo = info, };
			
			result.Quad.S = new Vector2(width, height);
			result.Quad.T = new Vector2(xPos, yPos);
			result.Scale = new Vector2(1.0f);
			tex.SetFilter(DefaultTextureFilterMode);
			return result;
		}
		
		public static Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile TiledSpriteFromFile(string filename, int x, int y)
		{
			if (TextureCache.ContainsKey(filename) == false)
			{
				TextureCache[filename] = new Texture2D(filename, false);
				TextureInfoCache[filename] = new TextureInfo(TextureCache[filename], new Vector2i(x, y));
			}
			
			var tex = TextureCache[filename];
			var info = TextureInfoCache[filename];
			var result = new Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile() { TextureInfo = info };

			result.TileIndex2D = new Vector2i(0, 0);

			result.Quad.S = new Vector2(info.Texture.Width / x, info.Texture.Height / y);

			tex.SetFilter(DefaultTextureFilterMode);

			return result;
		}
		
		public static Sce.PlayStation.HighLevel.GameEngine2D.SpriteUV SpriteUVFromFile(string filename, int x, int y)
		{
			if (TextureCache.ContainsKey(filename) == false)
			{
				TextureCache[filename] = new Texture2D(filename, false);
				TextureInfoCache[filename] = new TextureInfo(TextureCache[filename], new Vector2i(x, y));
			}
			
			var tex = TextureCache[filename];
			var info = TextureInfoCache[filename];
			var result = new Sce.PlayStation.HighLevel.GameEngine2D.SpriteUV() { TextureInfo = info };
			
			result.Quad.S = new Vector2(info.Texture.Width / x, info.Texture.Height / y);
			tex.SetFilter(DefaultTextureFilterMode);
			return result;
		}
		
		public class MusicSystem
		{
			public static MusicSystem Instance = new MusicSystem("/Application/assets/");

			public string AssetsPrefix;
			public Dictionary<string, BgmPlayer> MusicDatabase;

			public MusicSystem(string assets_prefix)
			{
				AssetsPrefix = assets_prefix;
				MusicDatabase = new Dictionary<string, BgmPlayer>();
			}

			public void StopAll()
			{
				foreach (KeyValuePair<string, BgmPlayer> kv in MusicDatabase)
				{
					kv.Value.Stop();
					kv.Value.Dispose();
				}

				MusicDatabase.Clear();
			}

			public void Play(string name, bool loop)
			{
				StopAll();

				using (var music = new Bgm(AssetsPrefix + name) )
				{
					var player = music.CreatePlayer();
					MusicDatabase[name] = player;
	
					MusicDatabase[name].Play();
					if (loop)
						MusicDatabase[name].Loop = true;
					MusicDatabase[name].Volume = 0.5f;
				}
			}

			public void Stop(string name)
			{
				StopAll();
			}

			public void PlayNoClobber(string name, bool loop)
			{
				if (MusicDatabase.ContainsKey(name))
				{
					if (MusicDatabase[name].Status == BgmStatus.Playing)
					{
						return;
					}
				}

				Play(name, loop);
			}
		}
	}
}

