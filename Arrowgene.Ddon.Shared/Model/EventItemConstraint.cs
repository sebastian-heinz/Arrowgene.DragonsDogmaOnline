using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public enum EventItemConstraint
    {
        None,
        All,
        AtLeastOne,
        LessThan,           // The value is < a
        LessThanOrEqual,    // The value <= a
        GreaterThan,        // The value is > a
        GreaterThanOrEqual, // The value is >= a
        InRange,            // The Value is in the range [a, b]
        IsBoss,
        IsNotBoss,
    }
}
