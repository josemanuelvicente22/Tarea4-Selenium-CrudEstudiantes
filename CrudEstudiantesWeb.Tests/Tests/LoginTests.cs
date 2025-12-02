using System;
using CrudEstudiantesWeb.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace CrudEstudiantesWeb.Tests.Tests
{
    [TestClass]
    public class LoginTests : BaseTest
    {
        [TestMethod]
        public void HU01_Login_ConCredencialesCorrectas_DebeEntrarAlCrud()
        {
            try
            {
                // Ir a la pantalla de login
                Driver.Navigate().GoToUrl($"{BaseUrl}/Account/Login");

                // Localizar campos
                var usuarioInput = Driver.FindElement(By.Id("Usuario"));
                var claveInput = Driver.FindElement(By.Id("Clave"));
                var botonEntrar = Driver.FindElement(By.CssSelector("button[type='submit']"));

                // Escribir credenciales válidas
                usuarioInput.Clear();
                usuarioInput.SendKeys("admin");

                claveInput.Clear();
                claveInput.SendKeys("admin123");

                // Enviar formulario
                botonEntrar.Click();

                // Esperar a que se cargue la pantalla del CRUD
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
                wait.Until(ExpectedConditions.ElementIsVisible(By.TagName("h1")));

                var titulo = Driver.FindElement(By.TagName("h1")).Text;

                // Verificar que se ve el listado de estudiantes
                Assert.IsTrue(
                    titulo.Contains("Estudiantes") || titulo.Contains("Listado"),
                    "No se mostró el listado de estudiantes después del login.");

                LogResult(nameof(HU01_Login_ConCredencialesCorrectas_DebeEntrarAlCrud), true,
                    "Login correcto y acceso al CRUD.");
            }
            catch (Exception ex)
            {
                LogResult(nameof(HU01_Login_ConCredencialesCorrectas_DebeEntrarAlCrud), false, ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void HU02_Login_ConCredencialesIncorrectas_DebeMostrarError()
        {
            try
            {
                // Ir a la pantalla de login
                Driver.Navigate().GoToUrl($"{BaseUrl}/Account/Login");

                var usuarioInput = Driver.FindElement(By.Id("Usuario"));
                var claveInput = Driver.FindElement(By.Id("Clave"));
                var botonEntrar = Driver.FindElement(By.CssSelector("button[type='submit']"));

                // Usuario correcto pero clave incorrecta
                usuarioInput.Clear();
                usuarioInput.SendKeys("admin");

                claveInput.Clear();
                claveInput.SendKeys("clave_incorrecta");

                botonEntrar.Click();

                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));

                // Esperar el mensaje de error (alert-danger)
                var alertaError = wait.Until(
                    ExpectedConditions.ElementIsVisible(By.ClassName("alert-danger")));

                var textoError = alertaError.Text;

                Assert.IsTrue(
                    textoError.Contains("incorrect") || textoError.Contains("inválid"),
                    "No se mostró el mensaje de error esperado al fallar el login.");

                LogResult(nameof(HU02_Login_ConCredencialesIncorrectas_DebeMostrarError), true,
                    "Se mostró mensaje de error al usar credenciales incorrectas.");
            }
            catch (Exception ex)
            {
                LogResult(nameof(HU02_Login_ConCredencialesIncorrectas_DebeMostrarError), false, ex.Message);
                throw;
            }
        }
    }
}
