﻿using System.Windows.Input;

namespace ViewModel.MVVMLight
{
    public class RelayCommand : ICommand
    {
        private readonly Action m_Execute;
        private readonly Func<bool> m_CanExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute) : this(execute, null) { }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            m_Execute = execute ?? throw new ArgumentNullException(nameof(execute));
            m_CanExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (m_CanExecute == null)
            {
                return true;
            }
                
            if (parameter == null)
            {
                return m_CanExecute();
            }
                
            return m_CanExecute();
        }

        public virtual void Execute(object parameter)
        {
            m_Execute();
        }

        internal void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}