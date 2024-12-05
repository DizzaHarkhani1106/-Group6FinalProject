using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class WoodenPlank
    {
        public Vector2 position { get; private set; }

        public WoodenPlank(Vector2 initialPosition)
        {
            position = initialPosition;
        }

        /*public Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y);
        }*/
    }

}
