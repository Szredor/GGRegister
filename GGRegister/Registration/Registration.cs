using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace GGRegister
{
    class Registration: IDisposable
    {
        private ChromeDriver driver;
        public Registration()
        {
            driver = new ChromeDriver();
        }

        ~Registration()
        {
            Dispose();
        }

        public void Dispose()
        {
            driver.Close();
        }

        /// <summary>
        /// Pierwsze okno rejestreacji
        /// email i haslo + zaznaczenie checkboxa
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        private void FirstRegistrationWindow(string email, string password)
        {
            WaitUntilClickable(By.CssSelector("#first_step_channel")).Click();
            driver.FindElement(By.CssSelector("#first_step_channel")).Clear();
            driver.FindElement(By.CssSelector("#first_step_channel")).SendKeys(email);
            driver.FindElement(By.CssSelector("#first_step_password")).Click();
            driver.FindElement(By.CssSelector("#first_step_password")).Clear();
            driver.FindElement(By.CssSelector("#first_step_password")).SendKeys(password);

            //Z jakiegoś powodu oryginalny checkbox jest niewidoczny i nalezy naciskac w spana.
            //Popranie checkboxa sluzy tylko do sprawdzenia stanu przelaczenia.
            var checkboxClick = driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[1]/form/div[2]/div/div[2]/label/span"));
            var checkbox = driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[1]/form/div[2]/div/div[2]/label/input"));
            if (!checkbox.Selected)
                checkboxClick.Click();
            driver.FindElement(By.Id("first_step_save")).Click();
        }

        /// <summary>
        /// Drugie okienko rejestracji
        /// imie i nazwisko, data urodzenia[DD.MM.YYYY] oraz miasto.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="birthDate"></param>
        /// <param name="town"></param>
        private void SecondRegistrationWindow(string name, string birthDate, string town)
        {
            WaitUntilVisible(By.Id("fill_data_form_name")).Click();
            driver.FindElement(By.Id("fill_data_form_name")).Clear();
            driver.FindElement(By.Id("fill_data_form_name")).SendKeys(name);
            driver.FindElement(By.Id("fill_data_form_birthday")).Click();
            driver.FindElement(By.Id("fill_data_form_birthday")).Clear();
            driver.FindElement(By.Id("fill_data_form_birthday")).SendKeys(birthDate);
            driver.FindElement(By.Id("fill_data_form_city")).Click();
            driver.FindElement(By.Id("fill_data_form_city")).Clear();
            driver.FindElement(By.Id("fill_data_form_city")).SendKeys(town);
            //Wybór pierwszego miasta z listy - te klasy CSS moga sie zmieniac z biegiem czasu
            WaitUntilClickable(By.CssSelector(".gg.ui.dropdown.list.autocomplete > li:first-child")).Click();
            driver.FindElement(By.Id("fill_data_form_confirm")).Click();
        }

        /// <summary>
        /// Trzecie okienko
        /// Potwierdzenie bycia czlowiekiem poprzez podanie kodu jednorazowego.
        /// </summary>
        /// <param name="phoneNumber"></param>
        private void ThirdRegistrationWindow(string phoneNumber)
        {
            WaitUntilVisible(By.Id("sms_confirm_phone")).Click();
            driver.FindElement(By.Id("sms_confirm_phone")).Clear();
            driver.FindElement(By.Id("sms_confirm_phone")).SendKeys(phoneNumber);
            driver.FindElement(By.Id("sms_confirm_confirm")).Click();
            //Czasami należy ponowić naciśniecie przycisku z numerem
            try
            {
                driver.FindElement(By.Id("sms_confirm_confirm")).Click();
            }
            catch
            { }
        }

        private bool IsRegistrationSuccesful()
        {
            try
            {
                var errors = WaitUntilVisible(By.Id("errors"));
                if (errors == null)
                    return true;
                return false;
            }
            catch 
            {
                return false;
            }
        }

        public bool RegisterAccount(RegisterData data)
        {
            driver.Navigate().GoToUrl("https://login.ggapp.com/en/register2/create-gg?origin=https%3A%2F%2Fwww.ggapp.com");

            //Przejscie procesu rejestracji
            FirstRegistrationWindow(data.Email, data.Password);
            SecondRegistrationWindow(data.FirstLastName, data.BirthDate, data.Town);
            ThirdRegistrationWindow(data.PhoneNumber);

            //Sprawdzić, czy rejestracja zakończyła się pomyślnie
            if (IsRegistrationSuccesful())
            {
                return true;
            }

            return false;
        }

        private IWebElement WaitUntilClickable(By element, int timeout = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                var clickableElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element));
                return clickableElement;
            }
            catch
            {
                return null;
            }
        }

        private IWebElement WaitUntilVisible(By element, int timeout = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                var clickableElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(element));
                return clickableElement;
            }
            catch
            {
                return null;
            }
        }
        
        private IWebElement WaitUntilExists(By element, int timeout = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                var clickableElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(element));
                return clickableElement;
            }
            catch
            {
                return null;
            }
        }
    }
}
