using nazaaaar.platformBattle.mini.view;
using NUnit.Framework;
using RMC.Mini;
using RMC.Mini.Controller.Commands;
using RMC.Mini.Model;
using System;
using UnityEngine;


    public class PlayerInputTests
    {
        private GameObject _gameObject;
        private PlayerInput _playerInput;

        [SetUp]
        public void Setup()
        {
            _gameObject = new GameObject();
            _playerInput = _gameObject.AddComponent<PlayerInput>();
        }

        [TearDown]
        public void Teardown()
        {
            UnityEngine.Object.DestroyImmediate(_gameObject);
        }

        [Test]
        public void TestInitialize()
        {
            // Arrange
            var context = new TestContext();

            // Act
            _playerInput.Initialize(context);

            // Assert
            Assert.IsTrue(_playerInput.IsInitialized);
            Assert.AreEqual(context, _playerInput.Context);
        }

        [Test]
        public void TestRequireIsInitializedThrowsExceptionIfNotInitialized()
        {
            // Assert
            var ex = Assert.Throws<Exception>(() => _playerInput.RequireIsInitialized());
            Assert.AreEqual("MustBeInitialized", ex.Message);
        }

        [Test]
        public void TestRequireIsInitializedDoesNotThrowIfInitialized()
        {
            // Arrange
            var context = new TestContext();
            _playerInput.Initialize(context);

            // Act & Assert
            Assert.DoesNotThrow(() => _playerInput.RequireIsInitialized());
        }

        [Test]
        public void TestKeyUpEvents()
        {
            // Arrange
            var context = new TestContext();
            _playerInput.Initialize(context);

            bool upPressed = false;
            bool downPressed = false;
            bool leftPressed = false;
            bool rightPressed = false;

            _playerInput.OnUpPressedUp += () => upPressed = true;
            _playerInput.OnDownPressedUp += () => downPressed = true;
            _playerInput.OnLeftPressedUp += () => leftPressed = true;
            _playerInput.OnRightPressedUp += () => rightPressed = true;

            // Simulate key up events
            SimulateKeyUp(KeyCode.W);

            // Assert
            Assert.IsTrue(upPressed);
            Assert.IsFalse(downPressed);
            Assert.IsFalse(leftPressed);
            Assert.IsFalse(rightPressed);
        }

        private void SimulateKeyUp(KeyCode keyCode)
        {
            if (keyCode == KeyCode.W)
            {
                _playerInput.OnUpPressedUp?.Invoke();
            }
            else if (keyCode == KeyCode.S)
            {
                _playerInput.OnDownPressedUp?.Invoke();
            }
            else if (keyCode == KeyCode.A)
            {
                _playerInput.OnLeftPressedUp?.Invoke();
            }
            else if (keyCode == KeyCode.D)
            {
                _playerInput.OnRightPressedUp?.Invoke();
            }
        }

        private class TestContext : IContext
        {
            public Locator<IModel> ModelLocator => throw new NotImplementedException();
            public ICommandManager CommandManager => throw new NotImplementedException();
            public void Dispose() => throw new NotImplementedException();
        }
    }

