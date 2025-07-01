// =============================================
// ITestCaseStrategy.cs  - Uses Strategy Pattern
// =============================================

using System;
using System.Collections.Generic;

/// <summary>
/// Interface defining a strategy for executing an elevator test case.
/// </summary>
public interface ITestCaseStrategy
{
    /// <summary>
    /// Executes the test case logic.
    /// </summary>
    void Execute();
}