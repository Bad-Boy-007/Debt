using Gameserver.Interfaces;
using Hwdtech.Ioc;
using Hwdtech;
using Moq;
using Gameserver.Commands;


namespace Tests
{
    public class CreateUObjectsCmdTest
    {
        public CreateUObjectsCmdTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
            ).Execute();
        }

        [Fact]
        public void SuccessfulCreatingGameUObjects()
        {
            var uobjectset = new Dictionary<int, IUObject>();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Create.UObjectSet",
                (object[] args) => uobjectset).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Create.UObject",
                (object[] args) => new Mock<IUObject>().Object).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Create.UObjects",
                (object[] args) => new CreateUObjectsCmd((int)args[0])).Execute();

            Assert.Empty(uobjectset);
            IoC.Resolve<Gameserver.Interfaces.ICommand>(
                "Game.Create.UObjects", 3).Execute();

            Assert.Equal(3, uobjectset.Count);
        }
    }
}
