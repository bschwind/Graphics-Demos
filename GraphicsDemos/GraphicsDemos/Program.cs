using System;

namespace GraphicsDemos
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GraphicsDemos game = new GraphicsDemos())
            {
                game.Run();
            }
        }
    }
#endif
}

