using System;
using NUnit.Framework;
using UnityEngine;
using RMC.Mini;
using RMC.Mini.Controller.Commands;
using RMC.Mini.Model;
using nazaaaar.platformBattle.mini.controller;
using nazaaaar.platformBattle.mini.view;
using nazaaaar.platformBattle.mini.model;

[TestFixture]
public class PlayerMovementControllerTests
{
    private GameObject playerGameObject;
    private PlayerView playerView;
    private PlayerModel playerModel;
    private PlayerInput playerInput;
    private MockContext mockContext;
    private MockCommandManager mockCommandManager;
    private PlayerMovementController playerMovementController;

    [SetUp]
    public void SetUp()
    {
        // Create a new GameObject and add the PlayerView component
        playerGameObject = new GameObject();
        playerView = playerGameObject.AddComponent<PlayerView>();
        playerModel = new PlayerModel();
        playerInput = playerGameObject.AddComponent<PlayerInput>();

        // Create a mock context
        mockContext = new MockContext();
        mockCommandManager = new MockCommandManager(mockContext);
        mockContext.CommandManager = mockCommandManager;

        // Initialize the PlayerMovementController
        playerMovementController = new PlayerMovementController(playerInput, playerView, playerModel);
        playerMovementController.Initialize(mockContext);
    }

    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.DestroyImmediate(playerGameObject);
    }

    [Test]
    public void Initialize_SetsContext()
    {
        Assert.AreEqual(mockContext, playerMovementController.Context);
    }

    [Test]
    public void Initialize_SetsIsInitialized()
    {
        Assert.IsTrue(playerMovementController.IsInitialized);
    }


    [Test]
    public void RequireIsInitialized_ThrowsExceptionWhenNotInitialized()
    {
        playerMovementController = new PlayerMovementController(playerInput, playerView, playerModel);
        Assert.Throws<Exception>(() => playerMovementController.RequireIsInitialized());
    }

    [Test]
    public void RequireIsInitialized_DoesNotThrowExceptionWhenInitialized()
    {
        Assert.DoesNotThrow(() => playerMovementController.RequireIsInitialized());
    }

    // Mock classes for testing
    private class MockContext : IContext
    {
        public ICommandManager CommandManager { get; set; }
        public Locator<IModel> ModelLocator => throw new NotImplementedException();
        public void Dispose() { throw new NotImplementedException(); }
    }

    private class MockCommandManager : CommandManager
    {
        public MockCommandManager(IContext context) : base(context)
        {
        }

    }
}
