using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlockWorld
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		private SpriteFont _font;
		private KeyboardState _lastKeyboard = Keyboard.GetState();

		private Mode _mode = Mode.Normal;
		private int _frameCounter;
		private double _fps;

		private float _rotation;
		private Effect _myEffect;
		private BasicEffect _effect;

		private VertexBuffer _cubeVertexBuffer;
		private VertexBuffer _instanceVertexBuffer;
		private short[] _cubeIndices;
		private VertexBufferBinding[] _instanceBinding;

		private VertexBuffer _vertexBuffer;
		private IndexBuffer _indexBuffer;
		private VertexPositionNormal[] _vertices;

		private VertexBufferBinding[] _vbBindings;
		private VertexPosition[] _vertexPositions;
		private VertexBuffer _positionBuffer;
		private VertexNormal[] _vertexNormals;
		private VertexBuffer _normalBuffer;

		private short[] _indices;

		private Cube _cube;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";

			graphics.SynchronizeWithVerticalRetrace = false;
			IsFixedTimeStep = false;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			// TODO: Add your initialization logic here
            
			base.Initialize ();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// load our font
			_font = Content.Load<SpriteFont>("mainfont");

			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			//_camera = Matrix.CreateLookAt(new Vector3 (0, -500, 0), Vector3.Zero, Vector3.Up);

			_myEffect = Content.Load<Effect>("instanced");
			var view = Matrix.CreateLookAt(Vector3.Transform(new Vector3(5, 5, 5), Matrix.CreateRotationY(MathHelper.ToRadians(_rotation))), new Vector3(0, 0, 0), Vector3.Up);
			var projection = Matrix.CreateOrthographic(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, -100f, 100.0f);
			var wvp = view * projection;
			_myEffect.Parameters["WorldViewProjection"].SetValue(wvp);
			_myEffect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Identity);

			_myEffect.Parameters["Ambient"].SetValue(0.3f);

			_myEffect.Parameters["LightDir0"].SetValue(new Vector3(0.5f, 0.2f, 0.3f));
			_myEffect.Parameters["LightDir1"].SetValue(new Vector3(-0.5f, 0.1f, -0.3f));
			_myEffect.Parameters["LightDir2"].SetValue(new Vector3(-1f, -1f, -1f));

			_myEffect.Parameters["Diffuse0"].SetValue(new Vector3(0.8f, 0.3f, 0.3f));
			_myEffect.Parameters["Diffuse1"].SetValue(new Vector3(0.3f, 0.8f, 0.3f));
			_myEffect.Parameters["Diffuse2"].SetValue(new Vector3(0.3f, 0.3f, 0.8f));

			_myEffect.Parameters["NumLights"].SetValue(3);


			_effect = new BasicEffect(GraphicsDevice);
			_effect.World = Matrix.Identity;
			_effect.View = Matrix.CreateLookAt(Vector3.Transform(new Vector3(5, 5, 5), Matrix.CreateRotationY(MathHelper.ToRadians(_rotation))), new Vector3(0, 0, 0), Vector3.Up);
			_effect.Projection = Matrix.CreateOrthographic(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, -100f, 100.0f); 

			_effect.EnableDefaultLighting();

			_cube = new Cube {
				Position = Vector3.Zero,
				Size = 16f
			};
			VertexPositionNormal[] tempVerts;
			short[] tempInd;
			_cube.PutVertices(out tempVerts, out _cubeIndices);

			const int cubeAmt = 10000;

			var vlen = tempVerts.Length;
			var ilen = _cubeIndices.Length;
			_vertices = new VertexPositionNormal[cubeAmt * vlen];
			_indices = new short[cubeAmt * ilen];
			for (int i = 0; i < cubeAmt; i++) {
				var vind = vlen * i;
				var iind = ilen * i;
				Array.Copy(tempVerts, 0, _vertices, vind, vlen);
				Array.Copy(_cubeIndices, 0, _indices, iind, ilen);
			}

			_cubeVertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionNormal.VertexDeclaration, tempVerts.Length, BufferUsage.WriteOnly);
			_cubeVertexBuffer.SetData(tempVerts);

			//_instanceVertexBuffer = new VertexBuffer(GraphicsDevice, VertexPosition.VertexDeclaration

			_vertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionNormal.VertexDeclaration, _vertices.Length, BufferUsage.WriteOnly);
			_indexBuffer = new IndexBuffer(GraphicsDevice, typeof(short), _indices.Length, BufferUsage.WriteOnly);

			_vertexBuffer.SetData(_vertices);
			_indexBuffer.SetData(_indices);

			_vertexPositions = new VertexPosition[_vertices.Length];
			_vertexNormals = new VertexNormal[_vertices.Length];
			for (int i = 0; i < _vertices.Length; i++) {
				var v = _vertices[i];
				_vertexPositions[i] = new VertexPosition(v.Position);
				_vertexNormals[i] = new VertexNormal(v.Normal);
			}

			_positionBuffer = new VertexBuffer(GraphicsDevice, VertexPosition.VertexDeclaration, _vertexPositions.Length, BufferUsage.WriteOnly);
			_positionBuffer.SetData(_vertexPositions);

			_normalBuffer = new VertexBuffer(GraphicsDevice, VertexNormal.VertexDeclaration, _vertexNormals.Length, BufferUsage.WriteOnly);
			_normalBuffer.SetData(_vertexNormals);

			_vbBindings = new VertexBufferBinding[] {
				new VertexBufferBinding(_positionBuffer),
				new VertexBufferBinding(_normalBuffer)
			};

			_instanceBinding = new VertexBufferBinding[] {
				new VertexBufferBinding(_cubeVertexBuffer),
				//new VertexBufferBinding(_instanceVertexBuffer)
			};
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			double secs = gameTime.ElapsedGameTime.TotalSeconds;
			_frameCounter++;
			//if ((_frameCounter & 255) == 0)
				_fps = 1.0 / secs;

			var keyboard = Keyboard.GetState();
			if (keyboard.IsKeyDown(Keys.Escape))
				Exit ();

			if (keyboard.IsKeyDown(Keys.E)) {
				_rotation = (_rotation + 100f * (float)secs) % 360;
			} else if (keyboard.IsKeyDown(Keys.Q)) {
				_rotation = (_rotation - 100f * (float)secs) % 360;
			}

			if (keyboard.IsKeyDown (Keys.Left) && _lastKeyboard.IsKeyUp (Keys.Left)) {
				_mode = (Mode) (((int)_mode + 2) % 3);
			} else if (keyboard.IsKeyDown (Keys.Right) && _lastKeyboard.IsKeyUp (Keys.Right)) {
				_mode = (Mode) (((int)_mode + 1) % 3);
			}

			_lastKeyboard = keyboard;

			Window.Title = "GL multiple vbo's test; mode = " + _mode;


			_effect.View = Matrix.CreateLookAt(Vector3.Transform(new Vector3(5, 5, 5), Matrix.CreateRotationY(MathHelper.ToRadians(_rotation))), new Vector3(0, 0, 0), Vector3.Up);
            
			base.Update (gameTime);
		}

		private int lastGCCount;

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			GraphicsDevice.Clear(GC.CollectionCount(0) == lastGCCount ? Color.CornflowerBlue : Color.Red);
			lastGCCount = GC.CollectionCount(0);
            
			spriteBatch.Begin();
			spriteBatch.DrawString(_font, "FPS: " + _fps.ToString("0.00"), new Vector2(graphics.PreferredBackBufferWidth - 60, 15), Color.Black, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
			spriteBatch.DrawString(_font, "Mem: " + (GC.GetTotalMemory(false) / (1024d * 1024)).ToString("N2"), new Vector2(graphics.PreferredBackBufferWidth - 60, 30), Color.Black, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
			spriteBatch.End();


			GraphicsDevice.Indices = _indexBuffer;
			_myEffect.CurrentTechnique.Passes[0].Apply();

			switch (_mode) {
			case(Mode.Normal):
				GraphicsDevice.SetVertexBuffer(_vertexBuffer);
				GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _indices.Length / 3);
				break;

			case(Mode.TwoBuffers):
				GraphicsDevice.SetVertexBuffers(_vbBindings);
				GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _indices.Length / 3);
				break;

			case(Mode.Instanced):
				//GraphicsDevice.SetVertexBuffers (_instanceBinding);
				//GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, _cubeVertexBuffer.VertexCount, 0, _cubeIndices.Length / 3, _instanceVertexBuffer.VertexCount);
				break;
			}
			//GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _indices.Length / 3);
			//GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length, _indices, 0, _indices.Length / 3);                
			//GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _simpleVertices, 0, 1);
            


			base.Draw (gameTime);
		}

		private enum Mode {
			Normal = 0,
			TwoBuffers = 1,
			Instanced = 2
		}
	}
}

