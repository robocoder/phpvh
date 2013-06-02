using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Components.Aphid.Interpreter
{
    public partial class AphidObject : Dictionary<string, AphidObject>
    {
        public object Value { get; set; }

        public AphidObject()
        {
        }

        public AphidObject(object value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("{{ {0}, {1} members }}", Value, Count);
        }

        public List<AphidObject> GetList()
        {
            return Value as List<AphidObject>;
        }

        public IEnumerable<string> GetStringList()
        {
            return GetList().Select(x => x.GetString());
        }

        public string GetString()
        {
            return Value as string;
        }

        public decimal GetNumber()
        {
            return (decimal)Value;
        }

        public bool GetBool()
        {
            return (bool)Value;
        }

        public AphidFunction GetFunction()
        {
            return Value as AphidFunction;
        }

        public void Bind(object obj)
        {
            var bindable = obj as IAphidBindable;

            if (bindable != null)
            {
                bindable.OnBinding(this);
            }

            var kvps = obj
                .GetType()
                .GetProperties()
                .Select(x => new 
                {
                    Property = x,
                    AphidProperty = x
                        .GetCustomAttributes(true)
                        .OfType<AphidPropertyAttribute>()
                        .FirstOrDefault(),
                })
                .Where(x => x.AphidProperty != null)
                .Select(x => new 
                {
                    x.Property,
                    AphidProperty = x.AphidProperty.Name ?? x.Property.Name,
                })
                .Where(x => ContainsKey(x.AphidProperty))
                .Select(x => new
                {
                    Property = x.Property,
                    Obj = this[x.AphidProperty],
                });

            foreach (var p in kvps)
            {
                if (TrySetProperty(p.Property, obj, p.Obj))
                {
                    continue;
                }
                else if (p.Property.PropertyType.IsArray)
                {
                    var elementType = p.Property.PropertyType.GetElementType();
                    var srcArray = p.Obj
                        .GetList()
                        .Select(x =>
                        {
                            var element = Activator.CreateInstance(elementType);
                            x.Bind(element);
                            return element;
                        })
                        .ToArray();
                    var destArray = Array.CreateInstance(elementType, srcArray.Length);
                    Array.Copy(srcArray, destArray, srcArray.Length);

                    p.Property.SetValue(obj, destArray, null);
                }
                else if (p.Property.PropertyType.IsEnum)
                {
                    var val = Enum.ToObject(p.Property.PropertyType, Convert.ToInt64(p.Obj.Value));
                    p.Property.SetValue(obj, val, null);
                }
                else if (p.Obj.Count == 0 ||
                    p.Property.PropertyType == typeof(bool) ||
                    p.Property.PropertyType == typeof(string) ||
                    p.Property.PropertyType == typeof(decimal))
                {
                    p.Property.SetValue(obj, p.Obj.Value, null);
                }
                else
                {
                    var childObj = Activator.CreateInstance(p.Property.PropertyType);
                    p.Obj.Bind(childObj);
                    p.Property.SetValue(obj, childObj, null);
                }
            }

            if (bindable != null)
            {
                bindable.OnBound(this);
            }
        }

        public T ConvertTo<T>()
            where T : new()
        {
            var obj = new T();
            Bind(obj);

            return obj;
        }

        public TElement[] ConvertToArray<TElement>()
            where TElement : new()
        {
            var list = GetList();
            var array = new TElement[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                array[i] = list[i].ConvertTo<TElement>();
            }

            return array;
        }
    }
}
