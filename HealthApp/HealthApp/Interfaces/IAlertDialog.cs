﻿using System.Threading.Tasks;

namespace HealthApp.Interfaces
{
    public interface IAlertDialog
    {
        Task ShowDialogAsync(string title, string message);
        Task<bool> ShowDialogConfirmationAsync(string title, string message, string accept, string cancel);
    }
}
