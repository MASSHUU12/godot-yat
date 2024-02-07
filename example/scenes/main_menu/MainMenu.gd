extends Control


func _ready() -> void:
	get_node("%Start").connect("pressed", _on_start_pressed)
	get_node("%Quit").connect("pressed", _on_quit_pressed)


func _on_start_pressed() -> void:
	get_tree().change_scene_to_file("res://example/scenes/game/Game.tscn")


func _on_quit_pressed() -> void:
	get_tree().quit()
