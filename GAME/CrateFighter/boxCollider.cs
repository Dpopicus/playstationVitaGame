//\=====================================
//\Author: Harley Laurie
//\Date Created: 21/10/2013
//\Last Edit: 21/10/2013
//\Brief: box collider class
//\IT WORKS PERFECTLY DONT TOUCH IT DAN LOL
//\=====================================

using System;
using Sce.PlayStation.Core;

namespace CrateFighter
{
	public class boxCollider
	{
		public Vector2 position;
		public int width;
		public int height;
		
		public boxCollider ()
		{
			position = new Vector2();
			width = new int();
			height = new int();
		}
		
		public void Set( Vector2 pos, int w, int h )
		{
			this.position = pos;
			this.width = w;
			this.height = h;
		}
		
		public bool isColliding( boxCollider rhs )
		{//if any sides are colliding return true, else false
			
			if ( this.leftCollide(rhs) )
				return true;
			if ( this.rightCollide(rhs) )
				return true;
			if ( this.topCollide(rhs) )
				return true;
			if ( this.bottomCollide(rhs) )
				return true;
			
			return false;
		}
		
		public bool leftCollide( boxCollider rhs )
		{
			if ( this.position.X <= ( rhs.position.X + rhs.width ) )
			{
				if ( this.position.X >= rhs.position.X)
				{
					if ( ( this.position.Y + this.height ) > rhs.position.Y )
					{
						if ( ( this.position.Y + this.height ) < ( rhs.position.Y + rhs.height ) )
							return true;
					}
					if ( this.position.Y < ( rhs.position.Y + rhs.height ) )
					{
						if ( this.position.Y > rhs.position.Y )
							return true;
					}
				}
			}
			return false;
		}
		
		public bool rightCollide( boxCollider rhs )
		{
			if ( ( this.position.X + this.width ) >= rhs.position.X )
			{
				if ( ( this.position.X + this.width ) <= ( rhs.position.X + rhs.width ) )
				{
					if ( ( this.position.Y + this.height ) > rhs.position.Y )
					{
						if ( ( this.position.Y + this.height ) < ( rhs.position.Y + rhs.height ) )
							return true;
					}
					if ( this.position.Y < ( rhs.position.Y + rhs.height ) )
					{
						if ( this.position.Y > rhs.position.Y )
							return true;
					}
				}
			}
			return false;
		}
		
		public bool topCollide( boxCollider rhs )
		{
			if ( ( this.position.Y + this.height ) >= rhs.position.Y )
			{
				if ( ( this.position.Y + this.height ) <= ( rhs.position.Y + rhs.height ) )
				{
					if ( this.position.X < ( rhs.position.X + rhs.width ) )
					{
						if ( this.position.X > rhs.position.X)
							return true;
					}
					if ( ( this.position.X + this.width ) > rhs.position.X )
					{
						if ( ( this.position.X + this.width ) < ( rhs.position.X + rhs.width ) )
							return true;
					}
				}
			}
			return false;
		}
		
		public bool bottomCollide( boxCollider rhs )
		{
			if ( this.position.Y <= ( rhs.position.Y + rhs.height ) )
			{
				if ( this.position.Y >= rhs.position.Y )
				{
					if ( this.position.X < ( rhs.position.X + rhs.width ) )
					{
						if ( this.position.X > rhs.position.X)
							return true;
					}
					if ( ( this.position.X + this.width ) > rhs.position.X )
					{
						if ( ( this.position.X + this.width ) < ( rhs.position.X + rhs.width ) )
							return true;
					}	
				}
			}
			return false;
		}
	}
}

