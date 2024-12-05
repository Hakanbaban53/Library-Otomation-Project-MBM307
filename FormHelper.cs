using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Library_Otomation
{
    public static class FormHelper
    {
        private static readonly Stack<Form> formStack = new Stack<Form>(); // Form ge�mi�i i�in y���n

        // Form de�i�ti�inde tetiklenecek olay
        public static event Action<Form> OnFormChange;

        // ��k�� yapmak i�in metod
        public static void Logout(Form currentForm)
        {
            DialogResult result = MessageBox.Show("��k�� yapmak istedi�inize emin misiniz?", "��k�� Onay�", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Properties.Settings.Default.Reset(); // Ayarlar� s�f�rla
                currentForm.Hide(); // Ge�erli formu gizle
                LoginForm loginForm = new LoginForm(); // LoginForm'u ba�lat
                loginForm.ShowDialog(); // LoginForm'u g�ster
            }
        }

        // Yeni formu ge�i� yapmak i�in kullan�l�r
        public static void Navigate(Form form)
        {
            formStack.Push(form);  // Yeni formu y���na ekle
            OnFormChange?.Invoke(form);  // Form de�i�iklik olay�n� tetikle
        }

        // Bir �nceki forma geri d�nmek i�in kullan�l�r
        public static void NavigateBack()
        {
            if (formStack.Count > 1)
            {
                formStack.Pop();  // Son eklenen formu ��kar
                Form previousForm = formStack.Peek();  // Ge�erli formdan �nceki formu al
                OnFormChange?.Invoke(previousForm);  // �nceki formu tetikle
            }
            else
            {
                MessageBox.Show("Geri d�nebilece�iniz bir form yok.");
            }
        }
    }
}