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

        public bool RegisterAccount(RegisterData data, bool waitForRodo = false)
        {
            driver.Navigate().GoToUrl("https://login.ggapp.com/en/register2/create-gg?origin=https%3A%2F%2Fwww.ggapp.com");

            /*
             * Pierwsze okno rejestreacji
             * email i haslo + zaznaczenie checkboxa
             */
            WaitUntilClickable(By.CssSelector("#first_step_channel")).Click();
            driver.FindElement(By.CssSelector("#first_step_channel")).Clear();
            driver.FindElement(By.CssSelector("#first_step_channel")).SendKeys(data.Email);
            driver.FindElement(By.CssSelector("#first_step_password")).Click();
            driver.FindElement(By.CssSelector("#first_step_password")).Clear();
            driver.FindElement(By.CssSelector("#first_step_password")).SendKeys(data.Password);
            var checkbox = driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[1]/form/div[2]/div/div[2]/label/span"));
            if (checkbox.GetCssValue("checked") != "true")
                checkbox.Click();
            driver.FindElement(By.Id("first_step_save")).Click();

            /*
             * Drugie okienko rejestracji
             * imie i nazwisko, data urodzenia[DD.MM.YYYY] oraz miasto.
             */
            WaitUntilVisible(By.Id("fill_data_form_name")).Click();
            driver.FindElement(By.Id("fill_data_form_name")).Clear();
            driver.FindElement(By.Id("fill_data_form_name")).SendKeys(data.FirstLastName);
            driver.FindElement(By.Id("fill_data_form_birthday")).Click();
            driver.FindElement(By.Id("fill_data_form_birthday")).Clear();
            driver.FindElement(By.Id("fill_data_form_birthday")).SendKeys(data.BirthDate);
            driver.FindElement(By.Id("fill_data_form_city")).Click();
            driver.FindElement(By.Id("fill_data_form_city")).Clear();
            driver.FindElement(By.Id("fill_data_form_city")).SendKeys(data.Town);
            //Wybór pierwszego miasta z listy - te klasy CSS moga sie zmieniac z biegiem czasu
            WaitUntilClickable(By.CssSelector(".gg.ui.dropdown.list.autocomplete > li:first-child")).Click();
            driver.FindElement(By.Id("fill_data_form_confirm")).Click();

            /*
             * Trzecie okienko
             * Potwierdzenie bycia czlowiekiem poprzez podanie kodu jednorazowego.
             */
            WaitUntilVisible(By.Id("sms_confirm_phone")).Click();
            driver.FindElement(By.Id("sms_confirm_phone")).Clear();
            driver.FindElement(By.Id("sms_confirm_phone")).SendKeys(data.PhoneNumber);
            driver.FindElement(By.Id("sms_confirm_confirm")).Click();

            //Tutaj dodac mechanizm sprawdzania, czy sie udalo zarejestrowac
            //Jezeli sie udalo, to poczekac na wpisanie danych przez czlowieka (kod potwierdzajacy)
            //Moze wydac jakis dzwiek przy udanym?


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
            catch (Exception e)
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
            catch (Exception e)
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
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
