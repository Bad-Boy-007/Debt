using Gameserver.Interfaces;
using Hwdtech.Ioc;
using Hwdtech;
using Gameserver.Commands;


namespace Tests
{
    public class TCreateUObjectCollectionCommand
    {
        public TCreateUObjectCollectionCommand()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
            ).Execute();
        }

        [Fact]
        public void SuccessfulCreatingGameUObjects()
        {
            var uobjectmap = new Dictionary<int, IUObject>();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Gameserver.UObject.Map",
                (object[] args) => uobjectmap).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Gameserver.UObject.Create",
                (object[] args) => new Mock<IUObject>().Object).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Gameserver.UObjects.Collection.Create",
                (object[] args) => new CreateUObjectCollectionCommand((int)args[0])).Execute();

            Assert.Empty(uobjectmap);
            IoC.Resolve<Gameserver.Interfaces.ICommand>(
                "Gameserver.UObjects.Collection.Create", 50).Execute();

            Assert.Equal(50, uobjectmap.Count);
        }
    }
}
