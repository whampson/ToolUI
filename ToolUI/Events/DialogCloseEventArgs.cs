using System;

namespace WHampson.ToolUI.Events
{
    public class DialogCloseEventArgs : EventArgs
    {
        public bool? DialogResult { get; set; }

        public DialogCloseEventArgs(bool? dialogResult = null)
        {
            DialogResult = dialogResult;
        }
    }
}
