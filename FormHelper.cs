using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Library_Otomation
{
    public static class FormHelper
    {
        private static readonly Stack<Form> formStack = new Stack<Form>();

        // Form de�i�ti�inde tetiklenecek olay
        public static event Action<Form> OnFormChange;

        // ��k�� yapmak i�in metod
        public static void Logout()
        {
            DialogResult result = MessageBox.Show("��k�� yapmak istedi�inize emin misiniz?", "��k�� Onay�", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Properties.Settings.Default.Reset();
                // Form ge�mi�ini temizle ve LoginForm'a y�nlendir
                ClearHistoryAndNavigate(new LoginForm());
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

        // Ge�mi�i temizle ve yeni bir form y�nlendirmesi yap
        public static void ClearHistoryAndNavigate(Form form)
        {
            formStack.Clear();  // Ge�mi�i temizle
            Navigate(form);  // Yeni formu y�nlendir
 �������}
    }
}