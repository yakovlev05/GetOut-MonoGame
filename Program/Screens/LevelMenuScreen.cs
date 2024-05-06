﻿using System;
using System.Collections.Generic;
using System.Linq;
using GetOut.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace GetOut.Program.Screens;

public class LevelMenuScreen : GameScreen
{
    private new Game1 Game => (Game1)base.Game;

    private TiledMap _tiledMap;
    private TiledMapRenderer _tiledMapRenderer;
    private SpriteBatch _spriteBatch;

    private Matrix _matrix;

    private Dictionary<string, TiledMapTileLayer> _buttonsLayers;

    private Dictionary<string, RectangleF> _buttonsRectangles;

    public LevelMenuScreen(Game game) : base(game)
    {
    }

    public override void LoadContent()
    {
        // Настриваем графику
        _tiledMap = Content.Load<TiledMap>("./LevelMenuScreen/chooseLevel");
        _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Делаем расчёт для преобразования карты из 1280*720 в 1920*1080
        var viewport = GraphicsDevice.Viewport;
        var scaleX = (float)viewport.Width / _tiledMap.Width;
        var scaleY = (float)viewport.Height / _tiledMap.Height;
        scaleX = Math.Min(scaleX, 1.5f);
        scaleY = Math.Min(scaleY, 1.5f);
        _matrix = Matrix.CreateScale(scaleX, scaleY, 1);

        // Инициализируем словари, интересуют только кнопки _active
        InitializeButtonsLayers(); // Заполняем словарь, сотояющий название слоя - слой (только кнопки)
        InitializeButtonsRectangles(); // Заполянем словарь, состоящий из название слоя - прямоугольник кнопки

        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        _tiledMapRenderer.Update(gameTime);

        foreach (var button in _buttonsRectangles)
        {
            var rectangle = button.Value;
            if (InputController.IsMouseInRectangle(rectangle))
            {
                _buttonsLayers[button.Key].IsVisible = true;
                if (InputController.IsLeftButtonPressed()) Console.WriteLine(button.Key);
            }
            else _buttonsLayers[button.Key].IsVisible = false;
        }
    }

    public override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _tiledMapRenderer.Draw(_matrix);
        foreach (var rect in _buttonsRectangles)
        {
            _spriteBatch.DrawRectangle(rect.Value, Color.Red);
        }

        _spriteBatch.End();
    }

    private void InitializeButtonsLayers()
    {
        _buttonsLayers = new();

        foreach (var layer in _tiledMap.TileLayers)
        {
            if (!layer.Name.Contains("active")) continue;
            if (layer.Name.Contains("level")) _buttonsLayers.Add(layer.Name, layer);
        }
    }

    private void InitializeButtonsRectangles()
    {
        _buttonsRectangles = new();
        foreach (var layer in _buttonsLayers)
        {
            // if (!layer.Key.Contains("active")) continue;
            var tileButton = layer.Value.Tiles.First(x => x.GlobalIdentifier != 0);
            var buttonVector = Vector2.Transform(new Vector2(tileButton.X * 16, tileButton.Y * 16), _matrix);
            var buttonRectangle = new RectangleF(buttonVector.X, buttonVector.Y, 128 * _matrix.M11, 128 * _matrix.M22);

            _buttonsRectangles.Add(layer.Key, buttonRectangle);
        }
    }
}