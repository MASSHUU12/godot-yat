namespace Confirma.Types;

public record TestMethodResult(
	uint TestsPassed,
	uint TestsFailed,
	uint TestsIgnored
);
