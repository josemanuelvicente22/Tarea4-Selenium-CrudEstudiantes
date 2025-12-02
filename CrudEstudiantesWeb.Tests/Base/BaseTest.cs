using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CrudEstudiantesWeb.Tests.Base
{
    [TestClass]
    public class BaseTest
    {
        protected IWebDriver Driver;
        protected string BaseUrl = "https://localhost:7045"; 

        private static string _reportPath;
        private static StreamWriter _reportWriter;

        private static readonly object _reportLock = new();

        private static void EnsureReportInitialized()
        {
            if (_reportWriter != null) return;

            lock (_reportLock)
            {
                if (_reportWriter != null) return;

                // Ir a la carpeta raíz del proyecto de pruebas
                var projectDir = Path.GetFullPath(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));

                var reportsDir = Path.Combine(projectDir, "Reports");
                Directory.CreateDirectory(reportsDir);

                _reportPath = Path.Combine(reportsDir, "TestReport.html");
                _reportWriter = new StreamWriter(_reportPath, false);

                _reportWriter.WriteLine("<html><head><title>Reporte de Pruebas</title></head><body>");
                _reportWriter.WriteLine("<h1>Reporte de Pruebas Automatizadas</h1>");
                _reportWriter.WriteLine("<table border='1' cellpadding='5' cellspacing='0'>");
                _reportWriter.WriteLine("<tr><th>Fecha/Hora</th><th>Prueba</th><th>Resultado</th><th>Detalle</th></tr>");
                _reportWriter.Flush();
            }
        }



        [TestInitialize]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            Driver = new ChromeDriver(options);
        }

        [TestCleanup]
        public void TearDown()
        {
            try
            {
                if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
                {
                    TakeScreenshot(TestContext.TestName);
                }
            }
            finally
            {
                Driver?.Quit();
                Driver?.Dispose();
            }
        }

        public TestContext TestContext { get; set; }

        protected void LogResult(string testName, bool passed, string detail = "")
        {
            // Asegura que el archivo esté listo
            EnsureReportInitialized();

            var resultText = passed ? "PASÓ" : "FALLÓ";
            var color = passed ? "green" : "red";

            _reportWriter.WriteLine(
                $"<tr><td>{DateTime.Now}</td><td>{testName}</td><td style='color:{color}'>{resultText}</td><td>{detail}</td></tr>");
            _reportWriter.Flush();
        }

        protected void TakeScreenshot(string testName)
        {
            try
            {
                var projectDir = Path.GetFullPath(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));

                var screenshotsDir = Path.Combine(projectDir, "Screenshots");
                Directory.CreateDirectory(screenshotsDir);

                var fileName = $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                var fullPath = Path.Combine(screenshotsDir, fileName);

                var ts = (ITakesScreenshot)Driver;
                var screenshot = ts.GetScreenshot();
                screenshot.SaveAsFile(fullPath);
            }
            catch
            {
                // si falla la captura, no tumbar la prueba
            }
        }

    }
}
