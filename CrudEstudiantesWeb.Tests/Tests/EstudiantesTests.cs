using System;
using CrudEstudiantesWeb.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace CrudEstudiantesWeb.Tests.Tests
{
    [TestClass]
    public class EstudiantesTests : BaseTest
    {
       
        // Hace login como admin para poder usar el CRUD.
       
        private void HacerLogin()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Estudiantes");

            Driver.FindElement(By.Id("Usuario")).SendKeys("admin");
            Driver.FindElement(By.Id("Clave")).SendKeys("admin123");
            Driver.FindElement(By.CssSelector("button[type='submit']")).Click();
        }

        [TestMethod]
        public void HU03_CrearEstudiante_ConDatosValidos_DebeAparecerEnListado()
        {
            try
            {
                HacerLogin();

                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));

                // Ir a la vista de creación
                var botonCrear = wait.Until(
                    ExpectedConditions.ElementIsVisible(By.LinkText("Crear nuevo estudiante")));

                botonCrear.Click();

                // Campos del formulario
                var matricula = Driver.FindElement(By.Id("Matricula"));
                var nombre = Driver.FindElement(By.Id("Nombre"));
                var carrera = Driver.FindElement(By.Id("Carrera"));
                var correo = Driver.FindElement(By.Id("Correo"));

                string matriculaTest = "MAT" + DateTime.Now.Ticks.ToString().Substring(10);
                string correoTest = $"test{DateTime.Now.Ticks}@correo.com";

                matricula.SendKeys(matriculaTest);
                nombre.SendKeys("Estudiante Prueba");
                carrera.SendKeys("Ingeniería de Software");
                correo.SendKeys(correoTest);

                Driver.FindElement(By.CssSelector("button[type='submit']")).Click();

                // Verificar que aparezca en el listado
                wait.Until(ExpectedConditions.ElementIsVisible(By.TagName("table")));

                var tabla = Driver.FindElement(By.TagName("table"));
                var textoTabla = tabla.Text;

                Assert.IsTrue(
                    textoTabla.Contains(matriculaTest) && textoTabla.Contains("Estudiante Prueba"),
                    "El estudiante creado no aparece en el listado.");

                LogResult(nameof(HU03_CrearEstudiante_ConDatosValidos_DebeAparecerEnListado), true,
                    "Estudiante creado correctamente y mostrado en el listado.");
            }
            catch (Exception ex)
            {
                LogResult(nameof(HU03_CrearEstudiante_ConDatosValidos_DebeAparecerEnListado), false, ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void HU04_EditarEstudiante_DebeActualizarDatosEnListado()
        {
            try
            {
                HacerLogin();
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));

                // 1) Crear estudiante base
                var botonCrear = wait.Until(
                    ExpectedConditions.ElementIsVisible(By.LinkText("Crear nuevo estudiante")));

                botonCrear.Click();

                var matricula = Driver.FindElement(By.Id("Matricula"));
                var nombre = Driver.FindElement(By.Id("Nombre"));
                var carrera = Driver.FindElement(By.Id("Carrera"));
                var correo = Driver.FindElement(By.Id("Correo"));

                string matriculaTest = "MAT" + DateTime.Now.Ticks.ToString().Substring(10);
                string correoTest = $"edit{DateTime.Now.Ticks}@correo.com";

                matricula.SendKeys(matriculaTest);
                nombre.SendKeys("Estudiante Editar");
                carrera.SendKeys("Software");
                correo.SendKeys(correoTest);

                Driver.FindElement(By.CssSelector("button[type='submit']")).Click();

                // 2) Buscar fila del estudiante recién creado
                wait.Until(ExpectedConditions.ElementIsVisible(By.TagName("table")));

                // Buscar link "Editar" en la fila que contiene la matrícula
                var linkEditar = Driver.FindElement(
                    By.XPath($"//table//tr[td[contains(text(), '{matriculaTest}')]]//a[contains(text(),'Editar')]"));

                linkEditar.Click();

                // 3) Editar nombre
                var nombreEdit = Driver.FindElement(By.Id("Nombre"));
                nombreEdit.Clear();
                nombreEdit.SendKeys("Estudiante Editado");

                Driver.FindElement(By.CssSelector("button[type='submit']")).Click();

                // 4) Verificar cambio en el listado
                wait.Until(ExpectedConditions.ElementIsVisible(By.TagName("table")));
                var tabla = Driver.FindElement(By.TagName("table"));
                var textoTabla = tabla.Text;

                Assert.IsTrue(
                    textoTabla.Contains("Estudiante Editado"),
                    "El cambio de nombre no se reflejó en el listado.");

                LogResult(nameof(HU04_EditarEstudiante_DebeActualizarDatosEnListado), true,
                    "Estudiante editado correctamente.");
            }
            catch (Exception ex)
            {
                LogResult(nameof(HU04_EditarEstudiante_DebeActualizarDatosEnListado), false, ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void HU05_EliminarEstudiante_DebeDesaparecerDelListado()
        {
            try
            {
                HacerLogin();
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));

                // 1) Crear estudiante base
                var botonCrear = wait.Until(
                    ExpectedConditions.ElementIsVisible(By.LinkText("Crear nuevo estudiante")));

                botonCrear.Click();

                var matricula = Driver.FindElement(By.Id("Matricula"));
                var nombre = Driver.FindElement(By.Id("Nombre"));
                var carrera = Driver.FindElement(By.Id("Carrera"));
                var correo = Driver.FindElement(By.Id("Correo"));

                string matriculaTest = "MAT" + DateTime.Now.Ticks.ToString().Substring(10);
                string correoTest = $"del{DateTime.Now.Ticks}@correo.com";

                matricula.SendKeys(matriculaTest);
                nombre.SendKeys("Estudiante Eliminar");
                carrera.SendKeys("Software");
                correo.SendKeys(correoTest);

                Driver.FindElement(By.CssSelector("button[type='submit']")).Click();

                // 2) Buscar link "Eliminar" en la fila del estudiante
                wait.Until(ExpectedConditions.ElementIsVisible(By.TagName("table")));

                var linkEliminar = Driver.FindElement(
                    By.XPath($"//table//tr[td[contains(text(), '{matriculaTest}')]]//a[contains(text(),'Eliminar')]"));

                linkEliminar.Click();

                // 3) Confirmar eliminación (vista Delete)
                var botonConfirmar = wait.Until(
                    ExpectedConditions.ElementIsVisible(By.CssSelector("button[type='submit']")));

                botonConfirmar.Click();

                // 4) Verificar que ya no aparece en el listado
                wait.Until(ExpectedConditions.ElementIsVisible(By.TagName("table")));
                var tabla = Driver.FindElement(By.TagName("table"));
                var textoTabla = tabla.Text;

                Assert.IsFalse(
                    textoTabla.Contains(matriculaTest),
                    "El estudiante todavía aparece en el listado después de eliminar.");

                LogResult(nameof(HU05_EliminarEstudiante_DebeDesaparecerDelListado), true,
                    "Estudiante eliminado correctamente.");
            }
            catch (Exception ex)
            {
                LogResult(nameof(HU05_EliminarEstudiante_DebeDesaparecerDelListado), false, ex.Message);
                throw;
            }
        }
    }
}
