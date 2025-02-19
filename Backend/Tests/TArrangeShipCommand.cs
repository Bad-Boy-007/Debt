using Gameserver.Interfaces;
using Hwdtech.Ioc;
using Hwdtech;
using Gameserver.Commands;


namespace Tests
{
    public class TArrangeShipCommand
    {
        public TArrangeShipCommand()
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
            positerator.SetupGet(x => x.Current).Verifiable();
            positerator.Setup(x => x.MoveNext()).Verifiable();

            var mockcommand = new Mock<Gameserver.Interfaces.ICommand>();
            mockcommand.Setup(x => x.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Gameserver.UObject.SetProperty",
                (object[] args) => mockcommand.Object).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Gameserver.Arrange.Ship",
                (object[] args) => new ArrangeShipCommand((IUObject)args[0], (IEnumerator<object>)args[1])).Execute();

            mockcommand.Verify(x => x.Execute(), Times.Never());

            IoC.Resolve<Gameserver.Interfaces.ICommand>(
                "Gameserver.Arrange.Ship",
                uobject.Object, positerator.Object).Execute();

            mockcommand.Verify(x => x.Execute(), Times.Once());
            positerator.VerifyAll();
        }
    }
}
