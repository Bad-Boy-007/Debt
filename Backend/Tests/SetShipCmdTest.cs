using Gameserver.Interfaces;
using Hwdtech.Ioc;
using Hwdtech;
using Moq;
using Gameserver.Commands;


namespace Tests
{
    public class SetShipCmdTest
    {
        public SetShipCmdTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
            ).Execute();
        }

        [Fact]
        public void SuccessfulArrangeShip()
        {
            var uobject = new Mock<IUObject>();
            var positerator = new Mock<IEnumerator<object>>();

            positerator.SetupGet(p => p.Current).Verifiable();
            positerator.Setup(p => p.MoveNext()).Verifiable();

            var mockcommand = new Mock<Gameserver.Interfaces.ICommand>();
            mockcommand.Setup(m => m.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.UObject.SetProperty",
                (object[] args) => mockcommand.Object).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Set.Ship",
                (object[] args) => new SetShipCmd((IUObject)args[0], (IEnumerator<object>)args[1])).Execute();

            mockcommand.Verify(m => m.Execute(), Times.Never());

            IoC.Resolve<Gameserver.Interfaces.ICommand>(
                "Game.Set.Ship",
                uobject.Object, positerator.Object).Execute();

            mockcommand.Verify(m => m.Execute(), Times.Once());
            positerator.VerifyAll();
        }
    }
}
