using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics; //Process
//need reference to C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\
//uiautomationclient.dll
//uiautomationprovider.dll
//uiautomationtypes.dll
using System.Windows.Automation; //PropertyCondition, AutomationElement, Automation, InvokePattern

namespace UIAutomation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //open notepad
            Process myNotepad = new Process();
            myNotepad.StartInfo.FileName = "notepad.exe";
            myNotepad.Start();
            myNotepad.WaitForInputIdle();

            //get a handle on the notepad window
            PropertyCondition findWindow = new PropertyCondition(AutomationElement.ClassNameProperty, "Notepad");
            AutomationElement myNotepadWindow = AutomationElement.RootElement.FindFirst(TreeScope.Children, findWindow);

            //make sure we have a valid handle
            if (myNotepadWindow == null)
            {
                MessageBox.Show("invalid notepad window handle");
                return;
            }

            //bring window to front for send keys
            myNotepadWindow.SetFocus();
                        
            //send a text string
            SendKeys.SendWait("testing");

            //close notepad
            PropertyCondition closeButtonCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, "Close");
            AutomationElement closeButton = myNotepadWindow.FindFirst(TreeScope.Descendants, closeButtonCondition);

            //make sure we have a valid handle to the close button
            if (closeButton == null)
            {
                MessageBox.Show("invalid notepad close button handle");
                return;
            }
            //click no to not save the document
            Automation.AddAutomationEventHandler(WindowPattern.WindowOpenedEvent, myNotepadWindow, TreeScope.Descendants, WindowOpenOnNotepadClose);

            //click the close button
            InvokePattern closeButtonInvokePattern = closeButton.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            closeButtonInvokePattern.Invoke();
        }

        private void WindowOpenOnNotepadClose(object sender, AutomationEventArgs e)
        {
            //click dont save the document
            AutomationElement saveWindow = sender as AutomationElement;

            PropertyCondition dontSaveButtonCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, "CommandButton_7");
            AutomationElement dontSaveButton = saveWindow.FindFirst(TreeScope.Descendants, dontSaveButtonCondition);

            if (dontSaveButton != null)
            {
                InvokePattern dontSaveButtonInvokePattern = dontSaveButton.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
                dontSaveButtonInvokePattern.Invoke();
            }

        }
    }
}
