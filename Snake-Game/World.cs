﻿using System;
using System.Numerics;
using Engine;
using Engine.Display;

namespace Snake_Game
{
    public class World
    {
        private readonly Random _random = new Random();

        public Vector2 Size { get; }
        public float CellSize { get; }

        public Sprite FoodSprite { get; }
        public World(Vector2 size, float cellSize)
        {
            Size = size;
            CellSize = cellSize;
            var currentFoodLocation = GetRandomFoodLocation();
            FoodSprite = new Sprite("food.png")
            {
                X = currentFoodLocation.X * CellSize, 
                Y = currentFoodLocation.Y * CellSize
            };
            KestrelApp.Stage.AddChild(FoodSprite);
        }

        public void CollectFood()
        {
            var currentFoodLocation = GetRandomFoodLocation();
            FoodSprite.X = currentFoodLocation.X * CellSize;
            FoodSprite.Y = currentFoodLocation.Y * CellSize;
        }

        private Vector2 GetRandomFoodLocation()
        {
            return new Vector2((int)(_random.NextDouble() * Size.X), (int)(_random.NextDouble() * Size.Y));
        }
    }
}