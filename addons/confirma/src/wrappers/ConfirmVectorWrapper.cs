using Confirma.Extensions;
using Godot;

namespace Confirma.Wrappers;

public partial class ConfirmVectorWrapper : WrapperBase
{
    #region ConfirmEqualApprox
    public static Vector2 ConfirmEqualApprox(
        Vector2 vector,
        Vector2 expected,
        float tolerance = 0.0001f,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmEqualApprox(
                expected,
                tolerance,
                ParseMessage(message)
            )
        );

        return vector;
    }

    public static Vector3 ConfirmEqualApprox(
        Vector3 vector,
        Vector3 expected,
        float tolerance = 0.0001f,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmEqualApprox(
                expected,
                tolerance,
                ParseMessage(message)
            )
        );

        return vector;
    }

    public static Vector4 ConfirmEqualApprox(
        Vector4 vector,
        Vector4 expected,
        float tolerance = 0.0001f,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmEqualApprox(
                expected,
                tolerance,
                ParseMessage(message)
            )
        );

        return vector;
    }
    #endregion ConfirmEqualApprox

    #region ConfirmNotEqualApprox
    public static Vector2 ConfirmNotEqualApprox(
        Vector2 vector,
        Vector2 expected,
        float tolerance = 0.0001f,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmNotEqualApprox(
                expected,
                tolerance,
                ParseMessage(message)
            )
        );

        return vector;
    }

    public static Vector3 ConfirmNotEqualApprox(
        Vector3 vector,
        Vector3 expected,
        float tolerance = 0.0001f,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmNotEqualApprox(
                expected,
                tolerance,
                ParseMessage(message)
            )
        );

        return vector;
    }

    public static Vector4 ConfirmNotEqualApprox(
        Vector4 vector,
        Vector4 expected,
        float tolerance = 0.0001f,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmNotEqualApprox(
                expected,
                tolerance,
                ParseMessage(message)
            )
        );

        return vector;
    }
    #endregion ConfirmNotEqualApprox

    #region ConfirmLessThan
    public static Vector2 ConfirmLessThan(
        Vector2 vector,
        Vector2 expected,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmLessThan(
                expected,
                ParseMessage(message)
            )
        );

        return vector;
    }

    public static Vector3 ConfirmLessThan(
        Vector3 vector,
        Vector3 expected,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmLessThan(
                expected,
                ParseMessage(message)
            )
        );

        return vector;
    }

    public static Vector4 ConfirmLessThan(
        Vector4 vector,
        Vector4 expected,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmLessThan(
                expected,
                ParseMessage(message)
            )
        );

        return vector;
    }
    #endregion ConfirmLessThan

    #region ConfirmLessThanOrEqual
    public static Vector2 ConfirmLessThanOrEqual(
        Vector2 vector,
        Vector2 expected,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmLessThanOrEqual(
                expected,
                ParseMessage(message)
            )
        );

        return vector;
    }

    public static Vector3 ConfirmLessThanOrEqual(
        Vector3 vector,
        Vector3 expected,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmLessThanOrEqual(
                expected,
                ParseMessage(message)
            )
        );

        return vector;
    }

    public static Vector4 ConfirmLessThanOrEqual(
        Vector4 vector,
        Vector4 expected,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmLessThanOrEqual(
                expected,
                ParseMessage(message)
            )
        );

        return vector;
    }
    #endregion ConfirmLessThanOrEqual

    #region ConfirmGreaterThan
    public static Vector2 ConfirmGreaterThan(
        Vector2 vector,
        Vector2 expected,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmGreaterThan(
                expected,
                ParseMessage(message)
            )
        );

        return vector;
    }

    public static Vector3 ConfirmGreaterThan(
        Vector3 vector,
        Vector3 expected,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmGreaterThan(
                expected,
                ParseMessage(message)
            )
        );

        return vector;
    }

    public static Vector4 ConfirmGreaterThan(
        Vector4 vector,
        Vector4 expected,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmGreaterThan(
                expected,
                ParseMessage(message)
            )
        );

        return vector;
    }
    #endregion ConfirmGreaterThan

    #region ConfirmGreaterThanOrEqual
    public static Vector2 ConfirmGreaterThanOrEqual(
        Vector2 vector,
        Vector2 expected,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmGreaterThanOrEqual(
                expected,
                ParseMessage(message)
            )
        );

        return vector;
    }

    public static Vector3 ConfirmGreaterThanOrEqual(
        Vector3 vector,
        Vector3 expected,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmGreaterThanOrEqual(
                expected,
                ParseMessage(message)
            )
        );

        return vector;
    }

    public static Vector4 ConfirmGreaterThanOrEqual(
        Vector4 vector,
        Vector4 expected,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmGreaterThanOrEqual(
                expected,
                ParseMessage(message)
            )
        );

        return vector;
    }
    #endregion ConfirmGreaterThanOrEqual

    #region ConfirmBetween
    public static Vector2 ConfirmBetween(
        Vector2 vector,
        Vector2 min,
        Vector2 max,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmBetween(
                min,
                max,
                ParseMessage(message)
            )
        );

        return vector;
    }

    public static Vector3 ConfirmBetween(
        Vector3 vector,
        Vector3 min,
        Vector3 max,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmBetween(
                min,
                max,
                ParseMessage(message)
            )
        );

        return vector;
    }

    public static Vector4 ConfirmBetween(
        Vector4 vector,
        Vector4 min,
        Vector4 max,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmBetween(
                min,
                max,
                ParseMessage(message)
            )
        );

        return vector;
    }
    #endregion ConfirmBetween

    #region ConfirmNotBetween
    public static Vector2 ConfirmNotBetween(
        Vector2 vector,
        Vector2 min,
        Vector2 max,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmNotBetween(
                min,
                max,
                ParseMessage(message)
            )
        );

        return vector;
    }

    public static Vector3 ConfirmNotBetween(
        Vector3 vector,
        Vector3 min,
        Vector3 max,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmNotBetween(
                min,
                max,
                ParseMessage(message)
            )
        );

        return vector;
    }

    public static Vector4 ConfirmNotBetween(
        Vector4 vector,
        Vector4 min,
        Vector4 max,
        string? message = null
    )
    {
        CallAssertion(
            () => vector.ConfirmNotBetween(
                min,
                max,
                ParseMessage(message)
            )
        );

        return vector;
    }
    #endregion ConfirmNotBetween
}
