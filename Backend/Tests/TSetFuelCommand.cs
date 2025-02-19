using Gameserver.Interfaces;
using Hwdtech.Ioc;
using Hwdtech;
using Gameserver.Commands;


namespace Tests
{
    public class TSetFuelCommand
    {
        public TSetFuelCommand()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
            ).Execute();
        }

        [Fact]
        public void SetFuelSuccess()
        {
            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Gameserver.UObject.SetProperty",
                (object[] args) => new ActionCommand(
                () => ((IUObject)args[0]).SetProperty((string)args[1], args[2]))).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Gameserver.UObjects.Set.Fuel",
                (object[] args) => new SetFuelForUObjectsCommand((IEnumerable<IUObject>)args[0], (double)args[1])).Execute();

            var uobjects = Enumerable.Range(0, 5).Select(x =>
            {
                var mock = new Mock<IUObject>();
                mock.Setup(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Verifiable();
                return mock;
            }).ToList();

            uobjects.ForEach(
                mock => mock.Verify(
                    x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>()), Times.Never()));

            IoC.Resolve<Gameserver.Interfaces.ICommand>(
                "Gameserver.UObjects.Set.Fuel",
                uobjects.Select(x => x.Object), 200.0).Execute();

            uobjects.ForEach(
                mock => mock.Verify(
                    x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>()), Times.Once()));
        }
    }
}
