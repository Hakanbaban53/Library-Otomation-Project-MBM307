using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Library_Otomation
{
    public static class FormHelper
    {
        private static readonly Stack<Form> formStack = new Stack<Form>(); // Form geçmiþi için yýðýn

        // Form deðiþtiðinde tetiklenecek olay
        public static event Action<Form> OnFormChange;

        // Çýkýþ yapmak için metod
        public static void Logout(Form currentForm)
        {
            DialogResult result = MessageBox.Show("Çýkýþ yapmak istediðinize emin misiniz?", "Çýkýþ Onayý", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Properties.Settings.Default.Reset(); // Ayarlarý sýfýrla
                currentForm.Hide(); // Geçerli formu gizle
                LoginForm loginForm = new LoginForm(); // LoginForm'u baþlat
                loginForm.ShowDialog(); // LoginForm'u göster
            }
        }

        // Yeni formu geçiþ yapmak için kullanýlýr
        public static void Navigate(Form form)
        {
            formStack.Push(form);  // Yeni formu yýðýna ekle
            OnFormChange?.Invoke(form);  // Form deðiþiklik olayýný tetikle
        }

        // Bir önceki forma geri dönmek için kullanýlýr
        public static void NavigateBack()
        {
            if (formStack.Count > 1)
            {
                formStack.Pop();  // Son eklenen formu çýkar
                Form previousForm = formStack.Peek();  // Geçerli formdan önceki formu al
                OnFormChange?.Invoke(previousForm);  // Önceki formu tetikle
            }
            else
            {
                MessageBox.Show("Geri dönebileceðiniz bir form yok.");
            }
        }
    }
}