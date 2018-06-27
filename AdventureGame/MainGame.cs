using NinjaGame.Assets;
using NinjaGame.Assets.Management;
using NinjaGame.Config;
using NinjaGame.Graphics2D.Managers;
using NinjaGame.Input;
using NinjaGame.Input.Controllers;
using NinjaGame.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Windows.Forms;
using NinjaGame.Graphics2D.Loading;

namespace NinjaGame
{
    public class MainGame : Game
    {
        public static MainGame Instance
        {
            get
            {
                if (ReferenceEquals(null, _game))
                    _game = new MainGame();

                return _game;
            }
            private set { _game = value; }
        }

        private static MainGame _game;


        public IScene CurrentScene
        {
            get { return _sceneStack.Peek(); }
            set
            {
                if (!ReferenceEquals(null, value))
                    _nextScene = value;
            }
        }

        public IAssetManager AssetManager
        {
            get { return _assetManager; }
            protected set { _assetManager = value; }
        }

        public IGraphics2DManager GraphicsManager
        {
            get { return _graphicsManager; }
            protected set { _graphicsManager = value; }
        }

        public GraphicsDeviceManager Graphics
        {
            get { return _graphics; }
            protected set { _graphics = value; }
        }

        public InputManager InputManaget
        {
            get => _inputManager; 
            protected set => _inputManager = value;
        }

        protected bool popScene;
        protected IScene _nextScene;
        protected IAssetManager _assetManager;
        protected IGraphics2DManager _graphicsManager;
        protected GraphicsDeviceManager _graphics;
        protected InputManager _inputManager;
        protected Stack<IScene> _sceneStack;


        public MainGame()
        {
            MainGame.Instance = this;

            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _sceneStack = new Stack<IScene>();
        }
        protected override void Initialize()
        {
            GlobalConfig.Initialize();

            _inputManager = new InputManager();
            var keyboardController = new KeyboardController();
            _inputManager.Controllers.Add(keyboardController);
            _inputManager.Subscribe_KeyChangeEvent(
                (k, b) =>
                {
                    if (k == Microsoft.Xna.Framework.Input.Keys.Escape && b == ButtonStates.Pressed)
                        UnloadAndExit();
                });

            _assetManager = new AssetManager(Services);
            _graphicsManager = new Graphics2DManager(new Graphic2DLoader(_assetManager));

            _sceneStack.Push(new InitializationScene());

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
        }
        
        protected override void UnloadContent()
        {
            AssetManager.UnloadAll();
        }
        
        protected override void Update(GameTime gameTime)
        {
            _inputManager.Update();
            _sceneStack.Peek().Update(gameTime);

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _sceneStack.Peek().Draw();

            base.Draw(gameTime);

            if (popScene)
                _sceneStack.Pop();
            if (!ReferenceEquals(null, _nextScene))
                _sceneStack.Push(_nextScene);

            _nextScene = null;
            popScene = false;
        }

        public void UnloadAndExit()
        {
            var result = MessageBox.Show("Any unsaved progress may be lost. Are you sure you want to exit?", "Confirm your selection.", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                UnloadContent();
                Exit();
            }
        }

        public void PushScene(IScene nextScene)
        {
            if (!ReferenceEquals(null, nextScene))
                _nextScene = nextScene;
        }

        public void PopScene()
        {
            popScene = true;
        }

        public Vector2 GetScreenCenter()
        {
            var center = new Vector2(
                GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2
                );
            return center;
        }

        public Vector2 GetScreenSize()
        {
            var size = new Vector2(
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height
                );
            return size;
        }

        public void SetResolution(int width = -1, int height = -1)
        {
            if (width == -1)
                width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

            if(height == -1)
                height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            Graphics.PreferredBackBufferWidth = width;
            Graphics.PreferredBackBufferHeight = height;
            Graphics.ApplyChanges();
        }

        public void SetFullScreen(bool fullScreen) 
        {
            Graphics.IsFullScreen = fullScreen;
        }
    }
}
