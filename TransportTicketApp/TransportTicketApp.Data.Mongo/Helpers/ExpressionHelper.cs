using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TransportTicketApp.Data.Mongo.Helpers
{
    public static class ExpressionHelper
    {
        public static string GetPropertyName<EntityType, EmbeddedDocumentType>(this Expression<Func<EntityType, IEnumerable<EmbeddedDocumentType>>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }

            if (expression.Body is UnaryExpression unaryExpression)
            {
                if (unaryExpression.Operand is MemberExpression unaryMemberExpression)
                {
                    return unaryMemberExpression.Member.Name;
                }
            }

            throw new InvalidOperationException("Expression türü desteklenmiyor");
        }

        public static string GetPropertyName<EmbeddedDocumentType>(this Expression<Func<EmbeddedDocumentType, object>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }

            if (expression.Body is UnaryExpression unaryExpression)
            {
                if (unaryExpression.Operand is MemberExpression unaryMemberExpression)
                {
                    return unaryMemberExpression.Member.Name;
                }
            }

            throw new InvalidOperationException("Expression türü desteklenmiyor");
        }
    }
}
