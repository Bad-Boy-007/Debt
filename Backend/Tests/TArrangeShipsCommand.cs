using Gameserver.Interfaces;
using Hwdtech.Ioc;
using Hwdtech;
using Gameserver.Commands;

namespace Tests
{
    public class TArrangeShipsCommand
    {
        public TArrangeShipsCommand()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New",
            IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void SuccessfulArrangeShips()
        {
            var uobjects = Enumerable.Repeat(
                new Mock<IUObject>().Object, 7)
                .ToList();

            var positerator = new Mock<IEnumerator<object>>();
            positerator.Setup(x => x.Reset()).Verifiable();

            var mockcommand = new Mock<Gameserver.Interfaces.ICommand>();
            mockcommand.Setup(x => x.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Gameserver.Position.Iterator",
                (object[] args) => positerator.Object).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
                "Gameserver.Arrange.Ship",
                (object[] args) => mockcommand.Object).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Gameserver.Arrange.Ships", (object[] args) => new ArrangeShipsCommand((IEnumerable<IUObject>)args[0])).Execute();

            positerator.Verify(x => x.Reset(), Times.Never());
            mockcommand.Verify(x => x.Execute(), Times.Never());

            IoC.Resolve<Gameserver.Interfaces.ICommand>(
                "Gameserver.Arrange.Ships",
                uobjects).Execute();

            positerator.Verify(x => x.Reset(), Times.Once());
            mockcommand.Verify(x => x.Execute(), Times.Exactly(7));
        }
    }
}
