﻿namespace TestApp.EndToEnd.Tests
{
  using TestApp.EndToEnd.Tests.Infrastructure;
  using OpenQA.Selenium;
  using Shouldly;
  using static Infrastructure.WaitAndAssert;

  public class ExecutionSideTests : BaseTest
  {
    /// <summary>
    ///
    /// </summary>
    /// <param name="aWebDriver"></param>
    /// <param name="aServerFixture">
    /// Is a dependency as the server needs to be running
    /// but is not referenced otherwise thus the injected item is NOT stored
    /// </param>
    public ExecutionSideTests(IWebDriver aWebDriver, ServerFixture aServerFixture)
      : base(aWebDriver, aServerFixture)
    {
      WebDriver = aWebDriver;
      aServerFixture.Environment = AspNetEnvironment.Development;
      aServerFixture.BuildWebHostMethod = Server.Program.BuildWebHost;

      JavaScriptExecutor = WebDriver as IJavaScriptExecutor;
      object clientApplication = JavaScriptExecutor.ExecuteScript("return window.localStorage.getItem('clientApplication');");
      clientApplication.ShouldBe("TestApp.Client.0.0.1");
    }

    private IWebDriver WebDriver { get; }
    private IJavaScriptExecutor JavaScriptExecutor { get; }

    public void LoadsServerSide()
    {
      Navigate("/", aReload: true);
      WaitUntilLoaded();

      JavaScriptExecutor.ExecuteScript("window.localStorage.setItem('executionSide','server');");

      Navigate("/", aReload: true);
      WaitUntilLoaded();

      IWebElement element1 = WebDriver.FindElement(By.CssSelector("[data-qa='BlazorLocation']"));
      element1.Text.ShouldBe("ServerSide");
    }

    public void LoadsClientSide()
    {
      Navigate("/", aReload: true);
      WaitUntilLoaded();

      JavaScriptExecutor.ExecuteScript("window.localStorage.setItem('executionSide','client');");     

      Navigate("/", aReload: true);
      WaitUntilLoaded();

      IWebElement element1 = WebDriver.FindElement(By.CssSelector("[data-qa='BlazorLocation']"));
      element1.Text.ShouldBe("ClientSide");
    }
  }
}