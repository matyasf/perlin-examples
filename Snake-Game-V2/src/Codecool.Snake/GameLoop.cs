using System.Linq;
using Codecool.Snake.Entities;
using Perlin;
using Perlin.Display;

namespace Codecool.Snake
{
    /// <summary>
    /// The game loop, you can start/stop the game here.
    /// </summary>
    public class GameLoop
    {
        private Entities.Snakes.Snake _snake;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameLoop"/> class.
        /// </summary>
        /// <param name="snake">The snake to display</param>
        public GameLoop(Entities.Snakes.Snake snake)
        {
            _snake = snake;
        }

        /// <summary>
        /// Adds an event listener that executes the Step() method every frame
        /// </summary>
        public void Start()
        {
            PerlinApp.Stage.EnterFrameEvent += Step;
        }

        /// <summary>
        /// Stops the Step() method executing every frame.
        /// </summary>
        public void Stop()
        {
            PerlinApp.Stage.EnterFrameEvent -= Step;
        }

        /// <summary>
        /// This code gets executed every frame.
        /// </summary>
        private void Step(DisplayObject target, float elapsedTimeSecs)
        {
            _snake.Step();
            foreach (GameEntity gameObject in Globals.Instance.Display.ObjectList)
            {
                if (gameObject is IAnimatable)
                {
                    ((IAnimatable)gameObject).Step();
                }
            }

            CheckCollisions();
            Globals.Instance.Display.FrameFinished();
        }

        private void CheckCollisions()
        {
            var gameObjs = Globals.Instance.Display.ObjectList;
            for (int idxToCheck = 0; idxToCheck < gameObjs.Count; idxToCheck++)
            {
                var objToCheck = gameObjs.ElementAt(idxToCheck);
                if (objToCheck is IInteractable)
                {
                    for (int otherObjIdx = idxToCheck + 1; otherObjIdx < gameObjs.Count; otherObjIdx++)
                    {
                        var otherObj = gameObjs.ElementAt(otherObjIdx);
                        if (otherObj is IInteractable)
                        {
                            if (objToCheck.GetBounds(objToCheck.Parent).Intersects(otherObj.GetBounds(otherObj.Parent)))
                            {
                                ((IInteractable)objToCheck).Apply(otherObj);
                                ((IInteractable)otherObj).Apply(objToCheck);
                            }
                        }
                    }
                }
            }
        }
    }
}