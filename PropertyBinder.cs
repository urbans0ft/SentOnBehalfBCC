using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SentOnBehalfBCC
{
    public class PropertyBinder<T>
    {
        private readonly AModel<T>                   target;
        private readonly Dictionary<string, Binding> bindings;
        private readonly List<BindingExpressionBase> bases;
        public PropertyBinder(AModel<T> target)
        {
            this.target   = target;
            this.bindings = new Dictionary<string, Binding>();
            this.bases = new List<BindingExpressionBase>();
        }
        public void Add(string propertyName, object source)
        {
            Binding b = new Binding("Text");
            b.Source  = source;
            b.Mode = BindingMode.TwoWay;
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            bases.Add(BindingOperations.SetBinding(target, target[propertyName], b));
            bindings.Add(propertyName, b);
        }
    }
}
