using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Library_Otomation
{
    public static class FormHelper
    {
        private static readonly Stack<Form> formStack = new Stack<Form>(); // Form geçmiþi için yýðýn / Stack for form history

        // Form deðiþtiðinde tetiklenecek olay / Event triggered when the form changes
        public static event Action<Form> OnFormChange;

        // Çýkýþ yapmak için metod / Method to log out
        public static void Logout(Form currentForm)
        {
            DialogResult result = MessageBox.Show("Çýkýþ yapmak istediðinize emin misiniz?", "Çýkýþ Onayý", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Properties.Settings.Default.Reset(); // Ayarlarý sýfýrla / Reset settings
                currentForm.Hide(); // Geçerli formu gizle / Hide the current form
                LoginForm loginForm = new LoginForm(); // LoginForm'u baþlat / Initialize LoginForm
                loginForm.ShowDialog(); // LoginForm'u göster / Show LoginForm
            }
        }

        // Yeni formu geçiþ yapmak için kullanýlýr / Used to navigate to a new form
        public static void Navigate(Form form)
        {
            formStack.Push(form);  // Yeni formu yýðýna ekle / Push the new form onto the stack
            OnFormChange?.Invoke(form);  // Form deðiþiklik olayýný tetikle / Trigger the form change event
        }

        // Bir önceki forma geri dönmek için kullanýlýr / Used to navigate back to the previous form
        public static void NavigateBack()
        {
            if (formStack.Count > 1)
            {
                formStack.Pop();  // Son eklenen formu çýkar / Pop the last added form
                Form previousForm = formStack.Peek();  // Geçerli formdan önceki formu al / Get the previous form from the current form
                OnFormChange?.Invoke(previousForm);  // Önceki formu tetikle / Trigger the previous form
            }
            else
            {
                MessageBox.Show("Geri dönebileceðiniz bir form yok."); // Geri dönebileceðiniz bir form yok / There is no form to go back to.
            }
        }
    }
}