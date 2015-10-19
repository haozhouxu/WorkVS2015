using System;
using System.Windows.Input;
using WpfApplication_MVVM.Models;

namespace WpfApplication_MVVM.ViewModels
{
    public class StudentViewModel
    {
        public DelegateCommand ShowCommand { get; set; }
        public StudentModel Student { get; set; }
        public StudentViewModel()
        {
            Student = new StudentModel();
            ShowCommand = new DelegateCommand();
            ShowCommand.ExecuteCommand = new Action<object>(ShowStudentData);
        }
        private void ShowStudentData(object obj)
        {
            Student.StudentId = 1;
            Student.StudentName = "tiana";
            Student.StudentAge = 20;
            Student.StudentEmail = "8644003248@qq.com";
            Student.StudentSex = "大帅哥";
        }

    }

    public class DelegateCommand : ICommand
    {
        public Action<object> ExecuteCommand = null;
        public Func<object, bool> CanExecuteCommand = null;
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (CanExecuteCommand != null)
            {
                return this.CanExecuteCommand(parameter);
            }
            else
            {
                return true;
            }
        }

        public void Execute(object parameter)
        {
            if (this.ExecuteCommand != null)
            {
                this.ExecuteCommand(parameter);
            }
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
