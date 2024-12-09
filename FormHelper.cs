using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Library_Otomation
{
    public static class FormHelper
    {
        private static readonly Stack<Form> formStack = new Stack<Form>(); // Form ge�mi�i i�in y���n / Stack for form history

        // Form de�i�ti�inde tetiklenecek olay / Event triggered when the form changes
        public static event Action<Form> OnFormChange;

        // ��k�� yapmak i�in metod / Method to log out
        public static void Logout(Form currentForm)
        {
            DialogResult result = MessageBox.Show("��k�� yapmak istedi�inize emin misiniz?", "��k�� Onay�", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Properties.Settings.Default.Reset(); // Ayarlar� s�f�rla / Reset settings
                currentForm.Hide(); // Ge�erli formu gizle / Hide the current form
                LoginForm loginForm = new LoginForm(); // LoginForm'u ba�lat / Initialize LoginForm
                loginForm.ShowDialog(); // LoginForm'u g�ster / Show LoginForm
            }
        }

        // Yeni formu ge�i� yapmak i�in kullan�l�r / Used to navigate to a new form
        public static void Navigate(Form form)
        {
            formStack.Push(form);  // Yeni formu y���na ekle / Push the new form onto the stack
            OnFormChange?.Invoke(form);  // Form de�i�iklik olay�n� tetikle / Trigger the form change event
        }

        // Bir �nceki forma geri d�nmek i�in kullan�l�r / Used to navigate back to the previous form
        public static void NavigateBack()
        {
            if (formStack.Count > 1)
            {
                formStack.Pop();  // Son eklenen formu ��kar / Pop the last added form
                Form previousForm = formStack.Peek();  // Ge�erli formdan �nceki formu al / Get the previous form from the current form
                OnFormChange?.Invoke(previousForm);  // �nceki formu tetikle / Trigger the previous form
            }
            else
            {
                MessageBox.Show("Geri d�nebilece�iniz bir form yok."); // Geri d�nebilece�iniz bir form yok / There is no form to go back to.
            }
        }
    }
}