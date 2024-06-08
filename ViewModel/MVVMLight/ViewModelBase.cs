using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ViewModel.MVVMLight
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
            {
                return false;
            }
               
            field = value;
            RaisePropertyChanged(propertyName);

            return true;
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (!(propertyExpression.Body is MemberExpression memberExpression))
            {
                throw new ArgumentException("Invalid expression", nameof(propertyExpression));
            }

            string propertyName = memberExpression.Member.Name;
            RaisePropertyChanged(propertyName);
        }
    }
}
