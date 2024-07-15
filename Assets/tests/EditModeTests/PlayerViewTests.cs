using NUnit.Framework;
using UnityEngine;
using RMC.Mini;
using RMC.Mini.Model;
using RMC.Mini.Controller.Commands;
using Object = UnityEngine.Object;
using nazaaaar.platformBattle.mini.view;
using nazaaaar.platformBattle.mini.controller.commands;

public class PlayerViewTests
{
    private GameObject playerGameObject;
    private PlayerView playerView;
    private MockContext mockContext;

    [SetUp]
    public void SetUp()
    {
        // Create a new GameObject and add the PlayerView component
        playerGameObject = new GameObject();
        playerView = playerGameObject.AddComponent<PlayerView>();
        
        // Add a Rigidbody component
        var rigidbody = playerGameObject.AddComponent<Rigidbody>();
        playerView.GetType().GetField("rigidbody", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(playerView, rigidbody);
        
        // Create a mock context
        mockContext = new MockContext();
        mockContext.CommandManager = new CommandManager(mockContext);
        playerView.Initialize(mockContext);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(playerGameObject);
    }

    [Test]
    public void PlayerView_Initialize_SetsIsInitialized()
    {
        Assert.IsTrue(playerView.IsInitialized);
    }

    [Test]
    public void PlayerView_Initialize_SetsContext()
    {
        Assert.AreEqual(mockContext, playerView.Context);
    }

    [Test]
    public void PlayerView_MoveSpeedChanged_SetsMoveSpeed()
    {
        mockContext.CommandManager.InvokeCommand(new MoveSpeedChangedCommand {MoveSpeed = 1f});
        Assert.AreEqual(1f, playerView.MoveSpeed);
    }

    [Test]
    public void PlayerView_MoveSpeedChangedTwice_SetsMoveSpeedToLast()
    {
        mockContext.CommandManager.InvokeCommand(new MoveSpeedChangedCommand {MoveSpeed = 1f});
        mockContext.CommandManager.InvokeCommand(new MoveSpeedChangedCommand {MoveSpeed = 7f});
        Assert.AreEqual(7f, playerView.MoveSpeed);
    }

    [Test]
    public void PlayerView_MoveRight_SetsVelocity()
    {
        mockContext.CommandManager.InvokeCommand(new MoveSpeedChangedCommand {MoveSpeed = 5f});
        mockContext.CommandManager.InvokeCommand(new RightPressedCommand { isPressed = true });
        playerView.CheckMovement();
        Assert.AreEqual(new Vector3(5f, 0, 0), playerView.GetComponent<Rigidbody>().velocity);
    }

    [Test]
    public void PlayerView_MoveLeft_SetsVelocity()
    {
        mockContext.CommandManager.InvokeCommand(new MoveSpeedChangedCommand {MoveSpeed = 5f});
        mockContext.CommandManager.InvokeCommand(new LeftPressedCommand { isPressed = true });
        playerView.CheckMovement();
        Assert.AreEqual(new Vector3(-5f, 0, 0), playerView.GetComponent<Rigidbody>().velocity);
    }

    [Test]
    public void PlayerView_MoveUp_SetsVelocity()
    {
        mockContext.CommandManager.InvokeCommand(new MoveSpeedChangedCommand {MoveSpeed = 5f});
        mockContext.CommandManager.InvokeCommand(new UpPressedCommand { isPressed = true });
        playerView.CheckMovement();
        Assert.AreEqual(new Vector3(0, 0, 5f), playerView.GetComponent<Rigidbody>().velocity);
    }

    [Test]
    public void PlayerView_MoveDown_SetsVelocity()
    {
        mockContext.CommandManager.InvokeCommand(new MoveSpeedChangedCommand {MoveSpeed = 5f});
        mockContext.CommandManager.InvokeCommand(new DownPressedCommand { isPressed = true });
        playerView.CheckMovement();
        Assert.AreEqual(new Vector3(0, 0, -5f), playerView.GetComponent<Rigidbody>().velocity);
    }

    [Test]
    public void PlayerView_MoveRight_DisableVelocity()
    {
        mockContext.CommandManager.InvokeCommand(new MoveSpeedChangedCommand {MoveSpeed = 5f});
        mockContext.CommandManager.InvokeCommand(new RightPressedCommand { isPressed = false });
        playerView.CheckMovement();
        Assert.AreEqual(Vector3.zero, playerView.GetComponent<Rigidbody>().velocity);
    }

    [Test]
    public void PlayerView_MoveLeft_DisableVelocity()
    {
        mockContext.CommandManager.InvokeCommand(new MoveSpeedChangedCommand {MoveSpeed = 5f});
        mockContext.CommandManager.InvokeCommand(new LeftPressedCommand { isPressed = false });
        playerView.CheckMovement();
        Assert.AreEqual(Vector3.zero, playerView.GetComponent<Rigidbody>().velocity);
    }

    [Test]
    public void PlayerView_MoveUp_DisableVelocity()
    {
        mockContext.CommandManager.InvokeCommand(new MoveSpeedChangedCommand {MoveSpeed = 5f});
        mockContext.CommandManager.InvokeCommand(new UpPressedCommand { isPressed = false });
        playerView.CheckMovement();
        Assert.AreEqual(Vector3.zero, playerView.GetComponent<Rigidbody>().velocity);
    }

    [Test]
    public void PlayerView_MoveDown_DisableVelocity()
    {
        mockContext.CommandManager.InvokeCommand(new MoveSpeedChangedCommand {MoveSpeed = 5f});
        mockContext.CommandManager.InvokeCommand(new DownPressedCommand { isPressed = false });
        playerView.CheckMovement();
        Assert.AreEqual(Vector3.zero, playerView.GetComponent<Rigidbody>().velocity);
    }

    [Test]
    public void PlayerView_MoveRight_WhileOtherCommandsAreActive()
    {
        mockContext.CommandManager.InvokeCommand(new MoveSpeedChangedCommand {MoveSpeed = 5f});
        // Move Right and Up
        mockContext.CommandManager.InvokeCommand(new RightPressedCommand { isPressed = true });
        mockContext.CommandManager.InvokeCommand(new UpPressedCommand { isPressed = true });
        playerView.CheckMovement();
        Assert.AreEqual(new Vector3(1f, 0, 1f).normalized*5, playerView.GetComponent<Rigidbody>().velocity);
    }

    [Test]
    public void PlayerView_DisableAllCommands_SetsVelocityToZero()
    {
        mockContext.CommandManager.InvokeCommand(new MoveSpeedChangedCommand {MoveSpeed = 5f});
        // Enable Right command
        mockContext.CommandManager.InvokeCommand(new RightPressedCommand { isPressed = true });
        playerView.CheckMovement();
        Assert.AreEqual(new Vector3(5f, 0, 0), playerView.GetComponent<Rigidbody>().velocity);

        // Disable all commands
        mockContext.CommandManager.InvokeCommand(new RightPressedCommand { isPressed = false });
        mockContext.CommandManager.InvokeCommand(new LeftPressedCommand { isPressed = false });
        mockContext.CommandManager.InvokeCommand(new UpPressedCommand { isPressed = false });
        mockContext.CommandManager.InvokeCommand(new DownPressedCommand { isPressed = false });

        playerView.CheckMovement();
        Assert.AreEqual(Vector3.zero, playerView.GetComponent<Rigidbody>().velocity);
    }
    // Mock classes to simulate context and command manager
    private class MockContext : IContext
    {
        private CommandManager _commandManager;

        public Locator<IModel> ModelLocator => throw new System.NotImplementedException();

        public ICommandManager CommandManager
        {
            get;
            set;
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }

}
