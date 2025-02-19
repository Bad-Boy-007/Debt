using Gameserver.Interfaces;
using Hwdtech.Ioc;
using Hwdtech;
using Moq;
using Gameserver.Commands;


namespace Tests
{
    public class SetShipsCmdTest
    {
        public SetShipsCmdTest()
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
                new Mock<IUObject>().Object, 3)
                .ToList();

            var positerator = new Mock<IEnumerator<object>>();
            positerator.Setup(p => p.Reset()).Verifiable();

            var mockcommand = new Mock<Gameserver.Interfaces.ICommand>();
            mockcommand.Setup(m => m.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Get.Iterator",
                (object[] args) => positerator.Object).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Set.Ship",
                (object[] args) => mockcommand.Object).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Set.Ships",
                (object[] args) => new SetShipsCmd((IEnumerable<IUObject>)args[0])).Execute();

            positerator.Verify(p => p.Reset(), Times.Never());
            mockcommand.Verify(m => m.Execute(), Times.Never());

            IoC.Resolve<Gameserver.Interfaces.ICommand>(
                "Game.Set.Ships",
                uobjects).Execute();

            positerator.Verify(p => p.Reset(), Times.Once());
            mockcommand.Verify(m => m.Execute(), Times.Exactly(3));
        }
    }
}
