using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GraphicsDemos.Graphics
{
    public interface IGraphicsObject
    {
        void Update(GameTime g);
        void Draw(GameTime g);
    }
}
