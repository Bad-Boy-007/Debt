using Hwdtech.Ioc;
using Hwdtech;
using Moq;
using Gameserver.Commands;
using Gameserver.Interfaces;


namespace Tests
{
    public class LongOperationTest
    {
        public LongOperationTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();


            var repeatCommand = new Mock<Gameserver.Interfaces.ICommand>();
            IoC.Resolve<Hwdtech.ICommand>( "IoC.Register", "Gameserver.Command.Repeat", (object[] args) => repeatCommand.Object).Execute();

            var injectCommand = new Mock<Gameserver.Interfaces.ICommand>();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.Command.Inject", (object[] args) => injectCommand.Object).Execute();

            var macroCommand = new Mock<Gameserver.Interfaces.ICommand>();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.MacroCommand.Create", (object[] args) => macroCommand.Object).Execute();
        }

        [Fact]
        public void LongOperationCreatesWithoutErrors()
        {
            string cmdName = "NameOfDependence";
            var target = new Mock<IUObject>();
            var longOperation = new LongOperation(cmdName, target.Object);

            Assert.NotNull(longOperation.Execute());
        }
    }
}
