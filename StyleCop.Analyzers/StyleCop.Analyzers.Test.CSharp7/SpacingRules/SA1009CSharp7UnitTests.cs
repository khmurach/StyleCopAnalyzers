﻿// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp7.SpacingRules
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using StyleCop.Analyzers.Test.SpacingRules;
    using TestHelper;
    using Xunit;

    public class SA1009CSharp7UnitTests : SA1009UnitTests
    {
        /// <summary>
        /// Verifies spacing around a <c>]</c> character in tuple types and expressions.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        /// <seealso cref="SA1001CSharp7UnitTests.TestBracketsInTupleTypesNotFollowedBySpaceAsync"/>
        /// <seealso cref="SA1011CSharp7UnitTests.TestBracketsInTupleTypesNotFollowedBySpaceAsync"/>
        [Fact]
        public async Task TestBracketsInTupleTypesNotFollowedBySpaceAsync()
        {
            const string testCode = @"using System;

public class Foo
{
    public (int[] , int[] ) TestMethod((int[] , int[] ) a)
    {
        (int[] , int[] ) ints = (new int[][] { new[] { 3 } }[0] , new int[][] { new[] { 3 } }[0] );
        return ints;
    }
}";
            const string fixedCode = @"using System;

public class Foo
{
    public (int[] , int[]) TestMethod((int[] , int[]) a)
    {
        (int[] , int[]) ints = (new int[][] { new[] { 3 } }[0] , new int[][] { new[] { 3 } }[0]);
        return ints;
    }
}";

            DiagnosticResult[] expected =
            {
                this.CSharpDiagnostic().WithLocation(5, 27).WithArguments(" not", "preceded"),
                this.CSharpDiagnostic().WithLocation(5, 55).WithArguments(" not", "preceded"),
                this.CSharpDiagnostic().WithLocation(7, 24).WithArguments(" not", "preceded"),
                this.CSharpDiagnostic().WithLocation(7, 98).WithArguments(" not", "preceded"),
            };

            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpDiagnosticAsync(fixedCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpFixAsync(testCode, fixedCode, cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }

        protected override Solution CreateSolution(ProjectId projectId, string language)
        {
            Solution solution = base.CreateSolution(projectId, language);
            Assembly systemRuntime = AppDomain.CurrentDomain.GetAssemblies().Single(x => x.GetName().Name == "System.Runtime");
            return solution
                .AddMetadataReference(projectId, MetadataReference.CreateFromFile(systemRuntime.Location))
                .AddMetadataReference(projectId, MetadataReference.CreateFromFile(typeof(ValueTuple).Assembly.Location));
        }
    }
}
