using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using Gameserver.Interfaces;
using Gameserver.Commands;

namespace Tests
{
    public class EndMoveCmdTest
    {
        private static void SetUpEndCommandTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.Command.Inject", (object[] args) =>
            {
                var target = (IInjectable)args[0];
                var injectedCommand = (Gameserver.Interfaces.ICommand)args[1];
                target.Inject(injectedCommand);
                return target;
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.UObject.DeleteProperty", (object[] args) =>
            {
                var target = (IUObject)args[0];
                var properties = (List<string>)args[1];
                properties.ForEach(prop => target.DeleteProperty(prop));
                return "";
            }).Execute();
        }

        [Fact]
        public void TestEndMovementCommand()
        {
            SetUpEndCommandTest();
            var mockEndable = new Mock<IEndable>();
            var mockCommand = new Mock<Gameserver.Interfaces.ICommand>();
            var injectCommand = new InjectCmd(mockCommand.Object);
            var target = new Mock<IUObject>();
            var keys = new List<string>() { "Movement" };
            var characteristics = new Dictionary<string, object>();

            var queue = new Mock<IQueue>();

            queue.Setup(q => q.Add(It.IsAny<Gameserver.Interfaces.ICommand>())).Verifiable();
            mockCommand.Setup(x => x.Execute()).Callback(() => { queue.Object.Add(injectCommand); });

            target.Setup(t => t.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>((key, value) => characteristics.Add(key, value));
            target.Setup(t => t.DeleteProperty(It.IsAny<string>())).Callback<string>((string key) => characteristics.Remove(key));
            target.Setup(t => t.GetProperty(It.IsAny<string>())).Returns((string key) => characteristics[key]);
            target.Object.SetProperty("Movement", 1);

            mockEndable.SetupGet(e => e.Command).Returns(injectCommand);
            mockEndable.SetupGet(e => e.Object).Returns(target.Object);
            mockEndable.SetupGet(e => e.Property).Returns(keys);

            var endmovementcommand = new EndMoveCmd(mockEndable.Object);

            injectCommand.Execute();
            queue.Verify(q => q.Add(injectCommand), Times.Once());
            endmovementcommand.Execute();
            injectCommand.Execute();
            queue.Verify(q => q.Add(injectCommand), Times.Once());
            Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => target.Object.GetProperty("Movement"));
        }

        [Fact]
        public void TestInjectCommand()
        {
            SetUpEndCommandTest();

            var mockCommand = new Mock<Gameserver.Interfaces.ICommand>();
            mockCommand.Setup(x => x.Execute()).Verifiable();

            var injectCommand = new InjectCmd(mockCommand.Object);
            injectCommand.Inject(new EmptyCmd());
            injectCommand.Execute();

            mockCommand.Verify(m => m.Execute(), Times.Never());
        }

        [Fact]
        public void EndMovementCommandDisabilityToDeletePropertiesCausesExeption()
        {
            SetUpEndCommandTest();

            var mockEndable = new Mock<IEndable>();
            var mockCommand = new Mock<Gameserver.Interfaces.ICommand>();
            var injectCommand = new InjectCmd(mockCommand.Object);
            var target = new Mock<IUObject>();
            var keys = new List<string>() { "NoExistentProperty" };

            target.Setup(t => t.DeleteProperty(It.IsAny<string>())).Callback(() => throw new Exception());

            mockEndable.SetupGet(e => e.Command).Returns(injectCommand);
            mockEndable.SetupGet(e => e.Object).Returns(target.Object);
            mockEndable.SetupGet(e => e.Property).Returns(keys);

            var endmovementcomman = new EndMoveCmd(mockEndable.Object);

            Assert.Throws<Exception>(() => endmovementcomman.Execute());
        }
    }
}
