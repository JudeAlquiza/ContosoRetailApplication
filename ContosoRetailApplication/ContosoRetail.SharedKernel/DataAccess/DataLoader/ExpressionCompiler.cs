using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ContosoRetail.SharedKernel.DataAccess.DataLoader
{
    
    public abstract class ExpressionCompiler
    {
        bool _guardNulls;

        public ExpressionCompiler(bool guardNulls)
        {
            _guardNulls = guardNulls;
        }

        protected bool GuardNulls
        {
            get { return _guardNulls; }
        }

        protected internal Expression CompileAccessorExpression(Expression target, string clientExpr, bool forceToString = false)
        {
            if (clientExpr == "this")
                return target;

            var progression = new List<Expression>();
            progression.Add(target);

            var clientExprItems = clientExpr.Split('.');
            var currentTarget = target;

            for (var i = 0; i < clientExprItems.Length; i++)
            {
                var clientExprItem = clientExprItems[i];

                if (ContosoRetail.SharedKernel.DataAccess.DataLoader.Utils.IsNullable(currentTarget.Type))
                {
                    clientExprItem = "Value";
                    i--;
                }

                currentTarget = Expression.PropertyOrField(currentTarget, clientExprItem);
                progression.Add(currentTarget);
            }

            if (forceToString && currentTarget.Type != typeof(String))
                progression.Add(Expression.Call(currentTarget, "ToString", Type.EmptyTypes));

            return CompileNullGuard(progression);
        }

        Expression CompileNullGuard(IEnumerable<Expression> progression)
        {
            var last = progression.Last();
            var lastType = last.Type;

            if (!_guardNulls)
                return last;

            Expression allTests = null;

            foreach (var i in progression)
            {
                if (i == last)
                    break;

                var type = i.Type;
                if (!ContosoRetail.SharedKernel.DataAccess.DataLoader.Utils.CanAssignNull(type))
                    continue;

                var test = Expression.Equal(i, Expression.Constant(null, type));
                if (allTests == null)
                    allTests = test;
                else
                    allTests = Expression.OrElse(allTests, test);
            }

            return Expression.Condition(
                allTests,
                Expression.Constant(ContosoRetail.SharedKernel.DataAccess.DataLoader.Utils.GetDefaultValue(lastType), lastType),
                last
            );
        }

        protected ParameterExpression CreateItemParam(Type type)
        {
            return Expression.Parameter(type, "obj");
        }

    }

}
