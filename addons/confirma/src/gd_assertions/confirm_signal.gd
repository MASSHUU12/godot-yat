@tool
class_name ConfirmSignal

static var exts: CSharpScript = load(
	GdHelper.get_plugin_path() + "wrappers/ConfirmSignalWrapper.cs"
).new()


static func exists(
	actual: Object,
	signalName: StringName,
	message: String = ""
) -> Object:
	exts.ConfirmSignalExists(actual, signalName, message)

	return actual


static func does_not_exist(
	actual: Object,
	signalName: StringName,
	message: String = ""
) -> Object:
	exts.ConfirmSignalDoesNotExist(actual, signalName, message)

	return actual
