using Gameserver.Interfaces;
using Scriban;


namespace Gameserver.Adapters
{
    public class AdapterCreator : ICreator
    {
        private readonly Type _oldtype;
        private readonly Type _newtype;

        public AdapterCreator(Type oldtype, Type newtype)
        {
            _oldtype = oldtype;
            _newtype = newtype;
        }

        public string Create()
        {
            var propertiesNew = _newtype.GetProperties().ToList();

            var templateString = @"public class {{new_type_name}}Adapter : {{new_type_name}} 
{
    {{old_type_name}} _obj;
    public {{new_type_name}}Adapter({{old_type_name}} obj) => _obj = obj;
    {{for property in (properties_new)}}
    public {{property.property_type.name}} {{property.name}}
    {
        {{if property.can_read}}
        get
        {
            return IoC.Resolve<{{property.property_type.name}}>(""Game.UObject.GetProperty"", ""{{property.name}}"", _obj);
        }{{end}}
        {{if property.can_write}}
        set
        {
            return IoC.Resolve<ICommand>(""Game.UObject.SetProperty"", ""{{property.name}}"", _obj, value).Execute();
        }{{end}}
   }
    {{end}}
}";
            var template = Template.Parse(templateString);
            var result = template.Render(new
            {
                new_type_name = _newtype.Name,
                old_type_name = _oldtype.Name,
                properties_new = propertiesNew,
            });
            return result;
        }
    }
}
