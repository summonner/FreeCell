// Add "DISABLE_LOG" to 'UnityEditor > PlayerSetting > Other Settings > Scripting Define Symbols'
// to disable UnityEngine.Debug.Log()

#if DISABLE_LOG
#undef DO_NOT_PRINT_LOG
using UnityEngine;
using System.Diagnostics;
using Exception = System.Exception;

public static class Debug {
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void Assert( bool condition, string message, Object context ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void Assert( bool condition, object message, Object context ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void Assert( bool condition, string message ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void Assert( bool condition, Object context ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void Assert( bool condition ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void AssertFormat( bool condition, string format, params object[] args ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void AssertFormat( bool condition, Object context, string format, params object[] args ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void Break() { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void ClearDeveloperConsole() { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void DebugBreak() { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void DrawLine( Vector3 start, Vector3 end, Color color, float duration ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void DrawLine( Vector3 start, Vector3 end, Color color ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void DrawLine( Vector3 start, Vector3 end, Color color, float duration, bool depthTest ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void DrawLine( Vector3 start, Vector3 end ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void DrawRay( Vector3 start, Vector3 dir, Color color, float duration ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void DrawRay( Vector3 start, Vector3 dir, Color color, float duration, bool depthTest ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void DrawRay( Vector3 start, Vector3 dir ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void DrawRay( Vector3 start, Vector3 dir, Color color ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void Log( object message, Object context ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void Log( object message ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void LogAssertion( object message, Object context ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void LogAssertion( object message ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void LogAssertionFormat( Object context, string format, params object[] args ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void LogAssertionFormat( string format, params object[] args ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void LogError( object message, Object context ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void LogError( object message ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void LogErrorFormat( string format, params object[] args ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void LogErrorFormat( Object context, string format, params object[] args ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void LogException( Exception exception, Object context ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void LogException( Exception exception ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void LogFormat( Object context, string format, params object[] args ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void LogFormat( string format, params object[] args ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void LogWarning( object message ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void LogWarning( object message, Object context ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void LogWarningFormat( string format, params object[] args ) { }
	[Conditional( "DO_NOT_PRINT_LOG" )]	public static void LogWarningFormat( Object context, string format, params object[] args ) { }
}
#endif