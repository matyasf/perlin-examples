using System.Linq;
using Perlin;
using Perlin.Display;
using Snake_Game_V2.Entities;
using Snake_Game_V2.Entities.Snakes;

namespace Snake_Game_V2
{
    public class GameLoop
    {
        private Snake _snake;

        public GameLoop(Snake snake)
        {
            _snake = snake;
        }

        public void Start()
        {
            PerlinApp.Stage.EnterFrameEvent += Step;
        }

        public void Stop()
        {
            PerlinApp.Stage.EnterFrameEvent -= Step;
        }

        public void Step(DisplayObject target, float elapsedTimeSecs)
        {
            _snake.Step();
            foreach (GameEntity gameObject in Globals.Instance.Display.ObjectList)
            {
                if (gameObject is IAnimatable)
                {
                    ((IAnimatable) gameObject).Step();
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
                                ((IInteractable) objToCheck).Apply(otherObj);
                                ((IInteractable) otherObj).Apply(objToCheck);
                            }
                        }
                    }
                }
            }
        }
    }
}