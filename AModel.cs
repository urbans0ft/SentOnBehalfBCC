using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SentOnBehalfBCC
{
    public abstract class AModel<T> : DependencyObject
    {
        // Properties
        //public static DependencyProperty MailProperty =
        //    DependencyProperty.Register("Mail", typeof(string), typeof(AddInModel));
        protected readonly Dictionary<string, DependencyProperty> dependencyProperties;
        protected AModel()
        {
            dependencyProperties = new Dictionary<string, DependencyProperty>();
            PropertyInfo[] props = typeof(T).GetProperties(
                BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo prop in props)
            {
                string name = prop.Name;
                Type t      = prop.PropertyType;
                dependencyProperties.Add(name,
                    DependencyProperty.Register(name, t, typeof(T)));
            }
        }
        public DependencyProperty this[string name] {
            get => dependencyProperties[name];
        }
        protected U GetValue<U>([CallerMemberName] string propName = null)
        {
            return (U)GetValue(dependencyProperties[propName]);
        }
        protected void SetValue(object value, [CallerMemberName] string propName = null)
        {
            SetValue(dependencyProperties[propName], value);
        }
    }
}
