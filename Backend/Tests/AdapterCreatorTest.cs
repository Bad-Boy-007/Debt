using Hwdtech.Ioc;
using Hwdtech;
using Xunit.Abstractions;
using Gameserver.Interfaces;
using Gameserver.Strategies;


namespace Tests
{
    public class AdapterCreatorTest
    {
        private readonly ITestOutputHelper outhelp;

        public AdapterCreatorTest(ITestOutputHelper output)
        {
            outhelp = output;

            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New",
            IoC.Resolve<object>("Scopes.Root"))).Execute();
        }


        [Fact]
        public void SuccessfullAdapterCreatorTest()
        {
            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.AdapterCreator",
                (object[] args) => new GetAdapterCreatorStrategy().Strategy(args)).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.AdapterCreator.Get.Code",
                (object[] args) => new GetCodeFromAdapterCreatorStrategy().Strategy(args)).Execute();

            var oldtype = typeof(IUObject);
            var newtype = typeof(IRotation);

            var result = IoC.Resolve<string>("Game.AdapterCreator.Get.Code", oldtype, newtype);

            var expected = @"public class IRotationAdapter : IRotation 
{
    IUObject _obj;
    public IRotationAdapter(IUObject obj) => _obj = obj;
    
        public Angle moveangle
        {
            
                get
                {
                    return IoC.Resolve<Angle>(""Game.UObject.GetProperty"", ""moveangle"", _obj);
                }
            
                set
                {
                    return IoC.Resolve<ICommand>(""Game.UObject.SetProperty"", ""moveangle"", _obj, value).Execute();
                }
       }
        
        public Angle speedangle
        {
            
                get
                {
                    return IoC.Resolve<Angle>(""Game.UObject.GetProperty"", ""speedangle"", _obj);
                }
            
       }
        
}";

            outhelp.WriteLine(result);
            Assert.Equal(expected, result);
        }


    }
}
