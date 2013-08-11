using Components.Aphid.Interpreter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Aphid.Tests.Integration
{
    public class AphidTests
    {
        protected virtual bool LoadStd { get { return false; } }

        protected virtual bool LoadReflection { get { return false; } }

        protected virtual string PrefixScript(string script)
        {
            var includes = "";

            if (LoadStd)
            {
                includes += "#'Std'; ";
            }

            if (LoadReflection)
            {
                includes += "#'Reflection'; ";
            }

            return includes + script;
        }

        private AphidObject Execute(string script)
        {
            script = PrefixScript(script);

            var interpreter = new AphidInterpreter();
            interpreter.Loader.SearchPaths.Add(Path.Combine(Environment.CurrentDirectory, "Library"));
            interpreter.Interpret(script);
            return interpreter.GetReturnValue();
        }

        protected void AssertEquals(object expected, string script)
        {
            Assert.AreEqual(expected, Execute(script).Value);
        }

        protected void AssertFoo(string script)
        {
            AssertEquals("foo", script);
        }

        protected void Assert9(string script)
        {
            AssertEquals(9m, script);
        }

        protected void AssertTrue(string script)
        {
            AssertEquals(true, script);
        }

        protected void AssertFalse(string script)
        {
            AssertEquals(false, script);
        }
    }
}
